using Moq;
using Scheduler.Controllers;
using Scheduler.Database;
using Scheduler.Models;

//Source https://learn.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking
namespace Scheduler.Tests;

[TestClass]
public class RecipesControllerTests
{
    Mock<IRepository<Recipe>> mockRepository = new();
    private readonly List<Recipe> data = new List<Recipe>
    {
        new(Guid.NewGuid(), "1", "Pudding", "cheese"),
        new(Guid.NewGuid(), "2", "Pudding", "cheese"),
        new(Guid.NewGuid(), "3", "Pudding", "cheese"),
    };

    [TestMethod]
    public async Task RecipesReturnedOnGet()
    {
        mockRepository.Setup(x => x.Query()).ReturnsAsync(data);
        var mapper = TestUtils.GetMapper();
        var service = new RecipesController(mockRepository.Object, mapper);

        var holidays = await service.GetRecipes();

        mockRepository.Verify(x => x.Query(), Times.Once());
    }

    [TestMethod]
    public async Task RecipeCreatedOnPost()
    {
        var mapper = TestUtils.GetMapper();
        var service = new RecipesController(mockRepository.Object, mapper);

        await service.PostRecipe(new RecipeCreateModel("Test", "Pudding", "Cake"));

        mockRepository.Verify(m => m.CreateAsync(It.IsAny<Recipe>()), Times.Once());
    }

    [TestMethod]
    public async Task RecipeDeletedOnDelete()
    {
        var mapper = TestUtils.GetMapper();
        mockRepository.Setup(x => x.GetByIdAsync(data[0].Id)).ReturnsAsync(data[0]);

        var service = new RecipesController(mockRepository.Object, mapper);
        await service.DeleteRecipe(data[0].Id);

        mockRepository.Verify(m => m.DeleteAsync(data[0]), Times.Once());
    }

    [TestMethod]
    public async Task RecipeUpdatedOnPut()
    {
        var mapper = TestUtils.GetMapper();
        var service = new RecipesController(mockRepository.Object, mapper);

        var updateModel = new RecipeUpdateModel(
            data[0].Id,
            data[0].Name,
            data[0].Type,
            data[0].Ingredients
        );

        await service.PutRecipe(data[0].Id, updateModel);

        mockRepository.Verify(m => m.UpdateAsync(It.IsAny<Recipe>()), Times.Once());
    }
}
