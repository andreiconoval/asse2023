
using Library.BL.Interfaces;
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;

namespace Library.BL.Services;

public class LibrarySettingsService : ILibrarySettingsService
{
    private readonly ILibrarySettingsRepository _librarySettingsRepository;

    #region Public fields

    public int DOMENII { get => LibrarySettings.MaxDomains; }

    public int NMC { get => LibrarySettings.MaxBookBorrowed * USER_IND; }

    public int PER { get => LibrarySettings.BorrowedBooksPeriod / USER_IND; }

    public int C { get => LibrarySettings.MaxBooksBorrowedPerTime * USER_IND; }

    public int D { get => LibrarySettings.MaxAllowedBooksPerDomain * USER_IND; }

    public int L { get => LibrarySettings.AllowedMonthsForSameDomain; }

    public int LIM { get => LibrarySettings.BorrowedBooksExtensionLimit * USER_IND; }

    public int DELTA { get => LibrarySettings.SameBookRepeatBorrowingLimt / USER_IND; }

    public int NCZ { get => LibrarySettings.MaxBorrowedBooksPerDay; }

    public int PERSIMP { get => LibrarySettings.LimitBookLend; }

    public int USER_IND { get; set; } = 1;

    public LibrarySettings LibrarySettings { get; set; }

    #endregion Public fields

    public LibrarySettingsService(ILibrarySettingsRepository librarySettingsRepository)
    {
        _librarySettingsRepository = librarySettingsRepository ?? throw new ArgumentNullException();
        LibrarySettings = _librarySettingsRepository.Get();
    }

    /// <summary>
    /// Check base on library settings is user can borrow books or not
    /// </summary>
    /// <param name="user">User that with borrow</param>
    /// <param name="newLoan">New redear loan</param>
    /// <param name="previousLoans">Previous user loans</param>
    /// <param name="staffLendCount">Staff lend count for today</param>
    /// <exception cref="ArgumentException"></exception>
    public void CheckIfUserCanBorrowBooks(User user, ReaderLoan newLoan, List<ReaderLoan> previousLoans, int staffLendCount)
    {
        if (user.LibraryStaff == null)
        {
            USER_IND = 2;
        }
        else
        {
            USER_IND = 1;
        }
        // Implementare pentru cerinte si limitari

        // Verificare limita NMC in perioada PER
        var loansInPeriod = previousLoans.Count(pl => (DateTime.Now - pl.LoanDate).TotalDays <= PER);
        if (loansInPeriod + 1 > NMC)
        {
            throw new ArgumentException("Exceeded maximum books borrowed in the specified period.");
        }

        // Verificare limita C
        if (newLoan.BookLoanDetails.Count() > C)
        {
            throw new ArgumentException("Exceeded maximum books borrowed per time.");
        }

        // Verificare categorii distincte daca sunt cel putin 3 carti imprumutate
        if (newLoan.BookLoanDetails.Count() >= 3)
        {
            var distinctCategoriesCount = newLoan.BookLoanDetails.Select(bld => bld.BookSample.BookEdition.Book.BookDomains.Select(bd => bd.DomainId)).Distinct().Count();
            if (distinctCategoriesCount < 2)
            {
                throw new ArgumentException("At least 2 distinct categories are required for borrowing 3 or more books.");
            }
        }

        // Verificare limita D pentru carti din acelasi domeniu in ultimele L luni
        var currentDate = DateTime.Now;
        var lastMonthsDate = currentDate.AddMonths(-L);
        var recentBorrowedBooks = previousLoans.SelectMany(pl => pl.BookLoanDetails)
                                              .Where(bld => bld.LoanDate >= lastMonthsDate && bld.LoanDate <= currentDate)
                                              .ToList();

        var borrowedBooksFromSameDomain = recentBorrowedBooks.Count(bld => bld.BookSample.BookEdition.Book.BookDomains.Any(bd => bd.DomainId == newLoan.BookLoanDetails.First().BookSample.BookEdition.Book.BookDomains.First().DomainId));
        if (borrowedBooksFromSameDomain + newLoan.BookLoanDetails.Count() > D)
        {
            throw new ArgumentException("Exceeded maximum allowed books from the same domain in the specified period.");
        }

        // Verificare limita DELTA pentru aceeasi carte
        var lastBorrowedBook = previousLoans.SelectMany(pl => pl.BookLoanDetails).OrderByDescending(bld => bld.LoanDate).FirstOrDefault();
        if (lastBorrowedBook != null && (DateTime.Now - lastBorrowedBook.LoanDate).Days < DELTA)
        {
            throw new ArgumentException("Cannot borrow the same book within the specified interval.");
        }

        // Verificare limita pentru personalul bibliotecii
        if (staffLendCount > PERSIMP)
        {
            throw new ArgumentException("Exceeded maximum books that library staff can lend in a day.");
        }

        var loansForNewLoanDay = previousLoans.Where(pl => pl.LoanDate.Date == newLoan.LoanDate.Date).SelectMany(pl => pl.BookLoanDetails).Count();

        if (user.LibraryStaff == null &&
            (newLoan.BookLoanDetails.Count() > NCZ || loansForNewLoanDay + newLoan.BookLoanDetails.Count() > NCZ))
        {
            throw new ArgumentException("Reader exceed limit for today");
        }
    }
}
