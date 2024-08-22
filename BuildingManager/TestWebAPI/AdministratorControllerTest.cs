using Domain;
using IBusinessLogic;
using IBusinessLogic.Exceptions;
using Moq;
using DTOs.Out;
using DTOs.In;
using Microsoft.AspNetCore.Mvc;
using WebAPI;
using Domain.Exceptions;

namespace TestWebAPI
{
    [TestClass]
    public class AdministratorControllerTest
    {
        private Mock<IBusinessLogic<Administrator>> _administratorLogic;
        private AdministratorController _administratorController;
        private Administrator administrator;


        [TestInitialize]
        public void Setup()
        {
            _administratorLogic = new Mock<IBusinessLogic<Administrator>>(MockBehavior.Strict);
            _administratorController = new AdministratorController(_administratorLogic.Object);
            administrator = new Administrator
            {
                Id = 1,
                Name = "Administrator",
                LastName = "Administrator",
                Email = "administrator@administrator.com"
            };
        }

        [TestMethod]
        public void GetAllAdministrators()
        {
            List<Administrator> administrators = new List<Administrator> { administrator };
            _administratorLogic.Setup(x => x.GetAll()).Returns(administrators);

            var expectedContent = administrators.Select(a => new AdministratorDetailModel(a)).ToList();

            var result = _administratorController.Index();
            var okResult = result as OkObjectResult;
            var actualContent = okResult.Value as List<AdministratorDetailModel>;

            _administratorLogic.VerifyAll();
            CollectionAssert.AreEqual(expectedContent, actualContent);
        }

        [TestMethod]
        public void GetOkTest()
        {
            List<Administrator> administrators = new List<Administrator> { administrator };
            _administratorLogic.Setup(o => o.GetById(It.IsAny<int>())).Returns(administrators.First());
            var expectedAdministratorModel = new AdministratorDetailModel(administrators.First());

            var result = _administratorController.Show(administrators.First().Id);
            var okResult = result as OkObjectResult;
            var administratorDetailModel = okResult.Value as AdministratorDetailModel;

            _administratorLogic.VerifyAll();
            Assert.AreEqual(expectedAdministratorModel, administratorDetailModel);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void ShowNotFoundTest()
        {
            _administratorLogic.Setup(o => o.GetById(It.IsAny<int>())).Throws(new NotFoundException("Administrator not found"));
            var result = _administratorController.Show(1);

            _administratorLogic.VerifyAll();
        }

        [TestMethod]
        public void CreateOkTest()
        {
            AdministratorCreateModel administrator;
            administrator = new AdministratorCreateModel
            {
                Name = "Administrator",
                LastName = "Administrator",
                Email = "test@test.com",
                Password = "Test.1234"
            };
            _administratorLogic.Setup(o => o.Create(It.IsAny<Administrator>())).Returns(administrator.ToEntity());
            var expectedAdministratorModel = new AdministratorDetailModel(administrator.ToEntity());
            var result = _administratorController.Create(administrator);
            var okResult = result as CreatedAtActionResult;
            var administratorDetailModel = okResult.Value as AdministratorDetailModel;

            _administratorLogic.VerifyAll();
            Assert.AreEqual(expectedAdministratorModel, administratorDetailModel);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void CreateInvalidAttributeTest()
        {
            AdministratorCreateModel administrator;
            administrator = new AdministratorCreateModel
            {
                Name = "",
                LastName = "Administrator",
                Email = "test@test.com",
                Password = "Test.1234"
            };
            _administratorLogic.Setup(o => o.Create(It.IsAny<Administrator>())).Returns(administrator.ToEntity());
            var expectedAdministratorModel = new AdministratorDetailModel(administrator.ToEntity());
            var result = _administratorController.Create(administrator);
        }

        [TestMethod]
        public void UpdateAdministrator()
        {
            AdministratorCreateModel administrator;
            administrator = new AdministratorCreateModel
            {
                Name = "Pedro",
                LastName = "Pereira",
                Email = "anotherEmail@test.com",
                Password = "Test.56789"
            };
            _administratorLogic.Setup(o => o.Update(It.IsAny<int>(), administrator.ToEntity())).Returns(administrator.ToEntity());
            var expectedAdministratorModel = new AdministratorDetailModel(administrator.ToEntity());

            var result = _administratorController.Update(administrator.ToEntity().Id, administrator);
            var okResult = result as OkObjectResult;
            var administratorDetailModel = okResult.Value as AdministratorDetailModel;
            _administratorLogic.VerifyAll();
            Assert.AreEqual(expectedAdministratorModel, administratorDetailModel);
        }

        [TestMethod]
        public void DeleteAdministrator()
        {
            _administratorLogic.Setup(o => o.Delete(It.IsAny<int>())).Returns(true);
            var result = _administratorController.Delete(administrator.Id);
            var noContent = result as NoContentResult;
            _administratorLogic.VerifyAll();
            Assert.AreEqual(204, noContent.StatusCode);
        }

        [TestMethod]
        public void DeleteNotFoundAdministrator()
        {
            _administratorLogic.Setup(o => o.Delete(It.IsAny<int>())).Returns(false);
            var result = _administratorController.Delete(administrator.Id);
            var notFound = result as NotFoundObjectResult;
            _administratorLogic.VerifyAll();
            Assert.AreEqual(404, notFound.StatusCode);
        }
    }
}