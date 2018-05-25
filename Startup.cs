using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarSale.Core;
using CarSale.Core.Models;
using CarSale.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarSale {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.Configure<PhotoSettings> (Configuration.GetSection ("PhotoSettings"));
            services.AddDbContext<CarDbContext> (opt => {
                opt.UseSqlServer (Configuration.GetConnectionString ("Default"));
            });
            services.AddMvc ();
            services.AddAutoMapper ();
            services.AddScoped<IVehicleRepository, VehicleRepository> ();
            services.AddScoped<IPhotoRepository, PhotoRepository> ();
            services.AddScoped<IUnitOfWork, UnitOfWork> ();

            // 1. Add Authentication Services
            services.AddAuthentication (options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer (options => {
                options.Authority = "https://carsaleproject.auth0.com/";
                options.Audience = "https://api.carsale.com";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
                app.UseWebpackDevMiddleware (new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = true
                });
            } else {
                app.UseExceptionHandler ("/Home/Error");
            }

            app.UseStaticFiles ();
            // 2. Enable authentication middleware
            app.UseAuthentication ();
            app.UseMvc (routes => {
                routes.MapRoute (
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute (
                    name: "spa-fallback",
                    defaults : new { controller = "Home", action = "Index" });
            });
        }
    }
}