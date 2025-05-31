using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactAppPolicy",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(options =>
    {
        builder.Configuration.Bind("AzureAdB2C", options);

        // Configure token validation parameters
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            NameClaimType = "given_name",
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    },
    options => { builder.Configuration.Bind("AzureAdB2C", options); });

builder.Services.AddAuthorization(options =>
{
    // Basic user policy - requires read scope
    options.AddPolicy("RequireReadScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("http://schemas.microsoft.com/identity/claims/scope", "testIIA-read");
    });

    // Admin policy - requires both read scope and admin role
    options.AddPolicy("RequireAdmin", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("http://schemas.microsoft.com/identity/claims/scope", "testIIA-read");
        policy.RequireClaim("extension_UserRole", "Admin"); // Using B2C custom attribute
    });
});

builder.Services.AddSingleton<ISessionStore, InMemorySessionStore>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable CORS
app.UseCors("ReactAppPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/public", () => "Welcome to the Todo API It is"+DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " UTC. This is a public endpoint accessible without authentication.");
app.MapGet("/hello", (HttpContext context) => {
    var name = context.User.Identity?.Name ?? "Unknown";
    return $"My Name is {name} and I am authenticated. This is a protected endpoint that requires authentication.";
})
.RequireAuthorization("RequireReadScope");

app.MapGet("/admin", (HttpContext context) => {
    var name = context.User.Identity?.Name ?? "Unknown";
    var role = context.User.FindFirst("extension_UserRole")?.Value ?? "Unknown";
    return $"Hello Admin {name}! Your role is: {role}. This is a protected admin endpoint.";
})
.RequireAuthorization("RequireAdmin");

app.Run();