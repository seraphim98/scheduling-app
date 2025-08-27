using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduler.Database;
using Scheduler.Models;

namespace Scheduler.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PeopleController(IRepository<Person> repository, IMapper mapper) : ControllerBase
{
    private readonly IRepository<Person> _repository = repository;
    private readonly IMapper _mapper = mapper;

    // GET: api/People
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonModel>>> GetPeople()
    {
        var people = await _repository.Query();
        return people.Select(person => _mapper.Map<PersonModel>(person)).ToList();
    }

    // GET: api/People/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PersonModel>> GetPerson(Guid id)
    {
        var person = await _repository.GetByIdAsync(id);

        if (person == null)
        {
            return NotFound();
        }
        return _mapper.Map<PersonModel>(person);
    }

    // POST: api/People
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<PersonCreateModel>> PostPerson(
        PersonCreateModel personCreateModel
    )
    {
        var person = _mapper.Map<Person>(personCreateModel);
        await _repository.CreateAsync(person);

        return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, person);
    }

    // DELETE: api/People/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(Guid id)
    {
        var person = await _repository.GetByIdAsync(id);
        if (person == null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(person);

        return NoContent();
    }

    // PUT: api/People/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPerson(Guid id, PersonUpdateModel personModel)
    {
        var person = _mapper.Map<Person>(personModel);
        if (id != person.Id)
        {
            return BadRequest();
        }

        try
        {
            await _repository.UpdateAsync(person);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await PersonExists(id))
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

    private async Task<bool> PersonExists(Guid id)
    {
        return await _repository.GetByIdAsync(id) != null;
    }
}
