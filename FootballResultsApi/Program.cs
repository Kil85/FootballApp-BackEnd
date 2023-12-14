using FootballResultsApi.Authentication;
using FootballResultsApi.Entities;
using FootballResultsApi.Helpers;
using FootballResultsApi.Interfaces;
using FootballResultsApi.Middleware;
using FootballResultsApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var authenticationSettings = new AuthenticationSettings();
var httpData = new HttpData();

builder.Configuration.GetSection("JWTInfo").Bind(authenticationSettings);
builder.Configuration.GetSection("HttpData").Bind(httpData);

builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddSingleton(httpData);
builder.Services.AddDirectoryBrowser();
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "Front",
        builder =>
        {
            builder.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader();
        }
    );
});

builder.Services
    .AddAuthentication(option =>
    {
        option.DefaultAuthenticateScheme = "Bearer";
        option.DefaultScheme = "Bearer";
        option.DefaultChallengeScheme = "Bearer";
    })
    .AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = authenticationSettings.JwtIssuer,
            ValidAudience = authenticationSettings.JwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)
            ),
        };
    });

builder.Services.AddControllers();

builder.Services.AddDbContext<FootballResultsDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"))
);

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<FetchApiData>();
builder.Services.AddScoped<ExceptionHandlingMiddleware>();
builder.Logging.ClearProviders();
builder.Host.UseNLog();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme.",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer"
        }
    );
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

var app = builder.Build();
app.UseCors("Front");
app.UseDirectoryBrowser();
app.UseMiddleware<ExceptionHandlingMiddleware>();

var scope = app.Services.CreateScope();
var fetchApi = scope.ServiceProvider.GetRequiredService<FetchApiData>();

await fetchApi.FeachFixtures();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Registration Task");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
