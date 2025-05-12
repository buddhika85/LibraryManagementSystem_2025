using AutoMapper;
using Core.DTOs;
using Core.Interfaces;
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
            //uowMock.Setup(uow => uow.BorrowalsRepository.GetAllBorrowalsWithNavPropsAsync())
            //                    .Returns(
            //                        Task.FromResult(BorrowalsTestDataFactory.CreateTestBorrowals())
            //                        );

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
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.Not.True);
            Assert.That(result.ErrorMessage, Is.Not.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("Return date must be after the borrowal start date."));
        }
    }
}
