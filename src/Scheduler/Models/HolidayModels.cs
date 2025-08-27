public record class HolidayModel(Guid Id, string Name, DateTime Date);

public class HolidayCreateModel(string name, DateTime date)
{
    public string Name { get; set; } = name;
    public DateTime Date { get; set; } = date;
}

public class HolidayUpdateModel(Guid id, string name, DateTime date)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public DateTime Date { get; set; } = date;
}
