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
                if(staff == null || staff.LibraryStaff == null)
                {
                    _logger.LogInformation($"Cannot add new loan, staff is missing");
                    throw new ArgumentException("Cannot add new loan, staff is missing");
                }

                foreach (var bookLoanDetails in loan.BookLoanDetails)
                {
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

                // TODO: Check also day of lend
                var staffLendCount = _readerLoanRepository.Get(s => s.StaffId == loan.StaffId, includeProperties: "BookLoanDetails").Count();
                _librarySettingsService.CheckIfUserCanBorrowBooks(user, loan, previousLoans, staffLendCount);

            }
            catch (Exception)
            {

                throw;
            }

            return loan;
        }


    }
}
