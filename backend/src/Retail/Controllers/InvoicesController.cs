namespace Retail.Controllers;

using DataAccess;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Models;

[ApiController]
[Route("api/invoices")]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceRepository repository;

    public InvoicesController(IInvoiceRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    [Route("")]
    public async Task<IEnumerable<Invoice>> GetList(string filter, string range, string sort)
    {
        var getListQuery = QueryHelper.ToGetListQuery(filter, range, sort);
        var invoices = await repository.QueryAsync(getListQuery);
        var count = await repository.CountAsync(getListQuery.Filter);

        HttpContext.IncludeContentRange("invoices", getListQuery.Range, count);

        return invoices;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<Invoice> GetOne(int id)
    {
        return await repository.QueryAsync(id);
    }

    [HttpPost]
    [Route("")]
    public async Task<Invoice> Create(Invoice invoice)
    {
        return await repository.CreateAsync(invoice);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<Invoice> Update(int id, Invoice invoice)
    {
        return await repository.UpdateAsync(id, invoice);
    }

    [HttpPut]
    [Route("")]
    public async Task<int[]> UpdateMany(string filter, Invoice invoice)
    {
        var ids = QueryHelper.ToIdFilters(filter);
        foreach (var id in ids)
        {
            await repository.UpdateAsync(id, invoice);
        }

        return ids;
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<Invoice> Delete(int id)
    {
        var invoice = await repository.QueryAsync(id);
        await repository.DeleteAsync(id);

        return invoice;
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