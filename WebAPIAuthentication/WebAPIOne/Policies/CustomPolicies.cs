using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace WebAPIOne.Policies
{
    public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                return Task.FromResult(0);
            }

            var dateOfBirth = Convert.ToDateTime(context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth).Value);
            var qualification = Convert.ToString(context.User.FindFirst(c => c.Type == "Qualification").Value);


            int calculatedAge = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.Today.AddYears(-calculatedAge))
            {
                calculatedAge--;
            }

            if (calculatedAge >= requirement.MinimumAge && requirement.Qualification == qualification)
            {
                context.Succeed(requirement);
            }
            return Task.FromResult(0);
        }
    }

    #region Age and Qualification Class
    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public MinimumAgeRequirement(int age, string qualification)
        {
            MinimumAge = age;
            Qualification = qualification;
        }

        public int MinimumAge { get; set; }

        public string Qualification { get; set; }

    }
    #endregion
}
