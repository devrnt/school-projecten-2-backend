using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using g16_dotnet.Data;
using g16_dotnet.Models;
using g16_dotnet.Services;
using System.Security.Claims;
using g16_dotnet.Models.Domain;
using g16_dotnet.Filters;
using g16_dotnet.Data.Repositories;

namespace g16_dotnet
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer("Server=.\\sqlexpress;Database=BreakoutBox;Trusted_Connection=True;MultipleActiveResultSets=true"));

            services.AddAuthorization(options => options.AddPolicy("Leerkracht", policy => policy.RequireClaim(ClaimTypes.Role, "Leerkracht")));
            

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            // Add application services.
            services.AddDbContext<ApplicationDbContext>();
            services.AddScoped<ISessieRepository, SessieRepository>();
            services.AddScoped<IPadRepository, PadRepository>();
            services.AddScoped<IOpdrachtRepository, OpdrachtRepository>();
            services.AddScoped<IActieRepository, ActieRepository>();
            services.AddScoped<IGroepRepository, GroepRepository>();
            services.AddScoped<ILeerkrachtRepository, LeerkrachtRepository>();
            services.AddScoped<PadFilter>();
            services.AddScoped<LeerkrachtFilter>();
            services.AddScoped<SessieFilter>();
            services.AddTransient<BreakoutBoxDataInitializer>();
            
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, BreakoutBoxDataInitializer breakoutBoxDataInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Sessie}/{action=Index}/{id?}");
            });

            breakoutBoxDataInitializer.InitializeData().Wait();
        }
    }
}
