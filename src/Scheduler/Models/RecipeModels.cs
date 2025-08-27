public record class RecipeModel(Guid Id, string Name, string Type, string Ingredients);

public class RecipeCreateModel(string name, string type, string ingredients)
{
    public string Name { get; set; } = name;
    public string Type { get; set; } = type;
    public string Ingredients { get; set; } = ingredients;
}

public class RecipeUpdateModel(Guid id, string name, string type, string ingredients)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Type { get; set; } = type;
    public string Ingredients { get; set; } = ingredients;
}
