namespace Scheduler.Models;

public class Event(Guid id, string name, DateTime startTime, DateTime endTime)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public DateTime StartTime { get; set; } = startTime;
    public DateTime EndTime { get; set; } = endTime;
    public List<Person> People { get; set; } = new();
}
