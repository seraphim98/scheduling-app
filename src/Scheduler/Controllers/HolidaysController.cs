using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduler.Database;
using Scheduler.Models;

namespace Scheduler.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HolidaysController(IRepository<Holiday> repository, IMapper mapper)
    : ControllerBase
{
    private readonly IRepository<Holiday> _repository = repository;
    private readonly IMapper _mapper = mapper;

    // GET: api/Holidays
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HolidayModel>>> GetHolidays()
    {
        var holidays = await _repository.Query();
        return holidays.Select(holiday => _mapper.Map<HolidayModel>(holiday)).ToList();
    }

    // GET: api/Holidays/5
    [HttpGet("{id}")]
    public async Task<ActionResult<HolidayModel>> GetHoliday(Guid id)
    {
        var holiday = await _repository.GetByIdAsync(id);

        if (holiday == null)
        {
            return NotFound();
        }
        return _mapper.Map<HolidayModel>(holiday);
    }

    // POST: api/Holidays
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<HolidayCreateModel>> PostHoliday(
        HolidayCreateModel HolidayCreateModel
    )
    {
        var holiday = _mapper.Map<Holiday>(HolidayCreateModel);
        await _repository.CreateAsync(holiday);

        return CreatedAtAction(nameof(GetHoliday), new { id = holiday.Id }, holiday);
    }

    // DELETE: api/Holidays/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHoliday(Guid id)
    {
        var holiday = await _repository.GetByIdAsync(id);
        if (holiday == null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(holiday);

        return NoContent();
    }

    // PUT: api/Holidays/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutHoliday(Guid id, HolidayUpdateModel holidayModel)
    {
        var holiday = _mapper.Map<Holiday>(holidayModel);
        if (id != holiday.Id)
        {
            return BadRequest();
        }

        try
        {
            await _repository.UpdateAsync(holiday);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await HolidayExists(id))
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

    private async Task<bool> HolidayExists(Guid id)
    {
        return await _repository.GetByIdAsync(id) != null;
    }
}
