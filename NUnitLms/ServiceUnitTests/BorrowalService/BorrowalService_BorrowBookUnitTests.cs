using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Moq;


namespace NUnitLms.ServiceUnitTests.BorrowalService
{
    [TestFixture]
    public class BorrowalService_BorrowBookUnitTests
    {
        private Mock<IUnitOfWork> uowMock;
        private Mock<IMapper> mapperMock;
        private BorrowalsService cut = null!;

        [SetUp]
        public void Setup()
        {
            uowMock = new Mock<IUnitOfWork>();
            mapperMock = new Mock<IMapper>();
        }

        // method_expectedBehaviour_scenatio
        [Test]
        public async Task BorrowBook_ShallReturnErrorMessage_IfStartDateLessToday()
        {
            // arrange
            cut = new BorrowalsService(uowMock.Object, mapperMock.Object);
            var request = new BookBorrowRequestDto
            {
                BookId = 1,
                Email = "",
                EndDate = DateTime.Today,
                StartDate = DateTime.Today.AddDays(-1)
            };

            // act
            var result = await cut.BorrowBook(request);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.Not.True);
            Assert.That(result.ErrorMessage, Is.Not.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("Borrowal start date cannot be in the past."));
        }

        [Test]
        public async Task BorrowBook_ShallReturnErrorMessage_IfReturnDateGreaterThanStart()
        {
            // arrange
            cut = new BorrowalsService(uowMock.Object, mapperMock.Object);
            var request = new BookBorrowRequestDto
            {
                BookId = 1,
                Email = "",
                EndDate = DateTime.Today,
                StartDate = DateTime.Today
            };

            // act
            var result = await cut.BorrowBook(request);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsSuccess, Is.Not.True);
                Assert.That(result.ErrorMessage, Is.Not.Null);
                Assert.That(result.ErrorMessage, Is.EqualTo("Return date must be after the borrowal start date."));
            });
        }

        [Test]
        public async Task BorrowBook_ShallReturnErrorMessage_IfBookRecordUnavailableInDb()
        {
            // arrange            
            var request = BorrowalsTestDataFactory.GetTestValidBorrowRequest();
            uowMock.Setup(uow => uow.UserRepository.GetUserByRoleAndEmailAsync(request.Email, UserRoles.Member))
                    .Returns(Task.FromResult<AppUser?>(BorrowalsTestDataFactory.GetTestMemberUser(request.Email)));
            uowMock.Setup(uow => uow.BookRepository.GetBookByIdAsync(request.BookId))
                    .Returns(Task.FromResult<Book?>(null));
            cut = new BorrowalsService(uowMock.Object, mapperMock.Object);

            // act
            var result = await cut.BorrowBook(request);

            // assert            
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsSuccess, Is.Not.True);
                Assert.That(result.ErrorMessage, Is.Not.Null);
                Assert.That(result.ErrorMessage, Is.EqualTo($"A book with {request.BookId} does not exist"));
            });            
        }

        [Test]
        public async Task BorrowBook_ShallReturnErrorMessage_IfUserRecordUnavailableInDb()
        {
            // arrange            
            var request = BorrowalsTestDataFactory.GetTestValidBorrowRequest();

            uowMock.Setup(uow => uow.UserRepository.GetUserByRoleAndEmailAsync(request.Email, UserRoles.Member))
                    .Returns(Task.FromResult<AppUser?>(null));
            uowMock.Setup(uow => uow.BookRepository.GetBookByIdAsync(request.BookId))
                    .Returns(Task.FromResult<Book?>(BorrowalsTestDataFactory.GetTestBook(request.BookId)));
            cut = new BorrowalsService(uowMock.Object, mapperMock.Object);

            // act
            var result = await cut.BorrowBook(request);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsSuccess, Is.Not.True);
                Assert.That(result.ErrorMessage, Is.Not.Null);
                Assert.That(result.ErrorMessage, Is.EqualTo($"A member with {request.Email} does not exist"));
            });
        }
    }
}
