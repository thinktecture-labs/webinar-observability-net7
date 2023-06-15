namespace Thinktecture.Webinars.SampleApi.Models;

public class ProductDetailsModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public IEnumerable<string> Categories { get; set; } = new List<string>();
}
