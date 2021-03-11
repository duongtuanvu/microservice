using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Filters;
using System.Reflection;
using MediatR;
using FluentValidation;
using UserService.Behaviour;

namespace UserService
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
            // Add AddDbContext
            services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("DbTest"));

            // AddControllers 
            services.AddControllers(opts =>
            {
                // ActionFilter
                opts.Filters.Add<AsyncActionFilter>(); // Run after valid ok
            })
                // Add validator
                .AddFluentValidation(opts => { opts.RegisterValidatorsFromAssemblyContaining<Startup>(); }); // Valid
            services.AddValidatorsFromAssembly(typeof(Startup).Assembly);

            // Add MediatR
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // Add transactionBehaviour
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));

            // Add Apiversioning
            services.AddApiVersioning(config => { config.ReportApiVersions = true; });
            services.AddVersionedApiExplorer(opts =>
            {
                opts.GroupNameFormat = "'v'VVV";
                opts.SubstituteApiVersionInUrl = true;
            });

            // Add swagger
            services.AddSwaggerGen(
                opts =>
                {
                    var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
                    {
                        opts.SwaggerDoc(description.GroupName, CreateMetaInfoAPIVersion(description));
                    }
                    // add a custom operation filter which sets default values
                    //options.OperationFilter<SwaggerDefaultValues>();

                    // integrate xml comments
                    //options.IncludeXmlComments(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), XmlCommentsFileName));

                    // Thông tin trong Authorize
                    opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please insert JWT with Bearer into field",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    });

                    opts.AddSecurityRequirement(new OpenApiSecurityRequirement {
                        {
                            new OpenApiSecurityScheme{
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] { }
                        }
                    });
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(opts =>
            {
                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    opts.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"API {description.GroupName.ToUpperInvariant()}");
                }
            });
        }

        private OpenApiInfo CreateMetaInfoAPIVersion(ApiVersionDescription apiDescription)
        {
            return new OpenApiInfo
            {
                Title = "MyTestService",
                Version = apiDescription.ApiVersion.ToString(),
                Description = " This service is TEST sample service which provides ability to get weather control data ",
            };

        }
    }
}
