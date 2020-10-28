using Microsoft.AspNetCore.Authorization;

namespace Authorization.AuthorizationClasses
{
    public class AgeRequirement : IAuthorizationRequirement
    {
        public string FirstName { get; }
        public int Age { get; set; }

        public AgeRequirement(string firstName, int age)
        {
            FirstName = firstName;
            Age = age;
        }
    }
}
