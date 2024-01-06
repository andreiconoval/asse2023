//------------------------------------------------------------------------------
// <copyright file="LibraryDbContext.cs" company="Transilvania University of Brasov">
// Copyright (c) Conoval. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Library.DAL.DataMapper
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Library.DAL.DomainModel;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Defines the <see cref="LibraryDbContext" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LibraryDbContext : DbContext
    {
        /// <summary>
        /// Gets or sets the Books.
        /// </summary>
        public DbSet<Book>? Books { get; set; }

        /// <summary>
        /// Gets or sets the Domains.
        /// </summary>
        public DbSet<Domain>? Domains { get; set; }

        /// <summary>
        /// Gets or sets the BooksDomains.
        /// </summary>
        public DbSet<BookDomain>? BooksDomains { get; set; }

        /// <summary>
        /// Gets or sets the Authors.
        /// </summary>
        public DbSet<Author>? Authors { get; set; }

        /// <summary>
        /// Gets or sets the BooksAuthors.
        /// </summary>
        public DbSet<BookAuthor>? BooksAuthors { get; set; }

        /// <summary>
        /// Gets or sets the BookEditions.
        /// </summary>
        public DbSet<BookEdition>? BookEditions { get; set; }

        /// <summary>
        /// Gets or sets the BookSamples.
        /// </summary>
        public DbSet<BookSample>? BookSamples { get; set; }

        /// <summary>
        /// Gets or sets the Users.
        /// </summary>
        public DbSet<User>? Users { get; set; }

        /// <summary>
        /// Gets or sets the Readers.
        /// </summary>
        public DbSet<Reader>? Readers { get; set; }

        /// <summary>
        /// Gets or sets the LibraryStaff.
        /// </summary>
        public DbSet<LibraryStaff>? LibraryStaff { get; set; }

        /// <summary>
        /// Gets or sets the ReaderLoans.
        /// </summary>
        public DbSet<ReaderLoan>? ReaderLoans { get; set; }

        /// <summary>
        /// Gets or sets the BookLoanDetails.
        /// </summary>
        public DbSet<BookLoanDetail>? BookLoanDetails { get; set; }

        /// <summary>
        /// Gets or sets the LibrarySettings.
        /// </summary>
        public DbSet<LibrarySettings>? LibrarySettings { get; set; }

        /// <summary>
        /// The OnConfiguring.
        /// </summary>
        /// <param name="optionsBuilder">The optionsBuilder<see cref="DbContextOptionsBuilder"/>.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=asse_library;user=root;password=mysqladmin");
        }

        /// <summary>
        /// The OnModelCreating.
        /// </summary>
        /// <param name="modelBuilder">The modelBuilder<see cref="ModelBuilder"/>.</param>
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

        /// <summary>
        /// The ConvertPropertyNameToColumnName.
        /// </summary>
        /// <param name="propertyName">The propertyName<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private static string ConvertPropertyNameToColumnName(string propertyName)
        {
            // Implement your naming convention logic here
            // For example, convert PascalCase to snake_case
            return string.Concat(propertyName.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }
    }
}
