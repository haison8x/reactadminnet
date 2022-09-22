namespace Retail.Controllers;

using DataAccess;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Models;

[ApiController]
[Route("api/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewRepository repository;

    public ReviewsController(IReviewRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    [Route("")]
    public async Task<IEnumerable<Review>> GetList(string filter, string range, string sort)
    {
        var getListQuery = QueryHelper.ToGetListQuery(filter, range, sort);
        var reviews = await repository.QueryAsync(getListQuery);
        var count = await repository.CountAsync(getListQuery.Filter);

        HttpContext.IncludeContentRange("reviews", getListQuery.Range, count);

        return reviews;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<Review> GetOne(int id)
    {
        return await repository.QueryAsync(id);
    }

    [HttpPost]
    [Route("")]
    public async Task<Review> Create(Review review)
    {
        return await repository.CreateAsync(review);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<Review> Update(int id, Review review)
    {
        return await repository.UpdateAsync(id, review);
    }

    [HttpPut]
    [Route("")]
    public async Task<int[]> UpdateMany(string filter, Review review)
    {
        var ids = QueryHelper.ToIdFilters(filter);
        foreach (var id in ids)
        {
            await repository.UpdateAsync(id, review);
        }

        return ids;
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<Review> Delete(int id)
    {
        var review = await repository.QueryAsync(id);
        await repository.DeleteAsync(id);

        return review;
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