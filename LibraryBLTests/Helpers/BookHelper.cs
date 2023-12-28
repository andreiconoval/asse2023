using Library.DAL.DomainModel;
using System.Diagnostics.CodeAnalysis;

namespace LibraryBLTests.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class BookHelper
    {
        static List<Book> GenerateBooks(int count)
        {
            var books = new List<Book>();
            var random = new Random();
            var domains = DomainHelper.GenerateDomains();

            for (int i = 1; i <= count; i++)
            {
                var book = new Book
                {
                    Id = i,
                    Title = $"Book Title {i}",
                    YearPublication = 2000 + i,
                    BookEditions = GenerateEditions(1, 2, i),
                };
                // Assign up to 2 unique domains to the book
                var assignedDomains = new HashSet<Domain>();
                while (assignedDomains.Count < 2)
                {
                    var randomDomain = domains[random.Next(domains.Count)];

                    // Check if the domain is not a descendant or ancestor of any already assigned domain
                    if (!assignedDomains.Any(d => DomainHelper.IsDescendantOrAncestor(randomDomain, d)))
                    {
                        assignedDomains.Add(randomDomain);
                    }
                }

                // Add book domains based on the assigned domains
                foreach (var assignedDomain in assignedDomains)
                {
                    book.BookDomains.Add(new BookDomain
                    {
                        DomainId = assignedDomain.Id,
                        Domain = assignedDomain,
                        BookId = book.Id,
                        Book = book
                    });
                }

                books.Add(book);
            }

            return books;
        }

        static List<BookEdition> GenerateEditions(int min, int max, int bookId)
        {
            var editions = new List<BookEdition>();

            for (int i = min; i <= max; i++)
            {
                var edition = new BookEdition
                {
                    Id = i,
                    BookId = bookId,
                    PageNumber = 200 + i * 50,
                    BookType = $"Type {i}",
                    Edition = $"Edition {i}",
                    ReleaseYear = 2000 + i,
                    BookSamples = GenerateSamples(5, i),
                };

                editions.Add(edition);
            }

            return editions;
        }

        static List<BookSample> GenerateSamples(int count, int editionId)
        {
            var samples = new List<BookSample>();

            for (int i = 1; i <= count; i++)
            {
                var sample = new BookSample
                {
                    Id = i,
                    BookEditionId = editionId,
                    AvailableForLoan = i % 2 == 0,
                    AvailableForHall = i % 2 == 1,
                };

                samples.Add(sample);
            }

            return samples;
        }

        public static List<Book> Books { get; set; } = new List<Book>
        {
            new Book
            {
                Id = 1,
                Title = "The Catcher in the Rye",
                YearPublication = 1951,
                BookDomains = new List<BookDomain>
                {
                    new BookDomain { DomainId = 1 }, // Assign a domain ID based on your domain structure
                    new BookDomain { DomainId = 2 } // Assign another domain ID
                },
                BookEditions = new List<BookEdition>
                {
                    new BookEdition
                    {
                        Id = 1,
                        PageNumber = 224,
                        BookType = "Paperback",
                        Edition = "First Edition",
                        ReleaseYear = 1951,
                        BookSamples = new List<BookSample>
                        {
                            new BookSample { Id = 1, AvailableForLoan = true, AvailableForHall = true },
                            new BookSample { Id = 2, AvailableForLoan = true, AvailableForHall = true },
                            // Add more samples if needed
                        }
                    },
                    // Add another edition if needed
                }
            },
            new Book
            {
                Id = 2,
                Title = "To Kill a Mockingbird",
                YearPublication = 1960,
                BookDomains = new List<BookDomain>
                {
                    new BookDomain { DomainId = 3 }, // Assign a domain ID based on your domain structure
                    new BookDomain { DomainId = 4 } // Assign another domain ID
                },
                BookEditions = new List<BookEdition>
                {
                    new BookEdition
                    {
                        Id = 2,
                        PageNumber = 281,
                        BookType = "Hardcover",
                        Edition = "First Edition",
                        ReleaseYear = 1960,
                        BookSamples = new List<BookSample>
                        {
                            new BookSample { Id = 3, AvailableForLoan = true, AvailableForHall = true },
                            new BookSample { Id = 4, AvailableForLoan = true, AvailableForHall = true },
                            // Add more samples if needed
                        }
                    },
                    // Add another edition if needed
                }
            },
            new Book
            {
                Id = 3,
                Title = "1984",
                YearPublication = 1949,
                BookDomains = new List<BookDomain>
                {
                    new BookDomain { DomainId = 5 }, // Assign a domain ID based on your domain structure
                    new BookDomain { DomainId = 6 } // Assign another domain ID
                },
                BookEditions = new List<BookEdition>
                {
                    new BookEdition
                    {
                        Id = 3,
                        PageNumber = 328,
                        BookType = "Paperback",
                        Edition = "First Edition",
                        ReleaseYear = 1949,
                        BookSamples = new List<BookSample>
                        {
                            new BookSample { Id = 5, AvailableForLoan = true, AvailableForHall = true },
                            new BookSample { Id = 6, AvailableForLoan = true, AvailableForHall = true },
                            // Add more samples if needed
                        }
                    },
                    // Add another edition if needed
                }
            },
            new Book
            {
                Id = 4,
                Title = "The Great Gatsby",
                YearPublication = 1925,
                BookDomains = new List<BookDomain>
                {
                    new BookDomain { DomainId = 7 }, // Assign a domain ID based on your domain structure
                    new BookDomain { DomainId = 8 } // Assign another domain ID
                },
                BookEditions = new List<BookEdition>
                {
                    new BookEdition
                    {
                        Id = 4,
                        PageNumber = 180,
                        BookType = "Hardcover",
                        Edition = "First Edition",
                        ReleaseYear = 1925,
                        BookSamples = new List<BookSample>
                        {
                            new BookSample { Id = 7, AvailableForLoan = true, AvailableForHall = true },
                            new BookSample { Id = 8, AvailableForLoan = true, AvailableForHall = true },
                            // Add more samples if needed
                        }
                    },
                    // Add another edition if needed
                }
            },
            new Book
            {
                Id = 5,
                Title = "Moby-Dick",
                YearPublication = 1851,
                BookDomains = new List<BookDomain>
                {
                    new BookDomain { DomainId = 9 }, // Assign a domain ID based on your domain structure
                    new BookDomain { DomainId = 10 } // Assign another domain ID
                },
                BookEditions = new List<BookEdition>
                {
                    new BookEdition
                    {
                        Id = 5,
                        PageNumber = 624,
                        BookType = "Paperback",
                        Edition = "First Edition",
                        ReleaseYear = 1851,
                        BookSamples = new List<BookSample>
                        {
                            new BookSample { Id = 9, AvailableForLoan = true, AvailableForHall = true },
                            new BookSample { Id = 10, AvailableForLoan = true, AvailableForHall = true },
                            // Add more samples if needed
                        }
                    },
                    // Add another edition if needed
                }
            }
        };

                // Now, 'books' list contains instances of five books with assigned domains, editions, and samples.

    }
}
