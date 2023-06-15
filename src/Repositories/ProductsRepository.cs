using Microsoft.EntityFrameworkCore;
using Thinktecture.Webinars.SampleApi.Entities;

namespace Thinktecture.Webinars.SampleApi.Repositories;

public class ProductsRepository
{
    private readonly SampleApiContext _ctx;

    public ProductsRepository(SampleApiContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<List<Product>> GetAllProducts()
    {
        using var a = Observability.Default.StartActivity("Retrieving products from DB");
        var products = await _ctx.Products.OrderBy(p => p.Name).ToListAsync();
        a?.SetTag("thinktecture.products.count", products.Count);
        return products;
    }

    public async Task<Product?> GetProductById(Guid id)
    {
        return await _ctx.Products
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> Create(Product p)
    {
        _ctx.Products.Add(p);
        await _ctx.SaveChangesAsync();
        return p;
    }

    public async Task<Product> UpdateProduct(Product p)
    {
        _ctx.Update(p);
        await _ctx.SaveChangesAsync();
        return p;
    }

    public async Task<bool> DeleteProductById(Guid id)
    {
        var found = await _ctx.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (found == null)
        {
            return false;
        }

        _ctx.Products.Remove(found);
        var affected = await _ctx.SaveChangesAsync();
        return affected == 1;
    }
}
