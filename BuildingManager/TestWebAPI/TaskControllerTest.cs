using Domain;
using IBusinessLogic;
using IBusinessLogic.Exceptions;
using Moq;
using DTOs.Out;
using DTOs.In;
using Microsoft.AspNetCore.Mvc;
using WebAPI;
using Task = Domain.Task;

namespace TestWebAPI
{
    [TestClass]
    public class TaskControllerTest
    {
        private Mock<IBusinessLogic<Task>> _taskLogic;
        private TaskController _taskController;
        private Task task;
        private Category category;
        private Building building;
        private Apartment apartment;

        [TestInitialize]
        public void Setup()
        {
            _taskLogic = new Mock<IBusinessLogic<Task>>(MockBehavior.Strict);
            _taskController = new TaskController(_taskLogic.Object);
            category = new Category
            {
                Id = 1
            };
            building = new Building
            {
                Id = 1
            };
            apartment = new Apartment
            {
                Id = 1,
                Building = building
            };
            task = new Task
            {
                Id = 1,
                Category = category,
                Apartment = apartment,
                Description = "Task"
            };
        }

        [TestMethod]
        public void GetAllTasks()
        {
            List<Task> tasks = new List<Task> { task };
            _taskLogic.Setup(x => x.GetAll()).Returns(tasks);

            var expectedContent = tasks.Select(a => new TaskDetailModel(a)).ToList();

            var result = _taskController.Index();
            var okResult = result as OkObjectResult;
            var actualContent = okResult.Value as List<TaskDetailModel>;

            _taskLogic.VerifyAll();
            CollectionAssert.AreEqual(expectedContent, actualContent);
        }

        [TestMethod]
        public void GetOkTest()
        {
            List<Task> tasks = new List<Task> { task };
            _taskLogic.Setup(o => o.GetById(It.IsAny<int>())).Returns(tasks.First());
            var expectedTaskModel = new TaskDetailModel(tasks.First());

            var result = _taskController.Show(tasks.First().Id);
            var okResult = result as OkObjectResult;
            var taskDetailModel = okResult.Value as TaskDetailModel;

            _taskLogic.VerifyAll();
            Assert.AreEqual(expectedTaskModel, taskDetailModel);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void ShowNotFoundTest()
        {
            _taskLogic.Setup(o => o.GetById(It.IsAny<int>())).Throws(new NotFoundException("Task not found"));
            var result = _taskController.Show(1);

            _taskLogic.VerifyAll();
        }

        [TestMethod]
        public void CreateOkTest()
        {
            TaskCreateModel taskCreateModel = new TaskCreateModel
            {
                CategoryId = 1,
                ApartmentId = 1,
                Description = "Task"
            };

            _taskLogic.Setup(x => x.Create(It.IsAny<Task>())).Returns(taskCreateModel.ToEntity());
            var expectedTaskModel = new TaskDetailModel(taskCreateModel.ToEntity());
            var result = _taskController.Create(taskCreateModel);
            var okResult = result as CreatedAtActionResult;
            var taskDetailModel = okResult.Value as TaskDetailModel;

            _taskLogic.VerifyAll();
            Assert.AreEqual(expectedTaskModel, taskDetailModel);
        }

        [TestMethod]
        public void UpdateStartTask()
        {
            TaskStartModel startTask = new TaskStartModel
            {
                StaffId = 1
            };
            _taskLogic.Setup(o => o.GetById(It.IsAny<int>())).Returns(task);
            _taskLogic.Setup(o => o.Update(It.IsAny<int>(), It.IsAny<Task>())).Returns(task);
            var expectedTaskModel = new TaskDetailModel(startTask.ToEntity());

            var result = _taskController.Start(1, startTask);
            var okResult = result as OkObjectResult;
            var taskDetailModel = okResult.Value as TaskDetailModel;
            _taskLogic.VerifyAll();
            Assert.AreEqual(expectedTaskModel.StaffId, taskDetailModel.StaffId);
        }

        [TestMethod]
        public void UpdateEndTask()
        {
            Staff staff = new Staff
            {
                Id = 1
            };
            task.StaffId = 1;
            task.Staff = staff;
            task.StartDate = DateTime.Today;
            TaskEndModel endTask = new TaskEndModel
            {
                Cost = 500
            };
            _taskLogic.Setup(o => o.GetById(It.IsAny<int>())).Returns(task);
            _taskLogic.Setup(o => o.Update(It.IsAny<int>(), endTask.ToEntity())).Returns(endTask.ToEntity());
            var expectedTaskModel = new TaskDetailModel(endTask.ToEntity());

            var result = _taskController.Finish(1, endTask);
            var okResult = result as OkObjectResult;
            var taskDetailModel = okResult.Value as TaskDetailModel;
            _taskLogic.VerifyAll();
            Assert.AreEqual(expectedTaskModel.Cost, taskDetailModel.Cost);
        }

        [TestMethod]
        public void DeleteTask()
        {
            _taskLogic.Setup(o => o.Delete(It.IsAny<int>())).Returns(true);
            var result = _taskController.Delete(task.Id);
            var noContent = result as NoContentResult;
            _taskLogic.VerifyAll();
            Assert.AreEqual(204, noContent.StatusCode);
        }

        [TestMethod]
        public void DeleteNotFoundTask()
        {
            _taskLogic.Setup(o => o.Delete(It.IsAny<int>())).Returns(false);
            var result = _taskController.Delete(task.Id);
            var notFound = result as NotFoundObjectResult;
            _taskLogic.VerifyAll();
            Assert.AreEqual(404, notFound.StatusCode);
        }
    }
}

