using Library.DAL.DomainModel;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Library.DAL.DataMapper
{
    [ExcludeFromCodeCoverage]
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
        public DbSet<LibrarySettings> LibrarySettings { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=asse_library;user=root;password=mysqladmin");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Book>()
                .HasMany(b => b.BookDomains)
                .WithOne()
                .HasForeignKey(bd => bd.BookId);

            modelBuilder.Entity<Domain>()
                .HasMany(d => d.BookDomains)
                .WithOne()
                .HasForeignKey(bd => bd.DomainId);
           
            modelBuilder.Entity<Domain>()
               .HasMany(d => d.Subdomains)
               .WithOne()
               .HasForeignKey(bd => bd.DomainId);

            modelBuilder.Entity<Domain>()
                .HasOne(ba => ba.ParentDomain)
                .WithMany(b => b.Subdomains)
                .HasForeignKey(ba => ba.DomainId);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.BookAuthors)
                .WithOne()
                .HasForeignKey(ba => ba.BookId);

            modelBuilder.Entity<Author>()
                .HasMany(a => a.BookAuthors)
                .WithOne()
                .HasForeignKey(ba => ba.AuthorId);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.BookEditions)
                .WithOne()
                .HasForeignKey(be => be.BookId);

            modelBuilder.Entity<BookEdition>()
                .HasMany(be => be.BookSamples)
                .WithOne()
                .HasForeignKey(bs => bs.BookEditionId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Reader)
                .WithOne(r => r.User);

            modelBuilder.Entity<User>()
                .HasOne(u => u.LibraryStaff)
                .WithOne(ls => ls.User);

            modelBuilder.Entity<Reader>()
                .HasMany(r => r.ReaderLoans)
                .WithOne(rl => rl.Reader);

            modelBuilder.Entity<ReaderLoan>()
                .HasMany(rl => rl.BookLoanDetails)
                .WithOne(bld => bld.ReaderLoan);

            modelBuilder.Entity<BookAuthor>()
                .HasKey(ba => new { ba.BookId, ba.AuthorId });

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.BookId);

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Author)
                .WithMany(a => a.BookAuthors)
                .HasForeignKey(ba => ba.AuthorId);


            modelBuilder.Entity<BookDomain>()
            .HasKey(ba => new { ba.BookId, ba.DomainId });

            modelBuilder.Entity<BookDomain>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.BookDomains)
                .HasForeignKey(ba => ba.BookId);

            modelBuilder.Entity<BookDomain>()
                .HasOne(ba => ba.Domain)
                .WithMany(a => a.BookDomains)
                .HasForeignKey(ba => ba.DomainId);

            modelBuilder.Entity<LibrarySettings>();
               
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    // Map the database column name based on the property name
                    property.SetColumnName(ConvertPropertyNameToColumnName(property.Name));
                }
            }
        }

        private static string ConvertPropertyNameToColumnName(string propertyName)
        {
            // Implement your naming convention logic here
            // For example, convert PascalCase to snake_case
            return string.Concat(propertyName.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }
    }
}
