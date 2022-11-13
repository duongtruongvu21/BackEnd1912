using System.Text;
using DatingApp.API.Data;
using DatingApp.API.Data.Repositories;
using DatingApp.API.Data.Seed;
using DatingApp.API.Profilespace;
using DatingApp.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var connectionString = builder.Configuration.GetConnectionString("Default"); // Default là tên cấu hình ở appsetting.json
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_myCORS",
                      policy =>
                      {
                          policy.WithOrigins("*")
                          .AllowAnyMethod().AllowAnyHeader();
                      });
});

var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
// Replace 'YourDbContext' with the name of your own DbContext derived class.
services.AddDbContext<DataContext>(
    dbContextOptions => dbContextOptions
        .UseMySql(connectionString, serverVersion)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);

services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IAuthUserService, AuthService>();
services.AddScoped<IMemberService, MemberService>();
services.AddScoped<ITokenService, TokenService>();
services.AddAutoMapper(typeof(UserMapperProfile).Assembly);

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"]))
        };
    });


var app = builder.Build();

using var scope = app.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;

try
{
    var context = serviceProvider.GetRequiredService<DataContext>();
    context.Database.Migrate();
    Seed.SeedUser(context);
}
catch (Exception e)
{
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogError(e, "Migration Faile!!");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseCors("_myCORS");

app.UseAuthorization();

app.MapControllers();

app.Run();