using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.Swagger;
using Vue2Spa.Models;
using Vue2Spa.Models.AuthenticationEvents;
using Vue2Spa.Models.Identity;

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

            // use in memory for testing.
            services
              .AddDbContext<HttpFileContext>(options =>
              {
                  //var sqlConnectionString = Configuration.GetConnectionString("DataAccessMySqlProvider");
                  //options.UseMySql("Server=localhost;database=unitofwork;uid=root;pwd=p@ssword;");
                  //// postgre
                  //options.UseNpgsql(sqlConnectionString);
                  options.UseInMemoryDatabase("UnitOfWork");
                  //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));   //Configuration["Data:DefaultConnection:ConnectionString"]
                  //options.UseSqlite("data.sqlite.db");
              })
              .AddUnitOfWork<HttpFileContext>();
            // Adds a default in-memory implementation of IDistributedCache.
            //services.AddDistributedMemoryCache();

            services.AddIdentity<ApplicationUser, ApplicationRole>()
              .AddEntityFrameworkStores<HttpFileContext>()
              .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(150);
                options.LoginPath = "/Account/SSOSignOn"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                options.LogoutPath = "/Account/SignOut"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
                options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
                options.SlidingExpiration = true;
            });

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie();

            services
              .AddAuthentication(options => {
                  options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                  options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                  options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
              })
              .AddCookie(options => {
                  //options.EventsType = typeof(CustomCookieAuthenticationEvents);

                  options.LoginPath = "/Account/SSOSignOn";
                  options.LogoutPath = "/Account/SignOut";
                  options.AccessDeniedPath = "/Account/AccessDenied";
                  options.SlidingExpiration = true;
                  options.Cookie.HttpOnly = true;
                  options.Cookie.Expiration = TimeSpan.FromDays(15);
              });

            // Add framework services.
            services.AddMvc(options => {
                
            }).AddJsonOptions(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.MaxDepth = 10;
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            });

            #region Compression - https://docs.microsoft.com/en-us/aspnet/core/performance/response-compression?tabs=aspnetcore2x only selfhosting
            #region Default Case
            services.AddResponseCompression();
            #endregion
            #region Custom Case
            //services.AddResponseCompression(options =>
            //{
            //    options.Providers.Add<GzipCompressionProvider>();
            //    //options.Providers.Add<CustomCompressionProvider>();
            //    //options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" });
            //});

            //services.Configure<GzipCompressionProviderOptions>(options =>
            //{
            //    options.Level = CompressionLevel.Fastest;
            //});
            #endregion
            #endregion
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("EmployeeNumber"));
            //});

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
                c.SwaggerDoc("v1", new Info() { Title = "My API 1.0", Version = "1.0" });
                //c.SwaggerDoc("v2", new Info() { Title = "My API 2.0", Version = "2.0" });
                c.OperationFilter<ExamplesOperationFilter>(services.BuildServiceProvider());

                //c.OperationFilter<ExamplesOperationFilter>(); // [SwaggerRequestExample] & [SwaggerResponseExample]
                //c.OperationFilter<DescriptionOperationFilter>(); // [Description] on Response properties
                //c.OperationFilter<AuthorizationInputOperationFilter>(); // Adds an Authorization input box to every endpoint
            });

            services.AddTransient<WeatherForecastExample>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();

                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    HotModuleReplacementEndpoint = "/dist/__webpack_hmr"
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseExceptionHandler(builder => {
                //    builder.Run(
                //      async context =>
                //      {
                //          context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //          context.Response.ContentType = "text/html";

                //          var error = context.Features.Get<IExceptionHandlerFeature>();
                //          if (error != null)
                //          {
                //              await context.Response.WriteAsync($"<h1>Error: {error.Error.Message}</h1>").ConfigureAwait(false);
                //          }
                //      });
                //});
            }

            app.UseSession();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseResponseCompression();

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API 1.0");
            });
            //app.UseSwaggerUI(c => {
            //    c.SwaggerEndpoint("/swagger/v2/swagger.json", "My API 2.0");
            //});

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
