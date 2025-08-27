using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduler.Database;
using Scheduler.Models;

namespace Scheduler.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsController(
    IRepository<Event> eventRepository,
    IRepository<PersonEvent> personEventRepository,
    IMapper mapper
) : ControllerBase
{
    private readonly IMapper _mapper = mapper;
    private readonly IRepository<Event> _eventRepository = eventRepository;
    private readonly IRepository<PersonEvent> _personEventRepository = personEventRepository;

    // GET: api/Events
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventModel>>> GetEvents()
    {
        var events = await _eventRepository.Query();
        return events.Select(person => _mapper.Map<EventModel>(person)).ToList();
    }

    // GET: api/Events/5
    [HttpGet("{id}")]
    public async Task<ActionResult<EventModel>> GetEvent(Guid id)
    {
        var @event = await _eventRepository.GetByIdAsync(id);

        if (@event == null)
        {
            return NotFound();
        }

        return _mapper.Map<EventModel>(@event);
    }

    // PUT: api/Events/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEvent(Guid id, EventUpdateModel @eventModel)
    {
        var @event = _mapper.Map<Event>(@eventModel);
        if (id != @eventModel.Id)
        {
            return BadRequest();
        }

        _eventRepository.Update(@event);

        try
        {
            if (@event.People.Count != 0)
            {
                var personEvents = await _personEventRepository.Query();
                var matchingPersonEvents = personEvents.Where(e => e.EventId == id).ToList();
                var peopleToAdd = @event.People.Where(p =>
                    !matchingPersonEvents.Select(pe => pe.PersonId).Contains(p.Id)
                );
                var personEventsToRemove = matchingPersonEvents.Where(pe =>
                    !@event.People.Select(p => p.Id).Contains(pe.PersonId)
                );
                foreach (var person in peopleToAdd)
                {
                    _personEventRepository.Create(
                        new PersonEvent(Guid.NewGuid(), person.Id, id, @event.Name)
                    );
                }
                foreach (var personEvent in personEventsToRemove)
                {
                    _personEventRepository.Delete(personEvent);
                }
                await _personEventRepository.SaveAsync();
            }

            await _eventRepository.SaveAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await EventExists(id))
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

    // POST: api/Events
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<EventCreateModel>> PostEvent(EventCreateModel @eventModel)
    {
        var @event = _mapper.Map<Event>(@eventModel);
        await _eventRepository.CreateAsync(@event);
        return CreatedAtAction("GetEvent", new { id = @event.Id }, @event);
    }

    // DELETE: api/Events/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        var @event = await _eventRepository.GetByIdAsync(id);
        if (@event == null)
        {
            return NotFound();
        }
        var personEvents = await _personEventRepository.Query();
        var matchingPersonEvents = personEvents.Where(e => e.EventId == id).ToList();
        foreach (var personEvent in matchingPersonEvents)
        {
            _personEventRepository.Delete(personEvent);
        }
        await _personEventRepository.SaveAsync();

        await _eventRepository.DeleteAsync(@event);

        return NoContent();
    }

    private async Task<bool> EventExists(Guid id)
    {
        return await _eventRepository.GetByIdAsync(id) != null;
    }
}
