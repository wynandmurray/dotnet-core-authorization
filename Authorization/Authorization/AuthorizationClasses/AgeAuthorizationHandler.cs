using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Authorization.AuthorizationClasses
{
    public class AgeAuthorizationHandler : AuthorizationHandler<AgeRequirement>
    {
        protected override Task HandleRequirementAsync(
                                                        AuthorizationHandlerContext context,
                                                        AgeRequirement requirement)
        {
            var requestedAge = requirement.Age;
            var authorizedAgeString = context.User.Claims.SingleOrDefault(c => c.Type == "Age")?.Value;

            if (string.IsNullOrEmpty(authorizedAgeString))
            {
                context.Fail();
            }
            else
            {
                if (requestedAge >= int.Parse(authorizedAgeString))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
