using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SeliseExam.Data;
using SeliseExam.Repository;
using SeliseExam.Model;
using Microsoft.Extensions.Logging;
using SeliseExam.GlobalErrorHandle;
using SeliseExam.JWTToken;

namespace SeliseExam
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
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Selise")));

            _ = services.AddIdentity<AppUser, IdentityRole>(o => { }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
            services.AddControllers().AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                        .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CreatePolicy", policy => policy.RequireRole(Role.User));

                //options.AddPolicy("CreateRolePolicy",
                //    policy => policy.RequireClaim("Create Role"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
