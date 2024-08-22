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
    public class CompanyControllerTest
    {
        private Mock<IBusinessLogic<Company>> _companyLogic;
        private CompanyController _companyController;
        private Company company;

        [TestInitialize]
        public void Setup()
        {
            _companyLogic = new Mock<IBusinessLogic<Company>>(MockBehavior.Strict);
            _companyController = new CompanyController(_companyLogic.Object);
            company = new Company
            {
                Id = 1,
                Name = "CategoryTest"
            };
        }
        [TestMethod]
        public void GetAllCompanies()
        {
            List<Company> companies = new List<Company> { new Company { Id = 1, Name = "Test" } };
            _companyLogic.Setup(x => x.GetAll()).Returns(companies);

            var expectedContent = companies.Select(a => new CompanyDetailModel(a)).ToList();

            var result = _companyController.Index();
            var okResult = result as OkObjectResult;
            var actualContent = okResult.Value as List<CompanyDetailModel>;

            _companyLogic.VerifyAll();
            CollectionAssert.AreEqual(expectedContent, actualContent);
        }

        [TestMethod]
        public void GetOkTest()
        {
            List<Company> companies = new List<Company> { company };
            _companyLogic.Setup(o => o.GetById(It.IsAny<int>())).Returns(companies.First());
            var expectedCompanyModel = new CompanyDetailModel(companies.First());

            var result = _companyController.Show(companies.First().Id);
            var okResult = result as OkObjectResult;
            var actualCompanyModel = okResult.Value as CompanyDetailModel;

            _companyLogic.VerifyAll();
            Assert.AreEqual(expectedCompanyModel, actualCompanyModel);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void ShowNotFoundTest()
        {
            _companyLogic.Setup(o => o.GetById(It.IsAny<int>())).Throws(new NotFoundException("Company not found"));
            var result = _companyController.Show(1);

            _companyLogic.VerifyAll();
        }

        [TestMethod]
        public void CreateOkTest()
        {
            CompanyCreateModel companyCreateModel = new CompanyCreateModel
            {
                Name = "Test"
            };
            _companyLogic.Setup(o => o.Create(It.IsAny<Company>())).Returns(companyCreateModel.ToEntity());
            var expectedCompanyModel = new CompanyDetailModel(companyCreateModel.ToEntity());
            var result = _companyController.Create(companyCreateModel);
            var okResult = result as CreatedAtActionResult;
            var actualCompanyModel = okResult.Value as CompanyDetailModel;

            _companyLogic.VerifyAll();
            Assert.AreEqual(expectedCompanyModel, actualCompanyModel);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void CreateInvalidAttributeTest()
        {
            CompanyCreateModel companyCreateModel = new CompanyCreateModel
            {
                Name = ""
            };
            _companyLogic.Setup(o => o.Create(It.IsAny<Company>())).Returns(companyCreateModel.ToEntity());
            var expectedCompanyModel = new CompanyDetailModel(companyCreateModel.ToEntity());
            var result = _companyController.Create(companyCreateModel);

            _companyLogic.VerifyAll();
        }

        [TestMethod]
        public void UpdateOkTest()
        {
            CompanyCreateModel companyCreateModel = new CompanyCreateModel
            {
                Name = "Test"
            };
            _companyLogic.Setup(o => o.Update(It.IsAny<int>(), It.IsAny<Company>())).Returns(companyCreateModel.ToEntity());
            var expectedCompanyModel = new CompanyDetailModel(companyCreateModel.ToEntity());
            var result = _companyController.Update(1, companyCreateModel);
            var okResult = result as OkObjectResult;
            var actualCompanyModel = okResult.Value as CompanyDetailModel;

            _companyLogic.VerifyAll();
            Assert.AreEqual(expectedCompanyModel, actualCompanyModel);
        }

        [TestMethod]
        public void DeleteCompany()
        {
            _companyLogic.Setup(o => o.Delete(It.IsAny<int>())).Returns(true);
            var result = _companyController.Delete(1);
            var noContentResult = result as NoContentResult;

            _companyLogic.VerifyAll();
            Assert.AreEqual(204, noContentResult.StatusCode);
        }

        [TestMethod]
        public void DeleteNotFoundCompany()
        {
            _companyLogic.Setup(o => o.Delete(It.IsAny<int>())).Returns(false);
            var result = _companyController.Delete(company.Id);
            var notFound = result as NotFoundObjectResult;
            _companyLogic.VerifyAll();
            Assert.AreEqual(404, notFound.StatusCode);
        }
    }
}
