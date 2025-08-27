namespace Scheduler.Models;

public class Holiday(Guid id, string name, DateTime date)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public DateTime Date { get; set; } = date;
}
