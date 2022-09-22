namespace Auth;

using System.Data;
using Dapper;
using Microsoft.Extensions.DependencyInjection;

public interface IUserRepository
{
    Task<UserDTO> GetUserAsync(UserModel userModel);
}

public class UserRepository : IUserRepository
{
    private readonly IServiceProvider serviceProvider;

    public UserRepository(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    /// <summary>
    ///     For simple demo, I used plaintext password
    /// </summary>
    /// <param name="userModel"></param>
    /// <returns></returns>
    public async Task<UserDTO> GetUserAsync(UserModel userModel)
    {
        var querySql = "SELECT * FROM users WHERE username = @username AND password = @password AND role = @role";

        using var connection = serviceProvider.GetService<IDbConnection>();
        return await connection.QueryFirstOrDefaultAsync<UserDTO>(querySql, userModel);
    }
}