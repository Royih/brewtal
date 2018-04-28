using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Brewtal.BLL;
using Brewtal.Database;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.IO;
using AutoMapper;
using Brewtal.CQRS;

namespace Brewtal
{
    public class Startup
    {
        private bool IsDevelopment { get; set; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            IsDevelopment = env.IsDevelopment();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BrewtalContext>();
            if (IsDevelopment)
            {
                services.AddSingleton(typeof(ITempReader), typeof(FakeTempReader));
                services.AddSingleton(typeof(IGPIO), typeof(FakeGPIO));
            }
            else
            {
                services.AddSingleton(typeof(ITempReader), typeof(TempReader));
                services.AddSingleton(typeof(IGPIO), typeof(GPIO));
            }
            services.AddSingleton<PidWorker>();

            services.AddScoped(typeof(IAggregateRootFactory), typeof(AggregateRootFactory));

            services.AddSignalR();

            services.AddMvc();

            services.AddMediatR(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                IsDevelopment = true;
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
                builder.AllowCredentials();
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<BrewtalHub>("brewtal");
            });

            Mapper.Initialize(cfg =>
           {
               cfg.AddProfile<AutoMapperProfileConfiguration>();
           });


            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseMvc();

            //handle client side routes
            app.Run(async (context) =>
           {
               context.Response.ContentType = "text/html";
               await context.Response.SendFileAsync(Path.Combine(env.WebRootPath, "index.html"));
           });

            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<BrewtalContext>();
                db.Database.Migrate();
                db.Seed();
            }

            var pidWorker = serviceProvider.GetRequiredService<PidWorker>();
            pidWorker.Start();






        }
    }
}
