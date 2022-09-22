namespace Retail.DataAccess;

using Models;
using Queries;

public interface IInvoiceRepository
{
    Task<IEnumerable<Invoice>> QueryAsync(GetListQuery query);

    Task<int> CountAsync(Dictionary<string, object> filters);

    Task<Invoice> QueryAsync(int id);

    Task<Invoice> CreateAsync(Invoice invoice);

    Task<Invoice> UpdateAsync(int id, Invoice invoice);

    Task<int> DeleteAsync(int id);
}

public class InvoiceRepository : IInvoiceRepository
{
    private const string Table = "invoices";

    private const string Columns = "date, command_id, customer_id, total_ex_taxes, delivery_fees, tax_rate, taxes, total";

    private readonly IRepository repository;

    public InvoiceRepository(IRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<Invoice>> QueryAsync(GetListQuery query)
    {
        return await repository.QueryAsync<Invoice>(Table, query);
    }

    public async Task<int> CountAsync(Dictionary<string, object> filters)
    {
        return await repository.CountAsync(Table, filters);
    }

    public async Task<Invoice> QueryAsync(int id)
    {
        return await repository.QueryAsync<Invoice>(Table, id);
    }

    public async Task<Invoice> CreateAsync(Invoice invoice)
    {
        invoice.id = await repository.CreateAsync(Table, Columns, invoice);

        return invoice;
    }

    public async Task<Invoice> UpdateAsync(int id, Invoice invoice)
    {
        invoice.id = id;
        await repository.UpdateAsync(Table, Columns, invoice);

        return invoice;
    }

    public async Task<int> DeleteAsync(int id)
    {
        await repository.DeleteAsync(Table, id);

        return id;
    }
}
