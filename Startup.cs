global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Identity;
global using FluentValidation;
global using FluentValidation.AspNetCore;
global using Microsoft.IdentityModel.Tokens;
global using System.Text;
global using Microsoft.EntityFrameworkCore;
global using PogodynkaAPI.Entities;
global using PogodynkaAPI.Models;
global using PogodynkaAPI.Models.Validators;
global using PogodynkaAPI.Services;
global using PogodynkaAPI.Exceptions;
global using PogodynkaAPI.Middleware;

namespace PogodynkaAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationSettings = new AuthenticationSettings();
            Configuration.GetSection("Authentication").Bind(authenticationSettings); //powiązanie z sekcją w "appsettings.json"
            services.AddSingleton(authenticationSettings); //rejestrujemy jako Singleton, żeby wstrzyknąć do AccountService.cs

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters //using Microsoft.IdentityModel.Tokens;
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)) //using Microsoft.IdentityModel.Tokens;
                };
            });
            services.AddControllers();
            services.AddFluentValidation(); //dodanie FluentValidatora (korzysta z using FluentValidation.AspNetCore;)
            services.AddDbContext<PogodynkaDbContext>();
            services.AddScoped<PogodynkaDbUtils>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>(); //dodanie validatora z NuGeta - FluentValidation
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPogodynkaService, PogodynkaService>();
            services.AddHttpContextAccessor();
            services.AddCors(options =>
            {
                options.AddPolicy("FrontEndClient", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
            });
        }

        //This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PogodynkaDbUtils dbUtils)
        {
            dbUtils.AutoMigrations();
            app.UseResponseCaching(); //caching zapytań z dysku lokalnego żeby odciążyć api np. [ResponseCache(Duration = 1200, VaryByQueryKeys = new[]{"fileName"})]
            app.UseStaticFiles(); //z paczki NuGetowej Microsoft.AspNetCore.StaticFiles - folder wwwroot np. [GET]https://localhost:7072/sample-file.txt
            app.UseCors("FrontEndClient"); //dodanie corsa z metody wyżej
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseAuthentication(); //musi być przed UseHtppsRedirection(); (to od autentykacji użytkowników, którą konfigurowałem w ConfigureServices)
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization(); //autoryzacja użytkowników WAŻNE: musi być po UseRouting(); i przed UseEndpoints(...);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
