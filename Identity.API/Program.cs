using Identity.API.Data;
using Identity.API.Repository;
using Identity.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext
builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));

    // Required by OpenIddict
    options.UseOpenIddict();
});

// Configure OpenIddict
builder.Services.AddOpenIddict()

    // Stores
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
               .UseDbContext<IdentityDbContext>();
    })

 .AddServer(options =>
 {
     options.SetTokenEndpointUris("/connect/token");

     options.AllowPasswordFlow();

     options.AcceptAnonymousClients();


     // Signing key (JWT signature)
     options.AddDevelopmentSigningCertificate();


     // Encryption key required by OpenIddict internally
     options.AddEphemeralEncryptionKey();


     // Return normal JWT access token
     options.DisableAccessTokenEncryption();


     options.UseAspNetCore()
            .EnableTokenEndpointPassthrough();
 });
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
// Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Authentication
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();