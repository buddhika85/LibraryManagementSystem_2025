using Core.DTOs;
using Core.Entities;

namespace NUnitLms.ServiceUnitTests.BorrowalService
{
    public static class BorrowalsTestDataFactory
    {
        public static IReadOnlyList<Borrowals> CreateTestBorrowals()
        {
            return new List<Borrowals>
            {
                CreateTestBorrowalDue(),
                //CreateTestBorrowalUnDue()
            };
        }

        public static AppUser GetTestMemberUser(string email)
        {
            return new AppUser
            {
                Email = email,
                FirstName = "Test User first name",
                LastName = "Test User lasy name"
            };
        }

        internal static Book GetTestBook(int id, bool isAvailable = true, string title = "Test Book")
        {
            return new Book
            {
                Id = id,
                Authors = new List<Author>
                    {
                        new Author
                        {
                            Id = 1,
                            Biography = string.Empty,
                            Books = new List<Book>(),
                            Country = string.Empty,
                            Name = "Test Author 1",
                            DateOfBirth = DateTime.Today.AddYears(-40)
                        }
                    },
                Genre = Core.Enums.BookGenre.None,
                IsAvailable = isAvailable,
                PictureUrl = string.Empty,
                PublishedDate = DateTime.Today.AddYears(-20),
                Title = title
            };
        }

        public static BookBorrowRequestDto GetTestValidBorrowRequest()
        {
            return new BookBorrowRequestDto
            {
                BookId = 1,
                Email = "testUser@lms.com",
                EndDate = DateTime.Today.AddDays(7),
                StartDate = DateTime.Today
            };
        }

        /// <summary>
        /// Returns a test borrowal which is due yesterday
        /// </summary>
        /// <returns>Borrowals entity</returns>
        private static Borrowals CreateTestBorrowalDue()
        {
            return new Borrowals
            {
                AppUser = new AppUser
                {
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    IsActive = true,
                    Address = new Address
                    { City = string.Empty, Country = string.Empty, Line1 = string.Empty, Postcode = string.Empty, State = string.Empty }
                },
                AppUserId = string.Empty,
                Book = new Book
                {
                    Id = 1,
                    Authors = new List<Author>
                    {
                        new Author
                        {
                            Id = 1,
                            Biography = string.Empty,
                            Books = new List<Book>(),
                            Country = string.Empty,
                            Name = string.Empty,
                            DateOfBirth = DateTime.Today.AddYears(-40)
                        }
                    },
                    Genre = Core.Enums.BookGenre.None,
                    IsAvailable = false,
                    PictureUrl = string.Empty,
                    PublishedDate = DateTime.Today.AddYears(-20),
                    Title = "Test Book - Due"
                },
                BookId = 1,
                BorrowalDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-8)),
                DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
                BorrowalStatus = Core.Enums.BorrowalStatus.Out,
                BorrowalReturn = null,
                Id = 1
            };
        }

        /// <summary>
        /// Returns a test borrowal which is, undue now, due after a week
        /// </summary>
        /// <returns>Borrowals entity</returns>
        private static Borrowals CreateTestBorrowalUnDue()
        {
            return new Borrowals
            {
                AppUser = new AppUser(),
                AppUserId = string.Empty,
                Book = new Book
                {
                    Id = 2,
                    Authors = new List<Author>
                    {
                        new Author
                        {
                            Id = 2,
                            Biography = string.Empty,
                            Books = new List<Book>(),
                            Country = string.Empty,
                            Name = string.Empty,
                            DateOfBirth = DateTime.Today.AddYears(-40)
                        }
                    },
                    Genre = Core.Enums.BookGenre.None,
                    IsAvailable = false,
                    PictureUrl = string.Empty,
                    PublishedDate = DateTime.Today.AddYears(-20),
                    Title = "Test Book - Undue"
                },
                BookId = 2,
                BorrowalDate = DateOnly.FromDateTime(DateTime.Today),
                DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(7)),
                BorrowalStatus = Core.Enums.BorrowalStatus.Out,
                BorrowalReturn = null,
                Id = 2
            };
        }
    }
}
