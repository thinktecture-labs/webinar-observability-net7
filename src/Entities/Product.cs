namespace Thinktecture.Webinars.SampleApi.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<Category> Categories { get; set; } = new();
}
