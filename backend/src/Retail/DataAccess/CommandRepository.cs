namespace Retail.DataAccess;

using Models;
using Queries;

public interface ICommandRepository
{
    Task<IEnumerable<Command>> QueryAsync(GetListQuery query);

    Task<int> CountAsync(Dictionary<string, object> filters);

    Task<Command> QueryAsync(int id);

    Task<Command> CreateAsync(Command command);

    Task<Command> UpdateAsync(int id, Command command);

    Task<int> DeleteAsync(int id);
}

public class CommandRepository : ICommandRepository
{
    private const string Table = "commands";

    private const string BasketTable = "baskets";

    private const string Columns = "reference, date, customer_id, total_ex_taxes, delivery_fees, "
                                   + "tax_rate, taxes, total, status, returned";

    private readonly IRepository repository;

    public CommandRepository(IRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<Command>> QueryAsync(GetListQuery query)
    {
        var commands = (await repository.QueryAsync<Command>(Table, query)).ToArray();
        foreach (var command in commands)
        {
            command.basket = (await GetBasketsByCommandId(command.id)).ToList();
        }

        return commands;
    }

    public async Task<int> CountAsync(Dictionary<string, object> filters)
    {
        return await repository.CountAsync(Table, filters);
    }

    public async Task<Command> QueryAsync(int id)
    {
        var command = await repository.QueryAsync<Command>(Table, id);

        command.basket = (await GetBasketsByCommandId(id)).ToList();

        return command;
    }

    private async Task<IEnumerable<Basket>> GetBasketsByCommandId(int id)
    {
        var filter = new Dictionary<string, object>
        {
            { "command_id", id }
        };

        var query = new GetListQuery
        {
            Filter = filter,
            Sort = Array.Empty<string>(),
            Range = Array.Empty<int>()
        };

        return await repository.QueryAsync<Basket>(BasketTable, query);
    }

    public async Task<Command> CreateAsync(Command command)
    {
        command.id = await repository.CreateAsync(Table, Columns, command);

        return command;
    }

    public async Task<Command> UpdateAsync(int id, Command command)
    {
        command.id = id;
        await repository.UpdateAsync(Table, Columns, command);

        return command;
    }

    public async Task<int> DeleteAsync(int id)
    {
        await repository.DeleteAsync(Table, id);

        return id;
    }
}