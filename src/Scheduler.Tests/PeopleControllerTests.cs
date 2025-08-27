using Moq;
using Scheduler.Controllers;
using Scheduler.Database;
using Scheduler.Models;

//Source https://learn.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking
namespace Scheduler.Tests;

[TestClass]
public class PeopleControllerTests
{
    private readonly Mock<IRepository<Person>> mockRepository = new();
    private readonly List<Person> data = new List<Person>
    {
        new(Guid.NewGuid(), string.Empty, String.Empty),
        new(Guid.NewGuid(), string.Empty, String.Empty),
        new(Guid.NewGuid(), string.Empty, String.Empty),
    };

    [TestMethod]
    public async Task PeopleReturnedOnGet()
    {
        var mockRepository = new Mock<IRepository<Person>>();
        mockRepository.Setup(x => x.Query()).ReturnsAsync(data);
        var mapper = TestUtils.GetMapper();
        var service = new PeopleController(mockRepository.Object, mapper);

        var people = await service.GetPeople();

        mockRepository.Verify(x => x.Query(), Times.Once());
    }

    [TestMethod]
    public async Task PersonCreatedOnPost()
    {
        var mapper = TestUtils.GetMapper();
        var service = new PeopleController(mockRepository.Object, mapper);

        await service.PostPerson(new PersonCreateModel("Test", "Dummy"));

        mockRepository.Verify(m => m.CreateAsync(It.IsAny<Person>()), Times.Once());
    }

    [TestMethod]
    public async Task PersonDeletedOnDelete()
    {
        var mapper = TestUtils.GetMapper();
        mockRepository.Setup(x => x.GetByIdAsync(data[0].Id)).ReturnsAsync(data[0]);

        var service = new PeopleController(mockRepository.Object, mapper);
        await service.DeletePerson(data[0].Id);

        mockRepository.Verify(m => m.DeleteAsync(data[0]), Times.Once());
    }

    [TestMethod]
    public async Task PersonUpdatedOnPut()
    {
        var mapper = TestUtils.GetMapper();
        var service = new PeopleController(mockRepository.Object, mapper);

        var updateModel = new PersonUpdateModel(data[0].Id, data[0].FirstName, data[0].LastName);

        await service.PutPerson(data[0].Id, updateModel);

        mockRepository.Verify(m => m.UpdateAsync(It.IsAny<Person>()), Times.Once());
    }
}
