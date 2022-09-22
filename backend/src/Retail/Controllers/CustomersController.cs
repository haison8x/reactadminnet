namespace Retail.Controllers;

using DataAccess;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Models;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository repository;

    public CustomersController(ICustomerRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    [Route("")]
    public async Task<IEnumerable<Customer>> GetList(string filter, string range, string sort)
    {
        var getListQuery = QueryHelper.ToGetListQuery(filter, range, sort);
        var customers = await repository.QueryAsync(getListQuery);
        var count = await repository.CountAsync(getListQuery.Filter);

        HttpContext.IncludeContentRange("customers", getListQuery.Range, count);

        return customers;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<Customer> GetOne(int id)
    {
        return await repository.QueryAsync(id);
    }

    [HttpPost]
    [Route("")]
    public async Task<Customer> Create(Customer customer)
    {
        return await repository.CreateAsync(customer);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<Customer> Update(int id, Customer customer)
    {
        return await repository.UpdateAsync(id, customer);
    }

    [HttpPut]
    [Route("")]
    public async Task<int[]> UpdateMany(string filter, Customer customer)
    {
        var ids = QueryHelper.ToIdFilters(filter);
        foreach (var id in ids)
        {
            await repository.UpdateAsync(id, customer);
        }

        return ids;
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<Customer> Delete(int id)
    {
        var customer = await repository.QueryAsync(id);
        await repository.DeleteAsync(id);

        return customer;
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