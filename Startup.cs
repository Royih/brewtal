using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Brewtal.BLL;

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

            services.AddSignalR();


            services.AddMvc();
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
                routes.MapHub<TempHub>("temp");
            });

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseMvc();

            var pidWorker = serviceProvider.GetRequiredService<PidWorker>();
            pidWorker.Start();

        }
    }
}
