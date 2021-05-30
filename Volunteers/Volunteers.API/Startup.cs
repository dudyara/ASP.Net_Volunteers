namespace Volunteers.API
{
    using System;
    using DB;
    using Entities;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    using Services;
    using Services.Mapper;

    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="configuration">configuration</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="services">services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
              .AddFluentValidation(s =>
               {
                   s.RegisterValidatorsFromAssemblyContaining<Startup>();
                   s.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
               });
            services.Scan(scan =>
                scan.FromAssemblyOf<BaseService<IEntity>>()
                    .AddClasses(x => x.Where(t => t.Name.EndsWith("Service")))
                    .AsSelf()
                    .WithTransientLifetime());
            services.AddMvc()
                .AddNewtonsoftJson(
                    options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    });
            ConfigureDbConnection(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API",
                    Description = "Volunteers API",
                });
            });

            services.AddSingleton<IVolunteerMapper, VolunteerMapper>();
            services.AddTransient<IDbRepository<Organization>, DbRepository<Organization>>();
            services.AddTransient<IDbRepository<Request>, DbRepository<Request>>();
            services.AddTransient<IDbRepository<ActivityType>, DbRepository<ActivityType>>();
        }

        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="app">app</param>
        /// <param name="env">env</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Volunteers API");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureDbConnection(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(GetConnectionString()));
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AppDbContext>();
        }

        private string GetConnectionString()
        {
            var dbHost = Environment.GetEnvironmentVariable(EnvironmentVariable.DbHost);
            if (string.IsNullOrEmpty(dbHost))
                return Configuration.GetConnectionString("DefaultConnection");

            var dbUser = Environment.GetEnvironmentVariable(EnvironmentVariable.DbUser);
            var dbPass = Environment.GetEnvironmentVariable(EnvironmentVariable.DbPass);
            var dbPort = Environment.GetEnvironmentVariable(EnvironmentVariable.DbPort);

            return $"Host={dbHost};Port={dbPort};Database=VolunteerDb;Username={dbUser};Password={dbPass}";
        }

        /// <summary>
        /// EnvironmentVariable
        /// </summary>
        public class EnvironmentVariable
        {
            /// <summary>
            /// DbHost
            /// </summary>
            public const string DbHost = "dbhost";

            /// <summary>
            /// DbUser
            /// </summary>
            public const string DbUser = "dbuser";

            /// <summary>
            /// DbPass
            /// </summary>
            public const string DbPass = "dbpass";

            /// <summary>
            /// DbPort
            /// </summary>
            public const string DbPort = "dbport";

            /// <summary>
            /// UseSsl
            /// </summary>
            public const string UseSsl = "usessl";
        }
    }
}
