using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphiQl;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using GraphQLBlogPost.Models.Type;
using GraphQLBlogPost.Repository;
using GraphQLBlogPost.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GraphQLBlogPost
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
            services.AddScoped<IDependencyResolver>(_ => new
                    FuncDependencyResolver(_.GetRequiredService));
            services.AddScoped<IDocumentExecuter, DocumentExecuter>();
            services.AddScoped<IDocumentWriter, DocumentWriter>();
            services.AddScoped<AuthorService>();
            services.AddScoped<AuthorRepository>();
            services.AddScoped<AuthorQuery>();
            services.AddScoped<AuthorType>();
            services.AddScoped<BlogPostType>();
            services.AddScoped<ISchema, GraphQLDemoSchema>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseGraphiQl("/graphql");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
