using Microsoft.EntityFrameworkCore;
using Thinktecture.Webinars.SampleApi.Entities;

namespace Thinktecture.Webinars.SampleApi.Repositories;

public class CategoriesRepository
{
    private readonly SampleApiContext _ctx;

    public CategoriesRepository(SampleApiContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<List<Category>> GetCategoriesByNames(IEnumerable<string> names)
    {
        var all = await _ctx.Categories
            .OrderBy(c => c.Name)
            .ToListAsync();

        var providedNames = names.ToList();
        var newCategories = providedNames
            .Where(name => all.All(c => c.Name != name))
            .Select(name => new Category { Name = name });

        return all.Where(c => providedNames.Contains(c.Name))
            .Concat(newCategories).ToList();
    }
}
