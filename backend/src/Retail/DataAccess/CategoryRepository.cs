namespace Retail.DataAccess;

using Models;
using Queries;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> QueryAsync(GetListQuery query);

    Task<int> CountAsync(Dictionary<string, object> filters);

    Task<Category> QueryAsync(int id);

    Task<Category> CreateAsync(Category category);

    Task<Category> UpdateAsync(int id, Category category);

    Task<int> DeleteAsync(int id);
}

public class CategoryRepository : ICategoryRepository
{
    private const string Table = "categories";

    private const string Columns = "name";
    
    private readonly IRepository repository;

    public CategoryRepository(IRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<Category>> QueryAsync(GetListQuery query)
    {
        return await repository.QueryAsync<Category>(Table, query);
    }

    public async Task<int> CountAsync(Dictionary<string, object> filters)
    {
        return await repository.CountAsync(Table, filters);
    }

    public async Task<Category> QueryAsync(int id)
    {
        return await repository.QueryAsync<Category>(Table, id);
    }

    public async Task<Category> CreateAsync(Category category)
    {
        category.id = await repository.CreateAsync(Table, Columns, category);
        
        return category;
    }

    public async Task<Category> UpdateAsync(int id, Category category)
    {
        category.id = id;
        await repository.UpdateAsync(Table, Columns, category);

        return category;
    }

    public async Task<int> DeleteAsync(int id)
    {
        await  repository.DeleteAsync(Table, id);
        
        return id;
    }
}
