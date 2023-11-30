using System.Data.Entity;
using Library.DAL.DomainModel;

namespace Library.DAL.DataMapper
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Domain> Domains { get; set; }
        public DbSet<BookDomain> BooksDomains { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BooksAuthors { get; set; }
        public DbSet<BookEdition> BookEditions { get; set; }
        public DbSet<BookSample> BookSamples { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<LibraryStaff> LibraryStaff { get; set; }
        public DbSet<ReaderLoan> ReaderLoans { get; set; }
        public DbSet<BookLoanDetail> BookLoanDetails { get; set; }


        public LibraryDbContext()
    : base("myConStr")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Book>()
                .HasMany(b => b.BookDomains)
                .WithRequired()
                .HasForeignKey(bd => bd.BookId);

            modelBuilder.Entity<Domain>()
                .HasMany(d => d.BookDomains)
                .WithRequired()
                .HasForeignKey(bd => bd.DomainId);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.BookAuthors)
                .WithRequired()
                .HasForeignKey(ba => ba.BookId);

            modelBuilder.Entity<Author>()
                .HasMany(a => a.BookAuthors)
                .WithRequired()
                .HasForeignKey(ba => ba.AuthorId);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.BookEditions)
                .WithRequired()
                .HasForeignKey(be => be.BookId);

            modelBuilder.Entity<BookEdition>()
                .HasMany(be => be.BookSamples)
                .WithRequired()
                .HasForeignKey(bs => bs.BookEditionId);

            modelBuilder.Entity<User>()
                .HasOptional(u => u.Reader)
                .WithRequired(r => r.User);

            modelBuilder.Entity<User>()
                .HasOptional(u => u.LibraryStaff)
                .WithRequired(ls => ls.User);

            modelBuilder.Entity<Reader>()
                .HasMany(r => r.ReaderLoans)
                .WithRequired(rl => rl.Reader);

            modelBuilder.Entity<ReaderLoan>()
                .HasMany(rl => rl.BookLoanDetails)
                .WithRequired(bld => bld.ReaderLoan);
        }
    }
}
