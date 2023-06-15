using Azure.Core;
using Thinktecture.Webinars.SampleApi.Entities;
using Thinktecture.Webinars.SampleApi.Models;
using Thinktecture.Webinars.SampleApi.Repositories;

namespace Thinktecture.Webinars.SampleApi.Services;

public class ProductsService
{
    private readonly ProductsRepository _repository;
    private readonly CategoriesRepository _categoriesRepository;

    public ProductsService(ProductsRepository repository,
        CategoriesRepository categoriesRepository)
    {
        _repository = repository;
        _categoriesRepository = categoriesRepository;
    }

    public async Task<ProductDetailsModel> CreateProduct(CreateProductModel model)
    {
        var categories = await _categoriesRepository.GetCategoriesByNames(model.Categories);
        var entity = new Product
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            Description = model.Description,
            Categories = categories
        };

        var created = await _repository.Create(entity);
        Observability.ProductCount.Add(1);
        return new ProductDetailsModel
        {
            Id = created.Id,
            Name = created.Name,
            Description = created.Description,
            Categories = created.Categories.Select(c => c.Name)
        };
    }

    public async Task<ProductDetailsModel?> UpdateProductById(Guid id, ProductUpdateModel model)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentOutOfRangeException(nameof(id));
        }

        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        var entity = await _repository.GetProductById(id);
        if (entity == null)
        {
            return null;
        }

        var categories = await _categoriesRepository.GetCategoriesByNames(model.Categories);
        entity.Name = model.Name;
        entity.Description = model.Description;
        entity.Categories = categories;

        var updated = await _repository.UpdateProduct(entity);
        return new ProductDetailsModel
        {
            Id = updated.Id,
            Name = updated.Name,
            Description = updated.Description,
            Categories = updated.Categories.Select(c => c.Name)
        };
    }

    public async Task<List<ProductListModel>> GetAllProducts()
    {
        var entities = await _repository.GetAllProducts();
        using var a = Observability.Default.StartActivity("Converting entities to models");
        return entities.Select(e => new ProductListModel()
        {
            Id = e.Id,
            Name = e.Name
        }).ToList();
    }

    public async Task<ProductDetailsModel?> GetProductById(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentOutOfRangeException(nameof(id));
        }

        var found = await _repository.GetProductById(id);
        if (found == null)
        {
            return null;
        }

        return new ProductDetailsModel
        {
            Id = found.Id,
            Name = found.Name,
            Description = found.Description,
            Categories = found.Categories.Select(c => c.Name)
        };
    }

    public async Task<bool> DeleteProductById(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentOutOfRangeException(nameof(id));
        }

        var succeeded = await _repository.DeleteProductById(id);
        if (succeeded)
        {
            Observability.ProductCount.Add(-1);
        }

        return succeeded;
    }
}
