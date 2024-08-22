using Domain;
using BusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;
using Moq;
using IBusinessLogic;

namespace TestBusinessLogic;

[TestClass]
public class CompanyLogicTest
{
    private Mock<IGenericRepository<Company>> _companyRepository;
    private Mock<ISessionLogic> _sessionLogic;
    private CompanyLogic _companyLogic;
    private CompanyAdmin companyAdmin;
    private Company company;


    [TestInitialize]
    public void Setup()
    {
        _companyRepository = new Mock<IGenericRepository<Company>>(MockBehavior.Strict);
        _sessionLogic = new Mock<ISessionLogic>(MockBehavior.Strict);
        _companyLogic = new CompanyLogic(_companyRepository.Object, _sessionLogic.Object);
        companyAdmin = new CompanyAdmin
        {
            Id = 1,
            Name = "CompanyAdmin",
            LastName = "CompanyAdmin",
            Email = "test@test.com",
            Password = "Test.1234"
        };
        company = new Company()
        {
            Id = 1,
            Name = "Company",
            companyAdmin = companyAdmin,
            CompanyAdminId = companyAdmin.Id
        };

    }

    [TestMethod]
    public void CreateCompanyTest()
    {
        string companyName = "Company";
        List<Company> companies = new List<Company> { };
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)companyAdmin);
        _companyRepository.Setup(c => c.GetAll<Company>()).Returns(companies);
        _companyRepository.Setup(c => c.Insert(company));
        var insertedCompany = _companyLogic.Create(company);

        _companyRepository.VerifyAll();
        Assert.AreEqual(company, insertedCompany);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateCompanyAdminOtherCompanyTest()
    {
        string companyName = "Company";
        Company otherCompany = new Company()
        {
            Id = 2,
            Name = "OtherCompany",
            companyAdmin = companyAdmin,
            CompanyAdminId = companyAdmin.Id
        };
        List<Company> companies = new List<Company> { company };
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)companyAdmin);
        _companyRepository.Setup(c => c.GetAll<Company>()).Returns(companies);
        var insertedCompany = _companyLogic.Create(company);

        _companyRepository.VerifyAll();
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateAlreadyExistingCompany()
    {
        string companyName = "Company";
        List<Company> companies = new List<Company> { company };
        _companyRepository.Setup(c => c.GetAll<Company>()).Returns(companies);
        var otherCompany = new Company()
        {
            Id = 2,
            Name = "Company",
        };
        var insertedCompany = _companyLogic.Create(otherCompany);

        _companyRepository.VerifyAll();
    }

    [TestMethod]
    public void GetAllCompanies()
    {
        List<Company> companies = new List<Company> { company };
        _companyRepository.Setup(c => c.GetAll<Company>()).Returns(companies);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)companyAdmin);
        List<Company> allCompanies = _companyLogic.GetAll();

        _companyRepository.VerifyAll();
        CollectionAssert.AreEqual(companies, allCompanies);
    }

    [TestMethod]
    public void GetCompanyById()
    {
        int companyId = 1;
        int companyAdminId = companyAdmin.Id;
        _companyRepository.Setup(c => c.Get(company => company.Id == companyId && company.CompanyAdminId == companyAdminId, null)).Returns(company);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)companyAdmin);
        var companyById = _companyLogic.GetById(companyId);

        _companyRepository.VerifyAll();
        Assert.AreEqual(company, companyById);
    }

    [TestMethod]
    public void GetCompanyByIdNotFound()
    {
        int companyId = 1;
        int companyAdminId = companyAdmin.Id;
        _companyRepository.Setup(c => c.Get(company => company.Id == companyId && company.CompanyAdminId == companyAdminId, null)).Returns((Company)null);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)companyAdmin);
        var companyById = _companyLogic.GetById(companyId);

        _companyRepository.VerifyAll();
        Assert.IsNull(companyById);
    }

    [TestMethod]
    public void UpdateCompany()
    {
        int companyId = 1;
        var updatedCompany = new Company
        {
            Id = 1,
            Name = "Updated Company",
        };
        List<Company> companies = new List<Company> { };
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)companyAdmin);
        _companyRepository.Setup(c => c.GetAll<Company>()).Returns(companies);
        _companyRepository.Setup(c => c.Get(company => company.Id == companyId, null)).Returns(company);
        _companyRepository.Setup(c => c.Update(company));
        var companyById = _companyLogic.Update(companyId, updatedCompany);

        _companyRepository.VerifyAll();
        Assert.AreEqual(updatedCompany, companyById);
    }
    [TestMethod]
    [ExpectedException(typeof(UnauthorizedException))]
    public void UpdateUnauthorizedCompany()
    {
        int companyId = 1;
        CompanyAdmin othercompanyAdmin = new CompanyAdmin
        {
            Id = 5,
            Name = "OtherCompanyAdmin",
            LastName = "CompanyAdmin",
            Email = "other@admin.com",
            Password = "Test.1234"
        };
        var updatedCompany = new Company
        {
            Id = 1,
            Name = "Updated Company",
        };
        List<Company> companies = new List<Company> { };
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)othercompanyAdmin);
        _companyRepository.Setup(c => c.GetAll<Company>()).Returns(companies);
        _companyRepository.Setup(c => c.Get(company => company.Id == companyId, null)).Returns(company);
        var companyById = _companyLogic.Update(companyId, updatedCompany);

        _companyRepository.VerifyAll();
        Assert.AreEqual(updatedCompany, companyById);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void UpdateAlreadyExistingCompany()
    {
        int companyId = 1;
        var updatedCompany = new Company()
        {
            Id = 1,
            Name = "Company",
        };
        List<Company> companies = new List<Company> { company };
        _companyRepository.Setup(c => c.GetAll<Company>()).Returns(companies);
        _companyRepository.Setup(c => c.Update(company));
        var companyById = _companyLogic.Update(companyId, updatedCompany);

        _companyRepository.VerifyAll();
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void UpdateNotFoundCompany()
    {
        int companyId = 1;
        var updatedCompany = new Company()
        {
            Id = 2,
            Name = "Updated Company",
        };
        List<Company> companies = new List<Company> { company };
        _companyRepository.Setup(c => c.GetAll<Company>()).Returns(companies);
        _companyRepository.Setup(c => c.Get(company => company.Id == companyId, null)).Returns((Company)null);
        _companyRepository.Setup(c => c.Update(updatedCompany));
        var companyById = _companyLogic.Update(companyId, updatedCompany);

        _companyRepository.VerifyAll();
    }

    [TestMethod]
    public void DeleteCompany()
    {
        int companyId = 1;
        List<Company> companies = new List<Company> { company };
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)companyAdmin);
        _companyRepository.Setup(c => c.Get(company => company.Id == companyId, null)).Returns(company);
        _companyRepository.Setup(c => c.Delete(company));
        var companyById = _companyLogic.Delete(companyId);

        _companyRepository.VerifyAll();
        Assert.IsTrue(companyById);
    }

    [TestMethod]
    [ExpectedException(typeof(UnauthorizedException))]
    public void DeleteUnauthorizedCompany()
    {
        int companyId = 1;
        CompanyAdmin othercompanyAdmin = new CompanyAdmin
        {
            Id = 5,
            Name = "OtherCompanyAdmin",
            LastName = "CompanyAdmin",
            Email = "other@admin.com",
            Password = "Test.1234"
        };
        List<Company> companies = new List<Company> { company };
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)othercompanyAdmin);
        _companyRepository.Setup(c => c.Get(company => company.Id == companyId, null)).Returns(company);
        var companyById = _companyLogic.Delete(companyId);

        _companyRepository.VerifyAll();
        Assert.IsTrue(companyById);
    }

    [TestMethod]
    public void DeleteNotFoundCompany()
    {
        int companyId = 1;
        List<Company> companies = new List<Company> { };
        _companyRepository.Setup(c => c.Get(company => company.Id == companyId, null)).Returns((Company)null);
        var companyById = _companyLogic.Delete(companyId);

        _companyRepository.VerifyAll();
        Assert.IsFalse(companyById);
    }
}