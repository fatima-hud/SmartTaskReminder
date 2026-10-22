using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SmartTaskReminder.API.Extensions;
using SmartTaskReminder.Application.Interfaces;
using SmartTaskReminder.Core.Entities;
using SmartTaskReminder.Core.Helping;
using SmartTaskReminder.Infrastructure.Context;
using SmartTaskReminder.Infrastructure.Seed;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));      
builder.Services.AddControllers()
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(
        new JsonStringEnumConverter()
    );
});
builder.Host.UseSerilog((context, configuration) =>
configuration.ReadFrom.Configuration(context.Configuration));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCustomJwtAuth(builder.Configuration);
builder.Services.AddSwaggerGenJwtAuth();
builder.Services.AddApplicationServices();
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JWT"));
builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var userRepository =
        services.GetRequiredService<IUserRepository>();

    var roleRepository =
        services.GetRequiredService<IRoleRepository>();

    var passwordHasher =
        services.GetRequiredService<IPasswordHasherService>();

    var configuration =
        services.GetRequiredService<IConfiguration>();

    await AdminSeeder.SeedDataAsync(
        userRepository,
        roleRepository,
        passwordHasher,
        configuration);
}

app.UseSerilogRequestLogging();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard("/dashboard");
app.MapControllers();

app.Run();
