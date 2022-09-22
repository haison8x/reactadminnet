namespace Auth;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

[ApiController]
[AllowAnonymous]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration configuration;
    private readonly IUserRepository userRepository;
    private readonly ITokenService tokenService;
    public AuthController(IConfiguration configuration, ITokenService tokenService, IUserRepository userRepository)
    {
        this.configuration = configuration;
        this.tokenService = tokenService;
        this.userRepository = userRepository;
    }

    [Route("login")]
    [HttpPost]
    public async Task<LoggedUser> Login(UserModel userModel)
    {
        var validUser = await userRepository.GetUserAsync(userModel);
        if (validUser == null)
        {
            return new LoggedUser();
        }

        var token = tokenService.BuildToken(
            configuration["Jwt:Key"],
            configuration["Jwt:Issuer"],
            userModel);

        var loggedUser = new LoggedUser
        {
            Id = validUser.Id,
            username = validUser.username,
            fullname = validUser.fullname,
            avatar = validUser.avatar,
            role = validUser.role,
            access_token = token ?? string.Empty
        };

        return loggedUser;
    }
}

