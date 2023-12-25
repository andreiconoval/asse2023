
using Library.DAL.DomainModel;
using Library.DAL.Interfaces;
using log4net.Core;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;

namespace Library.BL.Services;

public class LibrarySettingsService
{
    private readonly ILibrarySettingsRepository _librarySettingsRepository;

    #region Public fields

    public int DOMENII { get => LibrarySettings.MaxDomains; }

    public int NMC { get => LibrarySettings.MaxBookBorrowed; }

    public int PER { get => LibrarySettings.BorrowedBooksPeriod; }

    public int C { get => LibrarySettings.MaxBooksBorrowedPerTime; }

    public int D { get => LibrarySettings.MaxAllowedBooksPerDomain; }

    public int L { get => LibrarySettings.AllowedMonthsForSameDomain; }

    public int LIM { get => LibrarySettings.BorrowedBooksExtensionLimit; }

    public int DELTA { get => LibrarySettings.SameBookRepeatBorrowingLimt; }

    public int NCZ { get => LibrarySettings.MaxBorrowedBooksPerDay; }

    public int PERSIMP { get => LibrarySettings.LimitBookLend; }

    public LibrarySettings LibrarySettings { get; set; }

    #endregion Public fields

    public LibrarySettingsService(ILibrarySettingsRepository _librarySettingsRepository)
    {
        _librarySettingsRepository = _librarySettingsRepository ?? throw new ArgumentNullException();
        LibrarySettings = _librarySettingsRepository.Get();
    }

    public bool CanBorrowBooks(User user, ReaderLoan newLoan, List<ReaderLoan> previousLoans, int staffLendCount)
    {
        var canBorrow = true;

        var loansForNewLoanDay = previousLoans.Where(pl => pl.LoanDate.Date == newLoan.LoanDate.Date).SelectMany(pl => pl.BookLoanDetails).Count();

        if(staffLendCount > PERSIMP)
        {
            throw new ArgumentException("Staff exceed  lend limit for today");
        }

        if (user.LibraryStaff == null && 
            (newLoan.BookLoanDetails.Count() > NCZ || loansForNewLoanDay + newLoan.BookLoanDetails.Count() > NCZ))
        {
            throw new ArgumentException("Reader exceed limit for today");
        }



        return canBorrow;
    }

    // Pot imprumuta un numar maxim de carti NMC intr-o perioada PER

    // La un imprumut pot prelua cel mult C carti; daca numarul cartilor imprumutate la o
    // cerere de imprumut e cel putin 3, atunci acestea trebui sa faca parte din cel putin 2
    // categorii distincte

    // Nu pot imprumuta mai mult de D carti dintr-un acelasi domeniu – de tip frunza sau de
    // nivel superior - in ultimele L luni

    // Pot imprumuta o carte pe o perioada determinata; se permit prelungiri, dar suma
    // acestor prelungiri acordate in ultimele 3 luni nu poate depasi o valoare limita LIM data

    // Nu pot imprumuta aceeasi carte de mai multe ori intr-un interval DELTA specificat, unde
    // DELTA se masoara de la ultimul imprumut al cartii

    // - Pot imprumuta cel mult NCZ carti intr-o zi.

    // Despre personalul bibliotecii:
    // Daca sunt inregistrati drept cititori cu acelasi cont, atunci in cazul lor valorile pragurilor
    // NMC, C, D, LIM se dubleaza, DELTA si PER se injumatatesc.
    // - Nu pot acorda mai mult de PERSIMP carti intr-o zi; limita NCZ se ignora pentru ei.


}
