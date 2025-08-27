namespace Scheduler.Models;

public class PersonEvent(Guid id, Guid personId, Guid eventId, string eventName)
{
    public Guid Id { get; set; } = id;
    public Guid PersonId { get; set; } = personId;
    public Guid EventId { get; set; } = eventId;
    public string EventName { get; set; } = eventName;
}
