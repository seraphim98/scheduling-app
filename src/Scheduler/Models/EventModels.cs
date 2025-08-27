using Scheduler.Models;

public record class EventModel(
    Guid Id,
    string Name,
    DateTime StartTime,
    DateTime EndTime,
    List<PersonModel> People
);

public class EventCreateModel(string name, DateTime startTime, DateTime endTime)
{
    public string Name { get; set; } = name;
    public DateTime StartTime { get; set; } = startTime;
    public DateTime EndTime { get; set; } = endTime;
}

public class EventUpdateModel(
    Guid id,
    string name,
    DateTime startTime,
    DateTime endTime,
    List<Guid> people
)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public DateTime StartTime { get; set; } = startTime;
    public DateTime EndTime { get; set; } = endTime;
    public List<Guid> People { get; set; } = people;
}
