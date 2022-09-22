namespace Retail.Controllers;

using DataAccess;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Models;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository repository;

    public ProductsController(IProductRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    [Route("")]
    public async Task<IEnumerable<Product>> GetList(string filter, string range, string sort)
    {
        var getListQuery = QueryHelper.ToGetListQuery(filter, range, sort);
        var products = await repository.QueryAsync(getListQuery);
        var count = await repository.CountAsync(getListQuery.Filter);

        HttpContext.IncludeContentRange("products", getListQuery.Range, count);

        return products;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<Product> GetOne(int id)
    {
        return await repository.QueryAsync(id);
    }

    [HttpPost]
    [Route("")]
    public async Task<Product> Create(Product product)
    {
        return await repository.CreateAsync(product);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<Product> Update(int id, Product product)
    {
        return await repository.UpdateAsync(id, product);
    }

    [HttpPut]
    [Route("")]
    public async Task<int[]> UpdateMany(string filter, Product product)
    {
        var ids = QueryHelper.ToIdFilters(filter);
        foreach (var id in ids)
        {
            await repository.UpdateAsync(id, product);
        }

        return ids;
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<Product> Delete(int id)
    {
        var product = await repository.QueryAsync(id);
        await repository.DeleteAsync(id);

        return product;
    }

    [HttpDelete]
    [Route("")]
    public async Task<int[]> DeleteMany(string filter)
    {
        var ids = QueryHelper.ToIdFilters(filter);
        foreach (var id in ids)
        {
            await repository.DeleteAsync(id);
        }

        return ids;
    }
}