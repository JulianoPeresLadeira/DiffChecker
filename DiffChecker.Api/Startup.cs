using DiffChecker.Api.Middleware;
using DiffChecker.Api.Middleware.Interfaces;
using DiffChecker.Api.Services;
using DiffChecker.Api.Services.Interfaces;
using DiffChecker.DataAccess.Service;
using DiffChecker.Domain.Model;
using DiffChecker.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace DiffChecker.Api
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
                c.IncludeXmlComments(GetXmlCommentsFile());
            });

            services.AddSingleton<IRepository, MongoRepository>();
            services.AddSingleton<IDecodeService, DecodeService>();
            services.AddScoped<IDiffCheckerService, DiffCheckerService>();
            services.AddTransient<IExceptionHandlerMiddleware, ExceptionHandlerMiddleware>();

            services.Configure<MongoDbConfiguration>(
                Configuration.GetSection(nameof(MongoDbConfiguration)));

            services.AddSingleton<IMongoDbConfiguration>(sp =>
                sp.GetRequiredService<IOptions<MongoDbConfiguration>>().Value);
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

        private string GetXmlCommentsFile()
        {
            return Configuration.GetSection("SwaggerConfiguration").GetValue<string>("XmlCommentsFile");
        }
    }
}