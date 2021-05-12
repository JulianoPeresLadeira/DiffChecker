using DiffChecker.Middleware;
using DiffChecker.Middleware.Interfaces;
using DiffChecker.Services;
using DiffChecker.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DiffChecker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.IgnoreNullValues = true);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Diff Checker",
                    Version = "v1",
                    Description = "Compares two different base64 encoded strings, returning diff points and the length of diff, of if they have different sizes.",
                    Contact = new OpenApiContact
                    {
                        Name = "Juliano Ladeira",
                        Email = "julianoladeira@gmail.com"
                    },
                });
                c.IncludeXmlComments(@"DiffChecker.xml");
            });

            services.AddSingleton<IRepository, TemporaryRepository>();
            services.AddSingleton<IDecodeService, DecodeService>();
            services.AddScoped<IDiffCheckerService, DiffCheckerService>();
            services.AddTransient<IExceptionHandlerMiddleware, ExceptionHandlerMiddleware>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Diff Checker");
                c.RoutePrefix = string.Empty;
            });

            app.UseMiddleware<IExceptionHandlerMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}