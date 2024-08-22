using Domain;
using IBusinessLogic;
using Moq;
using WebAPI;
using Task = Domain.Task;

namespace TestWebAPI;

[TestClass]
public class ReportControllerTest
{

    private Mock<IBusinessLogic<Task>> _taskLogicMock;
    private Mock<ISessionLogic> _sessionLogicMock;
    private Mock<IBusinessLogic<Staff>> _staffLogicMock;
    private ReportController _reportController;
    private Building building;

    [TestInitialize]
    public void Setup()
    {
        _taskLogicMock = new Mock<IBusinessLogic<Task>>();
        _sessionLogicMock = new Mock<ISessionLogic>();
        _staffLogicMock = new Mock<IBusinessLogic<Staff>>();
        _reportController = new ReportController(_taskLogicMock.Object, _sessionLogicMock.Object, _staffLogicMock.Object);
    }
}
