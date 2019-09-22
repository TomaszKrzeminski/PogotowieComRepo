using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PogotowieCom.Models;
using PogotowieCom.Compontnts;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace PogotowieCom
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }




        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IRepository, Repository>();
            services.AddScoped<AppIdentityDbContext>();
            services.AddDbContext<AppIdentityDbContext>(options =>
                 options.UseSqlServer(
                     Configuration["Data:PogotowieCom:ConnectionString"]));


            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(opts => opts.LoginPath = "/User/Login");
            services.AddMvc();
            //services.AddMvcCore().AddJsonFormatters().AddAuthorization().AddDataAnnotations();

        }
            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IHostingEnvironment env, AppIdentityDbContext context)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes => {
                routes.MapRoute(name: "None", template: "{controller=Home}/{action=HomePage}/{id?}");


            });

            SeedAdmin.EnsurePopulated(context);
        }
    }
}
