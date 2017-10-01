﻿using System.Collections.Concurrent;
using WorldDomination.DelegatedAuthentication.Auth0;
using WorldDomination.DelegatedAuthentication.WebApi.Models;
using WorldDomination.DelegatedAuthentication.WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WorldDomination.DelegatedAuthentication.WebApi
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
            services.AddMvcCore()
                    .AddAuthorization()
                    .AddJsonFormatters()
                    .AddCors();

            var applicationSettings = Configuration.GetSection("Settings").Get<ApplicationSettings>();
            services.AddSingleton(applicationSettings);

            var accountsRepository = new ConcurrentDictionary<string, Account>();
            services.AddSingleton(accountsRepository);
            services.AddSingleton<IAccountService, AccountService>();

            var authenticationService = new AuthenticationService<Auth0Jwt, CustomJwt>(applicationSettings.Auth0Secret,
                                                                                       applicationSettings.CustomSecret);
            services.AddSingleton<IAuthenticationService<Auth0Jwt, CustomJwt>>(authenticationService);

            services.AddCustomJwtAuthentication(applicationSettings.CustomAudience,
                                                applicationSettings.CustomAuthority,
                                                applicationSettings.CustomSecret);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.WithOrigins("http://localhost:49497")
                                          .AllowAnyHeader()
                                          .AllowAnyMethod());

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}