using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABU.GitHubCommunicationService.ApplicationCore.MappingProfiles;
using ABU.GitHubCommunicationService.Core.Config;
using ABU.GitHubCommunicationService.Core.Constants;
using ABU.GitHubCommunicationService.Infrastructure.Abstractions;
using ABU.GitHubCommunicationService.Infrastructure.Services;
using ABU.GitHubCommunicationService.Infrastructure.Workers;
using GitHubApiClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDatabaseAdapter;
using RestSharp;

var builder = WebApplication.CreateBuilder();
var services = builder.Services;

services.AddControllers();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ABU.GitHubCommunicationService", 
        Version = "v1"
    });
});

// TODO: refactor into something more elegant
var isDev = false;
if (bool.TryParse(builder.Configuration["DevEnv"], out var val))
    isDev = val;

var mongoUri = builder.Configuration["MongoUri"];
var token = builder.Configuration["AuthToken"];
var username = builder.Configuration["Username"];

services.AddMongoDb(options =>
{
    if (isDev)
        options.AddConnectionString(mongoUri);
    else
        options.AddConnectionString(GetValueFromCommandLine(
            CommandLineArgKeyValues.MongoConnectionString)!);
});

services.AddGitHubApiClient(options =>
{
    if (isDev)
    {
        options.AddToken(token);
        options.AddUsername(username);
    }
    else
    {
        options.AddToken(GetValueFromCommandLine(CommandLineArgKeyValues.GitHubToken)!);
        options.AddUsername(GetValueFromCommandLine(CommandLineArgKeyValues.GitHubUsername)!);
    }
});

services.AddAutoMapper(cfg => { cfg.AddMaps(typeof(GitHubProfile)); });

services.AddLogging();
services.AddOptions();
services.Configure<GitHubOptions>(builder.Configuration.GetSection(nameof(GitHubOptions)));
services.AddTransient<IRestClient, RestClient>();
services.AddTransient<IGitHubApiService, GitHubApiService>();
services.AddHostedService<GitHubRepositoryBackgroundService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ABU.GitHubCommunicationService v1"));
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

string? GetValueFromCommandLine(string key)
{
    var value = args.FirstOrDefault(s => s.Contains(key));
    
    if (string.IsNullOrWhiteSpace(value))
        return null;

    return value.Replace(key, string.Empty);
}

app.Run();