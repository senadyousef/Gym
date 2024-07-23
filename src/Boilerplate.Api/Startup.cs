using System;
using System.Text;
using System.Text.Json.Serialization;
using Boilerplate.Api.Extensions;
using Boilerplate.Application.DTOs;
using Boilerplate.Application.Interfaces;
using Boilerplate.Application.Services;
using Boilerplate.Domain.Repositories;
using Boilerplate.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Boilerplate.Application.Auth;
using Microsoft.AspNetCore.Http;
using Rotativa.AspNetCore;
using static Boilerplate.Application.Services.UploadService;
using ISession = Boilerplate.Domain.Auth.Interfaces.ISession;
using Boilerplate.Application.AllServices;
using Boilerplate.Application.UserAllServices;
using Boilerplate.Application.Items;
using Boilerplate.Application.Gallery;
using Boilerplate.Application.Files;

namespace Boilerplate.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            // Add services to the container.
            services.AddControllersWithViews();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(50);//You can set Time   
            });
            
            //Extension method for less clutter in startup
            services.AddApplicationDbContext(Configuration);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<ICurrentUser, CurrentCustomer>();
            
            //DI Services and Repos 
            services.AddScoped<IUserRepository, UserRepository>();
           
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISession, Session>();
              
            //Events
            services.AddScoped<IEventsRepository, EventsRepository>();
            services.AddScoped<IEventsService, EventsService>();
            
            //User Events
            services.AddScoped<IUserEventsRepository, UserEventsRepository>();
            services.AddScoped<IUserEventsService, UserEventsService>();
             
            //Personal Trainers Classes
            services.AddScoped<IPersonalTrainersClassesRepository, PersonalTrainersClassesRepository>();
            services.AddScoped<IPersonalTrainersClassesService, PersonalTrainersClassesService>();
              
            //News
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<INewsService, NewsService>();

            //AllServices
            services.AddScoped<IAllServicesRepository, AllServicesRepository>();
            services.AddScoped<IAllServicesService, AllServicesService>();


            //UserAllServices
            services.AddScoped<IUserAllServicesRepository, UserAllServicesRepository>();
            services.AddScoped<IUserAllServicesService, UserAllServicesService>();

            //Items
            services.AddScoped<IItemsRepository, ItemsRepository>();
            services.AddScoped<IItemsService, ItemsService>();

            //Gallery
            services.AddScoped<IGalleryRepository, GalleryRepository>();
            services.AddScoped<IGalleryService, GalleryService>();

            //Files
            services.AddScoped<IFilesRepository, FilesRepository>();
            services.AddScoped<IFilesService, FilesService>();
            services.AddTransient<IFilesService, FilesService>();
             
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            // notification service 
            services.AddScoped<IPushTokenRepository, PushTokenRepository>();
            services.AddScoped<IPushTokenService, PushTokenService>();

            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IPushTicketRepository, PushTicketRepository>();
            services.AddScoped<INotificationTicketRepository, NotificationTicketRepositroy>();

            // aws configuration 
            services.Configure<UploadOptions>(Configuration.GetSection("AwsConfiguration"));

            services.AddTransient<IUploadService, UploadService>();
             
            // WebApi Configuration
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // for enum as strings
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; 
            });


            var tokenConfig = Configuration.GetSection("TokenConfiguration");
            services.Configure<TokenConfiguration>(tokenConfig);

            // configure jwt authentication
            var appSettings = tokenConfig.Get<TokenConfiguration>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = Environment.IsProduction();
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = appSettings.Issuer,
                        ValidAudience = appSettings.Audience
                    };
                });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
           services.AddCors(p => p.AddPolicy("corsapp", builder =>
            {
                builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            }));

           services.AddControllersWithViews();
            
           services.AddSession(options => {
               options.IdleTimeout = TimeSpan.FromMinutes(50);//You can set Time   
           });
           
           
            //  razor pages
            services.AddRazorPages();
            // AutoMapper settings
            services.AddAutoMapperSetup();

            // HttpContext for log enrichment 
            services.AddHttpContextAccessor();

            // Swagger settings
            services.AddApiDoc();
            // GZip compression
            services.AddCompression();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseCustomSerilogRequestLogging();

            app.UseSession();


            app.UseRouting();


            app.UseCors("corsapp");

            app.UseApiDoc();
             
            app.UseAuthentication();
            app.UseAuthorization();

            //added request logging
            app.UseHttpsRedirection();

            app.UseResponseCompression();
            
            app.UseStaticFiles();
            
            RotativaConfiguration.Setup("wwwroot");
            
            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapControllers();
                // endpoints.MapControllerRoute(
                //     name: "default",
                //     pattern: "{controller=Home}/{action=Index}/{id?}");
                // endpoints.MapRazorPages();
                endpoints.MapDefaultControllerRoute();
            });
            
            
        }
    }
}
