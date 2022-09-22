namespace Retail.DataAccess;

using Models;
using Queries;

public interface IReviewRepository
{
    Task< IEnumerable<Review>> QueryAsync(GetListQuery query);

    Task<int> CountAsync(Dictionary<string, object> filters);

    Task<Review> QueryAsync(int id);

    Task<Review> CreateAsync(Review review);

    Task<Review> UpdateAsync(int id, Review review);

    Task<int> DeleteAsync(int id);
}

public class ReviewRepository : IReviewRepository
{
    private const string Table = "reviews";

    private const string Columns = "date, status, command_id, product_id, customer_id, rating, comment";
    
    private readonly IRepository repository;

    public ReviewRepository(IRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<Review>> QueryAsync(GetListQuery query)
    {
        return await repository.QueryAsync<Review>(Table, query);
    }

    public async Task<int> CountAsync(Dictionary<string, object> filters)
    {
        return await repository.CountAsync(Table, filters);
    }

    public async Task<Review> QueryAsync(int id)
    {
        return await repository.QueryAsync<Review>(Table, id);
    }

    public async Task<Review> CreateAsync(Review review)
    {
        review.id = await repository.CreateAsync(Table, Columns, review);
        
        return review;
    }

    public async Task<Review> UpdateAsync(int id, Review review)
    {
        review.id = id;
        await repository.UpdateAsync(Table, Columns, review);

        return review;
    }

    public async Task<int> DeleteAsync(int id)
    {
       await repository.DeleteAsync(Table, id);

        return id;
    }
}
