using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Services;
using Moq;

namespace NUnitLms.ServiceUnitTests
{
    // method_expectedBehaviour_scenatio
    // assert.method(expected, actual)
    [TestFixture]
    public class BorrowalsServiceUnitTests
    {
        private Mock<IUnitOfWork> uowMock;
        private Mock<IMapper> mapperMock;
        private BorrowalsService cut = null!;

        [SetUp]
        public void Setup()
        {
            
            uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.BorrowalsRepository.GetAllBorrowalsWithNavPropsAsync())
                                .Returns(
                                    Task.FromResult(CreateTestBorrowals())
                                    );

            mapperMock = new Mock<IMapper>();
        }

        // Could not get this to work with auto mapper mapping which happens in the BorrowalsService class
        //[Test]
        //public async Task GetAllBorrowalsAsync_ReturnAll_WhenAvailable()
        //{
        //    // arrange 
        //    cut = new BorrowalsService(uowMock.Object, mapperMock.Object);   
        //    // act
        //    var actual = await cut.GetAllBorrowalsAsync();
        //    // assert
        //    Assert.That(actual.Count, Is.EqualTo(CreateTestBorrowals().Count));
        //    // mapping tests
        //}



        private static IReadOnlyList<Borrowals> CreateTestBorrowals()
        {
            return new List<Borrowals>
            {
                CreateTestBorrowalDue(),
                //CreateTestBorrowalUnDue()
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
