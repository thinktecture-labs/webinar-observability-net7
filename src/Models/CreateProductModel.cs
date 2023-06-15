using System.ComponentModel.DataAnnotations;

namespace Thinktecture.Webinars.SampleApi.Models;

public class CreateProductModel
{
    [Required] public string Name { get; set; } = null!;
    [Required] public string Description { get; set; } = null!;
    public List<string> Categories { get; set; } = new();
}
