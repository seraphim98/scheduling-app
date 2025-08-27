namespace Scheduler.Models;

public record class PersonModel(
    Guid Id,
    string FirstName,
    string LastName,
    List<string> EventNames
);

public class PersonCreateModel(string firstName, string lastName)
{
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
}

public class PersonUpdateModel(Guid id, string firstName, string lastName)
{
    public Guid Id { get; set; } = id;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
}
