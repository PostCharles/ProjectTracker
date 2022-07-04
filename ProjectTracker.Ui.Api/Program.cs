using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ProjectTracker.Core.DomainServices;
using ProjectTracker.Core.Interfaces.DomainServices;
using ProjectTracker.Core.Interfaces.InfrastructureServices;
using ProjectTracker.Core.Models.VaultSecrets;
using ProjectTracker.Infrastructure.Api.Data.Contexts;
using ProjectTracker.Infrastructure.Api.Data.Entities;
using ProjectTracker.Infrastructure.InfrastructureServices;
using ProjectTracker.Infrastructure.Interfaces.InfrastructureServices;

var builder = WebApplication.CreateBuilder(args);

var configurationRetriever = new ApiConfigurationRetriever(builder.Configuration);
var vaultPathRetriever = new VaultPathRetriever();
var vaultFactory = new VaultClientFactory(configurationRetriever);
var vault = new Vault(configurationRetriever, vaultFactory, vaultPathRetriever);

var secret = vault.GetSecretAsync<ConnectionStringSecret>().Result;

builder.Services.AddDbContext<UsersContext>(options => options.UseNpgsql(secret!.UserDb!));

builder.Services.AddDefaultIdentity<ProjectTrackerUser>(options => options.SignIn.RequireConfirmedAccount = true)
       .AddEntityFrameworkStores<UsersContext>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IConfigurationRetriever, ApiConfigurationRetriever>();
builder.Services.AddTransient<IVault, Vault>();
builder.Services.AddTransient<IVaultClientFactory, VaultClientFactory>();
builder.Services.AddTransient<IVaultPathRetriever, VaultPathRetriever>();   

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
