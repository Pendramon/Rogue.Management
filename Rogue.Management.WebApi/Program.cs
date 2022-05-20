using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Rogue.Management.Data;
using Rogue.Management.Service;
using Rogue.Management.Service.Cryptography;
using Rogue.Management.Service.Cryptography.Interfaces;
using Rogue.Management.Service.Interfaces;
using Rogue.Service.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<RogueContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("RogueManagement")));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddTransient<IHashFunction, BCryptFunction>();
builder.Services.AddSingleton<IHashService, HashService>();
builder.Services.AddScoped<IUserService, UserService>();

var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Secret"]);
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateLifetime = false,
        ValidateIssuer = false,
        ValidateAudience = false,

        // TODO: Change this shit.
        ClockSkew = TimeSpan.Zero, // remove delay of token when expire
    };
});

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
