using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.ProjectModel;
using PayPal.Business;
using PayPal.Business.DAL;

namespace PayPal.WebApi
{
    public class Startup
    {
        public IHostingEnvironment HostingEnvironment { get; private set; }

        public IConfiguration Configuration { get; private set; }
        

        public Startup(IHostingEnvironment env, IHostingEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }
        
      
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddDbContext<ProjectContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("PayPal")));

            services.AddScoped<IProjectRepository, ProjectRepository>();

            services.AddMvc();
            services.AddOptions();
            //services.AddAuthentication();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();
            app.UseHttpStatusCodeExceptionMiddleware(); //Extention
                                                        
            app.UseStaticFiles();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            
            app.UseStaticFiles();
            //app.UseAuthentication();

            app.UseCors("MyPolicy");
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name:  "default",
                    template: "api/{controller=Project}");
            });

            var options = new RewriteOptions().AddRedirectToHttps();

            app.UseRewriter(options);

            app.Run(async (context) =>
            {
                context.Response.StatusCode = 404;
                throw new HttpStatusCodeException(404,"MVC didn't find anything");
            });
        }
    }
}
