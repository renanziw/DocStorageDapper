using BCryptNet = BCrypt.Net.BCrypt;
using DocStorage.Api.Configuration;
using DocStorage.Domain;
using DocStorage.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using DocStorage.Service.Authorization;
using Microsoft.OpenApi.Models;

namespace DocStorage.Api
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
            services.AddControllers();
            services.AddPostgres(Configuration);
            services.Configure<TokenAuthOption>(options =>
            {
                options.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Security:secret_key"])), SecurityAlgorithms.HmacSha256Signature);
            });

            services.AddAuthConfiguration();
            services.AddSwaggerGen(setup => {
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                setup.OperationFilter<SwaggerFileOperationFilter>();
                setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
            });

            services.AddServices();
            services.AddAutoMapper(cfg => cfg.CreateMapper());
            services.AddMvcCore()
                .AddAuthorization(options =>
                {
                    options.AddPolicy("Bearer", policy => {
                        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​);
                        policy.RequireClaim("external", "true");
                        policy.RequireAuthenticatedUser().Build();
                    });
                }).AddJsonOptions(x =>
                {
                    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "DocStorage API");
            });

            app.UseMiddleware<JwtMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.StartDatabase();
        }
    }
}
