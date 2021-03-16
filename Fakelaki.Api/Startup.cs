using System;
using System.Text;
using System.Threading.Tasks;
using Fakelaki.Api.Helpers;
using Fakelaki.Api.Lib.DAL;
using Fakelaki.Api.Lib.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Fakelaki.Api.Lib.Services.Implementation;
using System.Collections.Generic;
using Fakelaki.Api.Lib.Helpers;
using Fakelaki.Api.JCCPayments.Helpers;
using Fakelaki.Api.JCCPayments.Services.Implimentation;
using Fakelaki.Api.JCCPayments.Services.Interfaces;

namespace Fakelaki.Api
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
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            services.AddCors();

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            //Add JWT JWT based authentication in our project
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = userService.GetById(userId);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Add controllers
            services.AddControllers();

            // Add Auto Mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Register Context
            //services.AddDbContext<FakelakiContext>(p => p.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))); //when ready to deploy
            services.AddDbContext<FakelakiContext>(options => options.UseInMemoryDatabase("FakelakiDatabase"));

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IFakelakiService, FakelakiService>();
            services.AddTransient<IMailService, MailService>();
            services.AddSingleton<IQRCoderService, QRCoderService>();

            // Add mail settings conguration IoC container
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

            // Add JCC settings conguration IoC container
            services.Configure<JccSettings>(Configuration.GetSection("JccSettings"));
            services.AddSingleton<IJccGateway, JccGateway>();

            // Add Stripe settings conguration IoC container
            services.Configure<StripeSettings>(Configuration.GetSection("StripeSettings"));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c => {
                c.DescribeAllEnumsAsStrings();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, FakelakiContext ctx)
        {

            new FakelakiGenerator(ctx).FakelakiDataSeed();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());


            //Add JWT JWT based authentication in our project
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
