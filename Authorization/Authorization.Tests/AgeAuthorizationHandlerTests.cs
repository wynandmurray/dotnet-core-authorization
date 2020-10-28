using Shouldly;
using System.Threading.Tasks;
using System.Security.Claims;
using Authorization.AuthorizationClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Authorization.Tests
{
    [TestClass]
    public class AgeAuthorizationHandlerTests
    {
        [TestMethod]
        public async Task User_Authorized()
        {
            // Arrange
            var requirement = new[] { new AgeRequirement("Wynand", 36) };

            var user = new ClaimsPrincipal(
                                            new ClaimsIdentity(
                                                new Claim[] {
                                                    new Claim(ClaimsIdentity.DefaultNameClaimType, "Wynand"),
                                                    new Claim("Age", "36")
                                                }, "Basic")
            );

            var context = new AuthorizationHandlerContext(requirement, user, null);
            var subject = new AgeAuthorizationHandler();

            // Act
            await subject.HandleAsync(context);

            // Assert
            context.HasSucceeded.ShouldBeTrue();
        }

        [TestMethod]
        public async Task User_NotAuthorized()
        {
            // Arrange
            var requirement = new[] { new AgeRequirement("Wynand Jr.", 34) };

            var user = new ClaimsPrincipal(
                                            new ClaimsIdentity(
                                                new Claim[] {
                                                    new Claim(ClaimsIdentity.DefaultNameClaimType, "Wynand"),
                                                    new Claim("Age", "36")
                                                }, "Basic")
            );

            var context = new AuthorizationHandlerContext(requirement, user, null);
            var subject = new AgeAuthorizationHandler();

            // Act
            await subject.HandleAsync(context);

            // Assert
            context.HasSucceeded.ShouldBeFalse();
        }

    }
}
