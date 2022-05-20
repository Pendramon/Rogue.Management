using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Rogue.Management.Data;
using Rogue.Management.Data.Model;
using Rogue.Management.Service.Cryptography.Interfaces;
using Rogue.Management.Service.Interfaces;
using Rogue.Management.View.Model;
using Rogue.Management.View.Model.Interfaces;

namespace Rogue.Management.Service;

public class UserService : IUserService
{
    private readonly RogueContext dbContext;
    private readonly IHashService hashService;
    private readonly IConfiguration configuration;
    private readonly IMapper mapper;

    public UserService(RogueContext dbContext, IHashService hashService, IConfiguration configuration, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.hashService = hashService;
        this.configuration = configuration;
        this.mapper = mapper;
    }

    public async Task<Response<AuthenticationResult>> RegisterAsync(RegisterViewModel userRegisterModel)
    {
        var errors = new List<IError>();

        var users = await this.dbContext.Users.Where(u =>
            u.Username.ToLower() == userRegisterModel.Username.ToLower() ||
            u.Email.ToLower() == userRegisterModel.Email.ToLower()).ToListAsync();

        if (users.Any(u => string.Equals(u.Username, userRegisterModel.Username, StringComparison.CurrentCultureIgnoreCase)))
        {
            errors.Add(new Error("Username", "User with this username already exists."));
        }

        if (users.Any(u => string.Equals(u.Email, userRegisterModel.Email, StringComparison.CurrentCultureIgnoreCase)))
        {
            errors.Add(new Error("Email", "User with this email address already exists."));
        }

        if (errors.Any())
        {
            return new Response<AuthenticationResult>()
            {
                Errors = errors,
                Succeeded = false,
                Result = null,
            };
        }

        var user = new UserDto(userRegisterModel.Username, userRegisterModel.Email.ToLower(), await this.hashService.ComputeHashAsync(userRegisterModel.Password), DateTime.UtcNow);

        this.dbContext.Add(user);
        await this.dbContext.SaveChangesAsync();

        return new Response<AuthenticationResult>
        {
            Succeeded = true,
            Result = new AuthenticationResult(this.GenerateToken(user), this.mapper.Map<UserDto, User>(user)),
        };
    }

    public async Task<Response<AuthenticationResult>> LoginAsync(LoginViewModel userLoginModel)
    {
        var errors = new List<IError>();

        var user = new EmailAddressAttribute().IsValid(userLoginModel.UsernameOrEmail)
            ? this.dbContext.Users.FirstOrDefault(u => u.Email.ToLower() == userLoginModel.UsernameOrEmail.ToLower())
            : this.dbContext.Users.FirstOrDefault(u => u.Username.ToLower() == userLoginModel.UsernameOrEmail.ToLower());

        if (user == null)
        {
            errors.Add(new Error("UsernameOrEmail", "The account does not exist."));
            return new Response<AuthenticationResult>()
            {
                Errors = errors,
                Succeeded = false,
            };
        }

        var result = await this.hashService.CompareHashAsync(userLoginModel.Password, user.PasswordHash);

        if (!result.Match)
        {
            errors.Add(new Error("Password", "Incorrect password."));
            return new Response<AuthenticationResult>()
            {
                Errors = errors,
                Succeeded = false,
            };
        }

        if (result.Rehashed)
        {
            user.PasswordHash = result.RehashResult!;
            this.dbContext.Users.Update(user);
            await this.dbContext.SaveChangesAsync();
        }

        return new Response<AuthenticationResult>()
        {
            Succeeded = true,
            Errors = errors,
            Result = new AuthenticationResult(this.GenerateToken(user), this.mapper.Map<UserDto, User>(user)),
        };
    }

    private string GenerateToken(UserDto user)
    {
        var symmetricKey = Encoding.ASCII.GetBytes(this.configuration["Jwt:Secret"]);
        var tokenHandler = new JwtSecurityTokenHandler();

        var now = DateTime.UtcNow;
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                    new Claim(ClaimTypes.NameIdentifier, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
            }),

            Expires = now.AddDays(7),

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(symmetricKey),
                SecurityAlgorithms.HmacSha256Signature),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}