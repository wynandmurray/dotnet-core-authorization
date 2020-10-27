using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Authorization.AuthorizationClasses
{
    public class AgePolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AgePolicyProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() 
        {
            var builder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
            builder.RequireAuthenticatedUser();

            var firstName = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var ageString = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Age")?.Value;
            int age = 0;
            if (!string.IsNullOrEmpty(ageString))
            {
                age = int.Parse(ageString);
            }

            builder.AddRequirements(new AgeRequirement(firstName, age));

            return Task.FromResult(builder.Build());
        }

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return Task.FromResult<AuthorizationPolicy>(null);
        }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            return Task.FromResult<AuthorizationPolicy>(null);
        }
    }
}
