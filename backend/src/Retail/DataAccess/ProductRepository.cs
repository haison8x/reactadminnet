namespace Retail.DataAccess;

using Models;
using Queries;

public interface IProductRepository
{
    Task<IEnumerable<Product>> QueryAsync(GetListQuery query);

    Task<int> CountAsync(Dictionary<string, object> filters);

    Task<Product> QueryAsync(int id);

    Task<Product> CreateAsync(Product product);

    Task<Product> UpdateAsync(int id, Product product);

    Task<int> DeleteAsync(int id);
}

public class ProductRepository : IProductRepository
{
    private const string Table = "products";

    private const string Columns = "category_id, reference, width, height, price, thumbnail, image, "
                                   + "description, stock, sales";

    private readonly IRepository repository;

    public ProductRepository(IRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<Product>> QueryAsync(GetListQuery query)
    {
        return await repository.QueryAsync<Product>(Table, query);
    }

    public async Task<int> CountAsync(Dictionary<string, object> filters)
    {
        return await repository.CountAsync(Table, filters);
    }

    public async Task<Product> QueryAsync(int id)
    {
        return await repository.QueryAsync<Product>(Table, id);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        product.id = await repository.CreateAsync(Table, Columns, product);

        return product;
    }

    public async Task<Product> UpdateAsync(int id, Product product)
    {
        product.id = id;
        await repository.UpdateAsync(Table, Columns, product);

        return product;
    }

    public async Task<int> DeleteAsync(int id)
    {
        await repository.DeleteAsync(Table, id);

        return id;
    }
}
