namespace Volunteers.API
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using DB;
    using Entities;
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    using Services;
    using Services.Mapper;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Validator;

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
                });
            services.Scan(scan =>
                scan.FromAssemblyOf<BaseService<BaseEntity, BaseDto>>()
                    .AddClasses(x => x.Where(t => t.Name.EndsWith("Service")))
                    .AsSelf()
                    .WithTransientLifetime());
            services.AddMvc()
                .AddNewtonsoftJson(
                    options => { options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; });
            ConfigureDbConnection(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API",
                    Description = "Volunteers API",
                });
                c.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

            var tokenKey = Configuration.GetValue<string>("TokenKey");
            var key = Encoding.ASCII.GetBytes(tokenKey);

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddSingleton<IVolunteerMapper, VolunteerMapper>();

            services.AddScoped<IDtoValidator, DtoValidator>();
            services.AddTransient<IValidator<OrganizationDto>, OrganizationValidator>();
            services.AddTransient<IValidator<RequestCreateDto>, RequestValidator>();
            services.AddTransient<IValidator<ActivityTypeDto>, ActivityTypeValidator>();
            services.AddTransient<IValidator<RegistrationDto>, RegistrationValidator>();

            services.AddScoped<IDbRepository<Organization>, DbRepository<Organization>>();
            services.AddScoped<IDbRepository<Request>, DbRepository<Request>>();
            services.AddScoped<IDbRepository<ActivityType>, DbRepository<ActivityType>>();
            services.AddScoped<IDbRepository<RegistrationToken>, DbRepository<RegistrationToken>>();
            services.AddScoped<IDbRepository<User>, DbRepository<User>>();
            services.AddScoped<IDbRepository<Role>, DbRepository<Role>>();
        }

        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="app">app</param>
        /// <param name="env">env</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Volunteers API"); });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseMiddleware<ErrorCodesMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            ILogger logger = loggerFactory.CreateLogger<Startup>();
            app.Run(async (context) =>
            {
                await Task.Run(() => logger.LogInformation("Requested Path: {0}", context.Request.Path));
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
            var dbName = Environment.GetEnvironmentVariable(EnvironmentVariable.DbName);

            return $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPass}";
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
            /// DbName
            /// </summary>
            public const string DbName = "dbname";

            /// <summary>
            /// UseSsl
            /// </summary>
            public const string UseSsl = "usessl";
        }
    }
}