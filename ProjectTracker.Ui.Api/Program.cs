using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ProjectTracker.Core.Models.VaultSecrets;
using ProjectTracker.Data.Contexts;
using ProjectTracker.Data.Models;
using ProjectTracker.Infrastructure.Api.DomainService;
using ProjectTracker.Infrastructure.DomainServices;


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
