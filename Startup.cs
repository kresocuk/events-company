using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace TestWebApp
{
    public class Startup
    {
        public IConfiguration Configuration {get;}
        public Startup(IConfiguration configuration){
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<Models.PI03Context>( options => {
                string connString = Configuration.GetConnectionString("Firma");
                string password = Configuration["FirmaSqlPassword"];
                connString = connString.Replace("sifra", password);
                options.UseSqlServer(connString);
            } );

            var appSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSection);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("Osobe i certifikati",
                    "{controller:regex(^(Osoba|Posao)$)}/Page{page:int}/Sort{sort:int}/ASC-{ascending:bool}",
                    new {action = "Index"}
                    );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "AutoCompleteRoute",
                    pattern: "autocomplete/{controller}/{action=Get}",
                    new { area = "AutoComplete" }
                );
            });
        }
    }
}
