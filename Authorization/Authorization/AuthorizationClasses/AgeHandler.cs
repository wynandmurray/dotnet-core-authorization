using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Authorization.AuthorizationClasses
{
    public class AgeHandler : AuthorizationHandler<AgeRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AgeHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(
                                                        AuthorizationHandlerContext context,
                                                        AgeRequirement requirement)
        {
            var requestedAgeString = _httpContextAccessor
                                    .HttpContext
                                    .Request
                                    .Query["age"].ToString();

            int requestedAge = 0;

            if (!string.IsNullOrEmpty(requestedAgeString))
            {
                requestedAge = int.Parse(requestedAgeString);
            }

            var authorizedAge = requirement.Age;


            if (requestedAge >= authorizedAge)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
