using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ProjectTracker.Core.DomainServices;
using ProjectTracker.Core.Interfaces.DomainServices;
using ProjectTracker.Core.Interfaces.InfrastructureServices;
using ProjectTracker.Infrastructure.Data.Contexts;
using ProjectTracker.Infrastructure.Entities;
using ProjectTracker.Infrastructure.InfrastructureServices;
using ProjectTracker.Infrastructure.Interfaces.InfrastructureServices;
using ProjectTracker.Infrastructure.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IConfigurationRetriever, ApiConfigurationRetriever>();
builder.Services.AddTransient<IVault, Vault>();
builder.Services.AddTransient<IVaultClientFactory, VaultClientFactory>();
builder.Services.AddTransient<IVaultPathRetriever, VaultPathRetriever>();

builder.Services.AddDefaultIdentity<ProjectTrackerUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<UsersContext>();

builder.Services.RegisterContext<UsersContext>(secret => secret!.UserDb!);

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

app.UseAuthorization();

app.MapControllers();

app.Run();
