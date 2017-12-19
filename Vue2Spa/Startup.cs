using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.Swagger;

namespace Vue2Spa
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

            });

            //// use in memory for testing.
            //services
            //  .AddDbContext<HttpFileContext>(options => {
            //        //options.UseMySql("Server=localhost;database=unitofwork;uid=root;pwd=p@ssword;")
            //        options.UseInMemoryDatabase("UnitOfWork");
            //        //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            //    })
            //  .AddUnitOfWork<HttpFileContext>();
            //// Adds a default in-memory implementation of IDistributedCache.
            ////services.AddDistributedMemoryCache();

            // Add framework services.
            services.AddMvc();

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = ".HFMUserSession";
            });

            services.AddApiVersioning(o => {
                //GET api/helloworld HTTP/1.1
                //host: localhost
                //accept: text / plain; v = 1.0
                o.ApiVersionReader = new MediaTypeApiVersionReader();
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ApiVersionSelector = new CurrentImplementationApiVersionSelector(o);
                //o.DefaultApiVersion = new ApiVersion(new DateTime(2017, 12, 1), 1, 0, "1.0");
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info() { Title = "My API", Version = "v1" });

                c.OperationFilter<ExamplesOperationFilter>(); // [SwaggerRequestExample] & [SwaggerResponseExample]
                c.OperationFilter<DescriptionOperationFilter>(); // [Description] on Response properties
                c.OperationFilter<AuthorizationInputOperationFilter>(); // Adds an Authorization input box to every endpoint
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    HotModuleReplacementEndpoint = "/dist/__webpack_hmr"
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSession();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
