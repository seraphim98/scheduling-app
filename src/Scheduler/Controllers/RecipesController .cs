using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduler.Database;
using Scheduler.Models;

namespace Scheduler.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecipesController(IRepository<Recipe> repository, IMapper mapper) : ControllerBase
{
    private readonly IRepository<Recipe> _repository = repository;
    private readonly IMapper _mapper = mapper;

    // GET: api/Recipes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RecipeModel>>> GetRecipes()
    {
        var recipes = await _repository.Query();
        return recipes.Select(recipe => _mapper.Map<RecipeModel>(recipe)).ToList();
    }

    // GET: api/Recipes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<RecipeModel>> GetRecipe(Guid id)
    {
        var recipe = await _repository.GetByIdAsync(id);

        if (recipe == null)
        {
            return NotFound();
        }
        return _mapper.Map<RecipeModel>(recipe);
    }

    // POST: api/Recipes
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<RecipeCreateModel>> PostRecipe(
        RecipeCreateModel RecipeCreateModel
    )
    {
        var recipe = _mapper.Map<Recipe>(RecipeCreateModel);
        await _repository.CreateAsync(recipe);

        return CreatedAtAction(nameof(GetRecipe), new { id = recipe.Id }, recipe);
    }

    // DELETE: api/Recipes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecipe(Guid id)
    {
        var recipe = await _repository.GetByIdAsync(id);
        if (recipe == null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(recipe);

        return NoContent();
    }

    // PUT: api/Recipes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRecipe(Guid id, RecipeUpdateModel recipeModel)
    {
        var recipe = _mapper.Map<Recipe>(recipeModel);
        if (id != recipe.Id)
        {
            return BadRequest();
        }

        try
        {
            await _repository.UpdateAsync(recipe);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await RecipeExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    private async Task<bool> RecipeExists(Guid id)
    {
        return await _repository.GetByIdAsync(id) != null;
    }
}
