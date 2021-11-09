using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using GitHubApiClient;
using GitHubCommunicationService.Abstractions;
using GitHubCommunicationService.Config;
using GitHubCommunicationService.Constants;
using GitHubCommunicationService.MappingProfiles;
using GitHubCommunicationService.Services;
using GitHubCommunicationService.Workers;
using RestSharp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;
using MongoDatabaseAdapter;

namespace GitHubCommunicationService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GitHubCommunicationService", Version = "v1" });
            });

            // TODO: refactor into something more elegant
            var isDev = false;
            if (bool.TryParse(Configuration["Development"], out var val))
                isDev = val;
            
            var mongoUri = Configuration["MongoUri"];
            var token = Configuration["AuthToken"];
            var username = Configuration["Username"];
            
            services.AddMongoDb(options =>
            {
                if (isDev)
                {
                    options.AddConnectionString(mongoUri);
                }
                else
                {
                    options.AddConnectionString(GetValueFromCommandLine(
                        CommandLineArgKeyValues.MongoConnectionString)!);
                }
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

            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(GitHubProfile));
            });
            
            services.AddLogging();
            services.AddOptions();
            services.Configure<GitHubOptions>(Configuration.GetSection(nameof(GitHubOptions)));
            services.AddTransient<IRestClient, RestClient>();
            services.AddTransient<IGitHubApiService, GitHubApiService>();
            services.AddHostedService<GitHubRepositoryBackgroundService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GitHubCommunicationService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static string? GetValueFromCommandLine(string key)
        {
            var commandLineArgs = Program.Args;
            
            var value = commandLineArgs?.FirstOrDefault(s => s.Contains(key));

            if (string.IsNullOrWhiteSpace(value))
                return null;
            
            return value.Replace(key, string.Empty);
        }

        private string? GetOptionStringValue()
        {
            throw new NotImplementedException();
        }
    }
}
