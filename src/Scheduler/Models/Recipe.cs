namespace Scheduler.Models;

public class Recipe(Guid id, string name, string type, string ingredients)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Type { get; set; } = type;
    public string Ingredients { get; set; } = ingredients;
}
