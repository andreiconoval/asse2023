using FluentValidation;
using Library.BL.Interfaces;
using Library.BL.Validators;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Library.BL.Services
{
    public class ReaderLoanService : IReaderLoanService
    {
        private readonly IBookLoanDetailRepository _bookLoanDetailRepository;
        private readonly IReaderLoanRepository _readerLoanRepository;
        private readonly IBookSampleRepository _bookSampleRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILibrarySettingsService _librarySettingsService;
        protected IValidator<ReaderLoan> _readerLoanValidator;
        protected IValidator<BookLoanDetail> _bookLoanDetailValidator;
        protected ILogger _logger;

        public ReaderLoanService(
            IReaderLoanRepository readerLoanRepository,
            IBookLoanDetailRepository bookLoanDetailRepository,
            IBookSampleRepository bookSampleRepository,
            ILibrarySettingsService librarySettingsService,
            IUserRepository userRepository,
            ILogger logger)
        {
            _bookLoanDetailRepository = bookLoanDetailRepository;
            _readerLoanRepository = readerLoanRepository;
            _bookSampleRepository = bookSampleRepository;
            _userRepository = userRepository;
            _librarySettingsService = librarySettingsService;
            _readerLoanValidator = new ReaderLoanValidator();
            _bookLoanDetailValidator = new BookLoanDetailValidator();
            _logger = logger;

        }

        /// <summary>
        /// Insert new reader loan
        /// </summary>
        /// <param name="loan">New reader loan</param>
        /// <returns>Reader loan with filled id</returns>
        /// <exception cref="ArgumentException">Exceptions if something is missing or restricted</exception>
        public ReaderLoan Insert(ReaderLoan loan)
        {
            try
            {
                loan.LoanDate = DateTime.Now;
                var result = _readerLoanValidator.Validate(loan);

                if (!result.IsValid)
                {
                    _logger.LogInformation($"Cannot add new loan, entity is invalid");
                    throw new ArgumentException("Cannot add new loan, entity is invalid");
                }

                var user = _userRepository.Get(u => u.Id == loan.ReaderId, includeProperties: "LibraryStaff,Reader").FirstOrDefault();
                if (user == null || user.Reader == null)
                {
                    _logger.LogInformation($"Cannot add new loan, reader is missing");
                    throw new ArgumentException("Cannot add new loan, reader is missing");
                }

                var staff = _userRepository.Get(u => u.Id == loan.StaffId, includeProperties: "LibraryStaff").FirstOrDefault();
                if (staff == null || staff.LibraryStaff == null)
                {
                    _logger.LogInformation($"Cannot add new loan, staff is missing");
                    throw new ArgumentException("Cannot add new loan, staff is missing");
                }

                foreach (var bookLoanDetails in loan.BookLoanDetails)
                {
                    bookLoanDetails.LoanDate = DateTime.Now;
                    var resultBookLoanDetails = _bookLoanDetailValidator.Validate(bookLoanDetails);
                    if (!resultBookLoanDetails.IsValid)
                    {
                        _logger.LogInformation($"Cannot add new book loan detail, entity is invalid");
                        throw new ArgumentException("Cannot add new book loan detail, entity is invalid");
                    }

                    var bookSample = _bookSampleRepository.Get(bs => bookLoanDetails.BookSampleId == bs.Id, includeProperties: "BookEdition,Book").FirstOrDefault();
                    if (bookSample == null)
                    {
                        _logger.LogInformation($"Cannot add new book loan detail, bookSample is invalid");
                        throw new ArgumentException("Cannot add new book loan detail, bookSample is invalid");
                    }

                    bookLoanDetails.BookSample = bookSample;
                    bookLoanDetails.BookEditionId = bookSample.BookEditionId;
                    bookLoanDetails.BookId = bookSample.BookEdition.BookId;
                }

                var previousLoans = _readerLoanRepository.Get(u => u.ReaderId == loan.ReaderId, includeProperties: "BookLoanDetails").ToList();

                var dateNow = DateTime.Now.Date;
                var staffLendCount = _readerLoanRepository.Get(s => s.StaffId == loan.StaffId && s.LoanDate.Date == dateNow, includeProperties: "BookLoanDetails").Count();
                _librarySettingsService.CheckIfUserCanBorrowBooks(user, loan, previousLoans, staffLendCount);


                _readerLoanRepository.Insert(loan);

                foreach (var item in loan.BookLoanDetails)
                {
                    item.ReaderLoanId = loan.Id;
                    _bookLoanDetailRepository.Insert(item);
                }

                return loan;
            }
            catch (Exception)
            {
                throw;
            }


        }

        /// <summary>
        /// Mark reader loan as returned for all book
        /// </summary>
        /// <param name="readerLoanId"> Reader loan id</param>
        /// <exception cref="ArgumentException"> Exception if book loan is missing </exception>
        public void Delete(int readerLoanId)
        {
            var readerLoan = _readerLoanRepository.Get(u => u.Id == readerLoanId, includeProperties: "BookLoanDetails").FirstOrDefault();
            if (readerLoan == null)
            {
                _logger.LogInformation($"Cannot delete book loan, book loan is invalid");
                throw new ArgumentException("Cannot delete book loan, book loan is invalid");
            }

            foreach (var item in readerLoan.BookLoanDetails)
            {
                item.EffectiveReturnDate ??= DateTime.Now;
                _bookLoanDetailRepository.Update(item);
            }
        }

        /// <summary>
        /// Extend Book loan details period from Now
        /// </summary>
        /// <param name="bookLoanDetailId"> Book loan detail id</param>
        /// <exception cref="ArgumentException">Extensions limit and missing entity exception</exception>
        public void SetExtensionsForLoan(int bookLoanDetailId)
        {
            var bookLoanDetail = _bookLoanDetailRepository.Get(bld => bld.Id == bookLoanDetailId, includeProperties: "ReaderLoan").FirstOrDefault();
            if (bookLoanDetail == null)
            {
                _logger.LogInformation($"Cannot update book loan detail, book loan is invalid");
                throw new ArgumentException("Cannot update book loan detail, book loan is invalid");
            }

            var previousLoans = _readerLoanRepository.Get(u => u.ReaderId == bookLoanDetail.ReaderLoan.ReaderId, includeProperties: "BookLoanDetails").ToList();

            // Verificare limita LIM pentru prelungiri
            var totalExtensionsInLastThreeMonths = previousLoans.Sum(pl => pl.ExtensionsGranted);
            if (totalExtensionsInLastThreeMonths + 1 > _librarySettingsService.LIM)
            {
                throw new ArgumentException("Exceeded maximum allowed extensions for borrowed books in the last three months.");
            }

            bookLoanDetail.ExpectedReturnDate = DateTime.Now;
            bookLoanDetail.ReaderLoan.ExtensionsGranted += 1;

            _bookLoanDetailRepository.Update(bookLoanDetail);
            _readerLoanRepository.Update(bookLoanDetail.ReaderLoan);
        }
    }
}
