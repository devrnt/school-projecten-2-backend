﻿using g16_dotnet.Data;
using g16_dotnet.Data.Repositories;
using g16_dotnet.Filters;
using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            // add application services.
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
            services.AddSession();
            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Sessie}/{action=Index}/{id?}");
            });
            //SessieDataInitializer.InitializeData(context);
            BreakoutBoxDataInitializer.InitializeData(context);
        }
    }
}
