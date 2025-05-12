using AutoMapper;
using Core.Interfaces;
using Infrastructure.Services;
using Moq;

namespace NUnitLms.ServiceUnitTests.BorrowalService
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
                                    Task.FromResult(BorrowalsTestDataFactory.CreateTestBorrowals())
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






        
    }
}
