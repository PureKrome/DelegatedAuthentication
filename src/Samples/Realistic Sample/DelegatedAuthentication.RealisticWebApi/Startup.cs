using DelegatedAuthentication.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WorldDomination.DelegatedAuthentication;
using WorldDomination.DelegatedAuthentication.Application;
using WorldDomination.DelegatedAuthentication.Application.Authentication;
using WorldDomination.DelegatedAuthentication.Auth0;
using WorldDomination.DelegatedAuthentication.Domain;

namespace WorldDomination.DelegatedAuthentication.RealisticWebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
            _environment = environment ?? throw new System.ArgumentNullException(nameof(environment));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Wire up our custom clean layers.
            var settings = services.AddApplication(_configuration);
            services.AddInfrastructure();


            //// Map the application settings to a strongly typed instance.
            //var applicationSettings = services.AddApplicationSettings(_configuration);
            //services.AddSingleton(applicationSettings); // Add these settings to the Container.

            // Wire up the normal stuff, like controllers, auth, etc.
            services.AddControllers();  // Endpoints are in controllers.

            services.AddAuthorization() // Endpoints are locked down.
                    .AddCors();         // Endpoints can be accessed from another domain.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(builder => builder.WithOrigins("http://localhost:49497")
                                          .AllowAnyHeader()
                                          .AllowAnyMethod());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
