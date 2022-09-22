namespace Retail.DataAccess;

using Models;
using Queries;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> QueryAsync(GetListQuery query);

    Task<int> CountAsync(Dictionary<string, object> filters);

    Task<Customer> QueryAsync(int id);

    Task<Customer> CreateAsync(Customer customer);

    Task<Customer> UpdateAsync(int id, Customer customer);

    Task<int> DeleteAsync(int id);
}

public class CustomerRepository : ICustomerRepository
{
    private const string Table = "customers";

    private const string Columns = "first_name, last_name, email, address, zipcode, city, stateAbbr, "
                                   + "avatar, birthday, first_seen, last_seen, has_ordered, "
                                   + "latest_purchase, has_newsletter, groups, nb_commands";

    private readonly IRepository repository;

    public CustomerRepository(IRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<Customer>> QueryAsync(GetListQuery query)
    {
        return await repository.QueryAsync<Customer>(Table, query);
    }

    public async Task<int> CountAsync(Dictionary<string, object> filters)
    {
        return await repository.CountAsync(Table, filters);
    }

    public async Task<Customer> QueryAsync(int id)
    {
        return await repository.QueryAsync<Customer>(Table, id);
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        customer.id = await repository.CreateAsync(Table, Columns, customer);

        return customer;
    }

    public async Task<Customer> UpdateAsync(int id, Customer customer)
    {
        customer.id = id;
        await repository.UpdateAsync(Table, Columns, customer);

        return customer;
    }

    public async Task<int> DeleteAsync(int id)
    {
        await repository.DeleteAsync(Table, id);

        return id;
    }
}
