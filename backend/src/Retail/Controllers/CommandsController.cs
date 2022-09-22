namespace Retail.Controllers;

using DataAccess;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Models;

[ApiController]
[Route("api/commands")]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepository repository;

    public CommandsController(ICommandRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    [Route("")]
    public async Task<IEnumerable<Command>> GetList(string filter, string range, string sort)
    {
        var getListQuery = QueryHelper.ToGetListQuery(filter, range, sort);
        var commands = await repository.QueryAsync(getListQuery);
        var count = await repository.CountAsync(getListQuery.Filter);

        HttpContext.IncludeContentRange("commands", getListQuery.Range, count);

        return commands;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<Command> GetOne(int id)
    {
        return await repository.QueryAsync(id);
    }

    [HttpPost]
    [Route("")]
    public async Task<Command> Create(Command command)
    {
        return await repository.CreateAsync(command);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<Command> Update(int id, Command command)
    {
        return await repository.UpdateAsync(id, command);
    }

    [HttpPut]
    [Route("")]
    public async Task<int[]> UpdateMany(string filter, Command command)
    {
        var ids = QueryHelper.ToIdFilters(filter);
        foreach (var id in ids)
        {
            await repository.UpdateAsync(id, command);
        }

        return ids;
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<Command> Delete(int id)
    {
        var command = await repository.QueryAsync(id);
        await repository.DeleteAsync(id);

        return command;
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

