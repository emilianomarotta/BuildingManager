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
    public class StaffControllerTest
    {
        private Mock<IBusinessLogic<Staff>> _staffLogic;
        private StaffController _staffController;
        private Staff staff;

        [TestInitialize]
        public void Setup()
        {
            _staffLogic = new Mock<IBusinessLogic<Staff>>(MockBehavior.Strict);
            _staffController = new StaffController(_staffLogic.Object);
            staff = new Staff
            {
                Id = 1,
                Name = "Staff",
                LastName = "Staff",
                Email = "staff@staff.com",
            };
        }

        [TestMethod]
        public void GetAllStaffs()
        {
            List<Staff> staffs = new List<Staff> { staff };
            _staffLogic.Setup(x => x.GetAll()).Returns(staffs);

            var expectedContent = staffs.Select(a => new StaffDetailModel(a)).ToList();

            var result = _staffController.Index();
            var okResult = result as OkObjectResult;
            var actualContent = okResult.Value as List<StaffDetailModel>;

            _staffLogic.VerifyAll();
            CollectionAssert.AreEqual(expectedContent, actualContent);
        }

        [TestMethod]
        public void GetOkTest()
        {
            List<Staff> staffs = new List<Staff> { staff };
            _staffLogic.Setup(o => o.GetById(It.IsAny<int>())).Returns(staffs.First());
            var expectedStaffModel = new StaffDetailModel(staffs.First());

            var result = _staffController.Show(staffs.First().Id);
            var okResult = result as OkObjectResult;
            var staffDetailModel = okResult.Value as StaffDetailModel;

            _staffLogic.VerifyAll();
            Assert.AreEqual(expectedStaffModel, staffDetailModel);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void ShowNotFoundTest()
        {
            _staffLogic.Setup(o => o.GetById(It.IsAny<int>())).Throws(new NotFoundException("Staff not found"));
            var result = _staffController.Show(1);

            _staffLogic.VerifyAll();
        }

        [TestMethod]
        public void CreateOkTest()
        {
            StaffCreateModel staff;
            staff = new StaffCreateModel
            {
                Name = "Staff",
                LastName = "Staff",
                Email = "test@test.com",
                Password = "Test.1234",
            };
            _staffLogic.Setup(o => o.Create(It.IsAny<Staff>())).Returns(staff.ToEntity());
            var expectedStaffModel = new StaffDetailModel(staff.ToEntity());
            var result = _staffController.Create(staff);
            var okResult = result as CreatedAtActionResult;
            var staffDetailModel = okResult.Value as StaffDetailModel;

            _staffLogic.VerifyAll();
            Assert.AreEqual(expectedStaffModel, staffDetailModel);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void CreateInvalidAttributeTest()
        {
            StaffCreateModel staff;
            staff = new StaffCreateModel
            {
                Name = "",
                LastName = "Staff",
                Email = "test@test.com",
                Password = "Test.1234",
            };
            _staffLogic.Setup(o => o.Create(It.IsAny<Staff>())).Returns(staff.ToEntity());
            var expectedStaffModel = new StaffDetailModel(staff.ToEntity());
            var result = _staffController.Create(staff);
        }

        [TestMethod]
        public void UpdateStaff()
        {
            StaffCreateModel staff;
            staff = new StaffCreateModel
            {
                Name = "Pedro",
                LastName = "Pereira",
                Email = "anotherEmail@test.com",
                Password = "Test.56789"
            };
            _staffLogic.Setup(o => o.Update(It.IsAny<int>(), staff.ToEntity())).Returns(staff.ToEntity());
            var expectedStaffModel = new StaffDetailModel(staff.ToEntity());

            var result = _staffController.Update(staff.ToEntity().Id, staff);
            var okResult = result as OkObjectResult;
            var staffDetailModel = okResult.Value as StaffDetailModel;
            _staffLogic.VerifyAll();
            Assert.AreEqual(expectedStaffModel, staffDetailModel);
        }

        [TestMethod]
        public void DeleteStaff()
        {
            _staffLogic.Setup(o => o.Delete(It.IsAny<int>())).Returns(true);
            var result = _staffController.Delete(staff.Id);
            var noContent = result as NoContentResult;
            _staffLogic.VerifyAll();
            Assert.AreEqual(204, noContent.StatusCode);
        }

        [TestMethod]
        public void DeleteNotFoundStaff()
        {
            _staffLogic.Setup(o => o.Delete(It.IsAny<int>())).Returns(false);
            var result = _staffController.Delete(staff.Id);
            var notFound = result as NotFoundObjectResult;
            _staffLogic.VerifyAll();
            Assert.AreEqual(404, notFound.StatusCode);
        }
    }
}