using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CS348Blog.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CSC348Blog.Data.Repository;
using CSC348Blog.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CS348Blog
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddTransient<IRepo, Repo>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Create Post", policy => policy.RequireClaim("Create Post", "allowed"));
                options.AddPolicy("Edit Post", policy => policy.RequireClaim("Edit Post", "allowed"));
                options.AddPolicy("Delete Post", policy => policy.RequireClaim("Delete Post", "allowed"));
                options.AddPolicy("View Post", policy => policy.RequireClaim("View Post", "allowed"));
                options.AddPolicy("View Post List", policy => policy.RequireClaim("View Post List", "allowed"));
                options.AddPolicy("Create Comment", policy => policy.RequireClaim("Create Comment", "allowed"));
                options.AddPolicy("Edit Comment", policy => policy.RequireClaim("Edit Comment", "allowed"));
                options.AddPolicy("Delete Comment", policy => policy.RequireClaim("Delete Comment", "allowed"));
                options.AddPolicy("View Comment", policy => policy.RequireClaim("View Comment", "allowed"));
                options.AddPolicy("Like", policy => policy.RequireClaim("Like", "allowed"));
                options.AddPolicy("Dislike", policy => policy.RequireClaim("Dislike", "allowed"));
                options.AddPolicy("Permission Panel", policy => policy.RequireClaim("Permission Panel", "allowed"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            DbSeeder.SeedDb(userManager, roleManager);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Post}/{action=PostList}/{id?}");
            });
        }
    }
}
