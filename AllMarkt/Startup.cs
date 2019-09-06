using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AllMarkt.Data;
using AllMarkt.Services;
using AllMarkt.Services.Interfaces;
using AllMarkt.Tools;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AllMarkt
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
            services.AddCors();
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<AllMarktContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("AllMarktDatabase"))
            );

            services.AddScoped<AllMarktQueryContext>();
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IPrivateMessagesService, PrivateMessagesService>();
            services.AddTransient<IProductCategoriesService, ProductCategoriesService>();
            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<IShopCommentsService, ShopCommentsService>();
            services.AddTransient<IOrdersService, OrdersService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IProductCommentsService, ProductCommentsService>();
            services.AddTransient<IAllMarktQueryContext, AllMarktQueryContext>();
            services.AddTransient<ITokenGenerator, TokenGenerator>();
            services.AddTransient<IClaimsGetter, ClaimsGetter>();
            services.AddTransient<IShopCategoryService,ShopCategoryService>();

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(
                    new WebpackDevMiddlewareOptions
                    {
                        HotModuleReplacement = true
                    }
                );
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseAuthentication();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}