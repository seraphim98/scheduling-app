namespace Scheduler.Models;

public class Person(Guid id, string firstName, string lastName)
{
    public Guid Id { get; set; } = id;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public List<Event> Events { get; set; } = new();
    public List<PersonEvent> PersonEvents { get; set; } = new();
}
