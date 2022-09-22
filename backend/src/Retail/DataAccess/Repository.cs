namespace Retail.DataAccess;

using System.Data;
using Dapper;
using Helpers;
using Microsoft.Extensions.DependencyInjection;
using Queries;

public interface IRepository
{
    Task<int> CountAsync(string table, Dictionary<string, object> filters);

    Task<IEnumerable<T>> QueryAsync<T>(string table, GetListQuery query);

    Task<T> QueryAsync<T>(string table, int id);

    Task<int> CreateAsync<T>(string table, string columns, T item);

    Task UpdateAsync<T>(string table, string columns, T item);

    Task<int> DeleteAsync(string table, int id);
}

public class Repository : IRepository
{
    private readonly IServiceProvider serviceProvider;

    public Repository(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    
    public async Task< IEnumerable<T>> QueryAsync<T>(string table, GetListQuery query)
    {
        var builder = new SqlBuilder();
        var sql = $"SELECT * FROM {table} /**where**/ /**orderby**/ LIMIT @start, @count";
        var template = builder.AddQueryTemplate(sql, query.Range);

        builder.Where(query.Filter);
        builder.OrderBy(query.Sort);

        using var connection = serviceProvider.GetService<IDbConnection>();
        return await connection.QueryAsync<T>(template.RawSql, template.Parameters);
    }

    public async Task<int> CountAsync(string table, Dictionary<string, object> filters)
    {
        var builder = new SqlBuilder();

        var sql = $"SELECT COUNT(1) FROM {table} /**where**/";
        var template = builder.AddTemplate(sql);

        builder.Where(filters);

        using var connection = serviceProvider.GetService<IDbConnection>();
        return await connection.ExecuteScalarAsync<int>(template.RawSql, template.Parameters);
    }

    public async Task<T> QueryAsync<T>(string table, int id)
    {
        var querySql = $"SELECT * FROM {table} WHERE id = @id";

        using var connection = serviceProvider.GetService<IDbConnection>();
        return await connection.QueryFirstOrDefaultAsync<T>(querySql, new { id });
    }

    public async Task<int> CreateAsync<T>(string table, string columns, T item)
    {
        var columnsArray = columns.Split(',',StringSplitOptions.RemoveEmptyEntries);
        var valueClauses = string.Join(", ", columnsArray.Select(c => $"@{c}"));
        var sql = @$"INSERT INTO {table} ({columns}) VALUES({valueClauses});
                    SELECT LAST_INSERT_ID();";
        using var connection = serviceProvider.GetService<IDbConnection>();
        return await connection.ExecuteScalarAsync<int>(sql, item);
    }

    public async  Task UpdateAsync<T>(string table, string columns, T item)
    {
        var columnsArray = columns.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var setClauses = columnsArray.Select(c=>c.Trim())
            .Select(c=> $"{c} = @{c}");
        var setClausesSql = string.Join(", ", setClauses);
        
        var sql = $"UPDATE {table} SET {setClausesSql} WHERE id = @id";
        using var connection = serviceProvider.GetService<IDbConnection>();
        
        await connection.ExecuteAsync(sql, item);
    }

    public async Task<int> DeleteAsync(string table, int id)
    {
        var sql = @$"DELETE FROM {table} WHERE id = @id";
        
        using var connection = serviceProvider.GetService<IDbConnection>();
        await connection.ExecuteAsync(sql, new { id });

        return id;
    }
}
