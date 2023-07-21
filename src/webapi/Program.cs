// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using eppeta.webapi.Identity.Data;
using eppeta.webapi.Identity.Models;
using eppeta.webapi.Service;
using eppeta.webapi.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Quartz;


namespace eppeta.webapi;

internal class Program
{
    const string ConnectionStringName = "DefaultConnection";
    const string TokenEndpoint = "connect/token";
    const string AllowedOriginsPolicy = "AllowedOriginsPolicy";

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        AppSettings.Initialize(builder.Configuration);

        // Add services to the container.
        builder.Services.AddControllers();
        ConfigureCorsService(builder.Services);
        ConfigureDatabaseConnection(builder);
        ConfigureLocalIdentityProvider(builder.Services);
        ConfigureQuartz(builder.Services);
        ConfigureSwaggerUIServices(builder.Services);
        ConfigureAspNetAuth(builder.Services);

        // Add authentication configuration service to the container.
        builder.Services.AddScoped<IAuthenticationConfigurationService>(
            provider => new AuthenticationConfigurationService("https://localhost:443/api/", "populated", "populatedSecret")
        );


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        ConfigureSwaggerUIApp(app);
        app.UseForwardedHeaders();
        app.UseRouting();
        app.UseCors(AllowedOriginsPolicy);
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapDefaultControllerRoute();
        });
        app.Run();

        static void ConfigureCorsService(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowedOriginsPolicy, policy =>
                {
                    policy.WithOrigins(AppSettings.AllowedOrigins)
                          .WithHeaders(HeaderNames.ContentType, "Content-Type");
                });
            });
        }

        static void ConfigureDatabaseConnection(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString(ConnectionStringName) ?? throw new InvalidOperationException($"Connection string '{ConnectionStringName}' not found.");

            builder.Services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
                options.UseOpenIddict();
            });

            builder.Services.AddScoped<IIdentityRepository, IdentityDbContext>();
        }

        static void ConfigureLocalIdentityProvider(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                        .UseDbContext<IdentityDbContext>();
                    options.UseQuartz();
                })
                .AddServer(options =>
                {
                    options.SetTokenEndpointUris(TokenEndpoint);

                    // These two go hand-in-hand: allowing anonymous client means you can
                    // send password request without _also_ providing a client_id.
                    options.AllowPasswordFlow();
                    options.AcceptAnonymousClients();

                    options.AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();
                    var aspNetCoreBuilder = options.UseAspNetCore()
                        .EnableTokenEndpointPassthrough();

                    if (!AppSettings.Authentication.RequireHttps)
                    {
                        aspNetCoreBuilder.DisableTransportSecurityRequirement();
                    }

                    options.AddSigningKey(AppSettings.Authentication.SigningKey ?? throw new InvalidOperationException("Unable to initialize Identity because there is no Signing Key configured in appsettings"));
                    options.SetIssuer(AppSettings.Authentication.IssuerUrl ?? throw new InvalidOperationException("Unable to initialize Identity because there is no Issuer URL configured in appsettings"));
                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
                    options.Configure(options => options.TokenValidationParameters.IssuerSigningKey = AppSettings.Authentication.SigningKey);
                });
        }

        static void ConfigureAspNetAuth(IServiceCollection services)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.Authority = AppSettings.Authentication.Authority;
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = AppSettings.Authentication.Audience,
                    ValidIssuer = AppSettings.Authentication.IssuerUrl!.ToString(),
                    IssuerSigningKey = AppSettings.Authentication.SigningKey
                };
                opt.RequireHttpsMetadata = AppSettings.Authentication.RequireHttps;
            });
            services.AddAuthorization();
        }

        static void ConfigureQuartz(IServiceCollection services)
        {
            // Quartz will be used for scheduled removal of old authorization tokens
            services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();
                options.UseSimpleTypeLoader();
                options.UseInMemoryStore();
            });
            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
        }

        static void ConfigureSwaggerUIServices(IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(x => x.FullName?.Replace("+", "."));
                options.OperationFilter<TokenEndpointBodyDescriptionFilter>();
                options.OperationFilter<TagByResourceUrlFilter>();
                options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
                        },
                        new string[] {}
                    }
                });

                options.DocumentFilter<ExplicitSchemaDocumentFilter>();
                options.DocumentFilter<SwaggerIgnoreDocumentFilter>();
                options.SchemaFilter<SwaggerOptionalSchemaFilter>();
                options.SchemaFilter<SwaggerExcludeSchemaFilter>();
                options.EnableAnnotations();
                options.OrderActionsBy(x =>
                {
                    return x.HttpMethod != null && Enum.TryParse<HttpVerbOrder>(x.HttpMethod, out var verb)
                        ? ((int)verb).ToString()
                        : int.MaxValue.ToString();
                });
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "EPP Evaluation Tracker", Version = "v1" });
            });
        }

        static void ConfigureSwaggerUIApp(WebApplication app)
        {
            if (app.Configuration.GetValue<bool>("EnableSwagger"))
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.EnableTryItOutByDefault();
                    options.DocumentTitle = "EPP Evaluation Tracker API Documentation";
                });
            }
        }
    }
}
