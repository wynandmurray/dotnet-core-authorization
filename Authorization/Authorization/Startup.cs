using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Authorization.AuthorizationClasses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Authorization
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
            services.AddControllers();
            services.AddHttpContextAccessor();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
                            {
                                x.RequireHttpsMetadata = false;
                                x.SaveToken = true;
                                x.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuerSigningKey = true,
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("A_VERY_SECRET_SECURITY_KEY_FOR_JWT_AUTH")),
                                    ValidateAudience = false,
                                    ValidateIssuer = false,
                                };
                            }
            );

            services.AddAuthorization(); // This does ONE thing only: Checks that the use is AUTHENTICATED (not Authorized).



            // Add the IAuthorizationPolicyProvider
            services.AddSingleton<IAuthorizationPolicyProvider, AgePolicyProvider>();

            // Add the FirstNameAuthHandler
            services.AddSingleton<IAuthorizationHandler, AgeHandler>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });
        }
    }
}