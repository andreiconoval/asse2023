using Library.DAL.DomainModel;

namespace Library.BL.Interfaces;

public interface ILibrarySettingsService
{
    int C { get; }
    int D { get; }
    int DELTA { get; }
    int DOMENII { get; }
    int L { get; }
    LibrarySettings LibrarySettings { get; set; }
    int LIM { get; }
    int NCZ { get; }
    int NMC { get; }
    int PER { get; }
    int PERSIMP { get; }
    int USER_IND { get; set; }

    void CheckIfUserCanBorrowBooks(User user, ReaderLoan newLoan, List<ReaderLoan> previousLoans, int staffLendCount);
}