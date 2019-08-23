using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace ProjectNameApi
{
    public class Startup
    {
        private const string connectionName = "Connection";
        private readonly ILogger _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) 
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("domain.com", "http://localhost:4200");
                    });

                // Just in case it needs a different policy to update the data
                options.AddPolicy("Policy2",
                    builder =>
                    {
                        builder
                            .WithOrigins("domain2.com", "http://localhost:4200")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            // DI
            services.Services();
            services.Repositories();
            services.Databases(Configuration.GetConnectionString(connectionName));
            services.Configure<ConfigKeys>(Configuration.GetSection("ConfigKeys"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Api test",
                    Description = "Api to test",
                    TermsOfService = "",
                    Contact = new Contact
                    {
                        Name = "Test",
                        Email = "test@test.com",
                        Url = ""
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test v1");
            });

            if (env.IsDevelopment())
            {
                _logger.LogInformation("In Development environment");
                //app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseHsts(); // Available for 30 days
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors();
            app.UseMvc();          
        }
    }
}
