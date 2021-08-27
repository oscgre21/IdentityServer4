using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Datamodel.Models;

namespace Auth.IdentityServer
{
    public class ProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            //Get Required claims
            var userEmailClaim = context.Subject.Claims.FirstOrDefault(c => c.Type == "email");
            //var userPhone = context.Subject.Claims.FirstOrDefault(c => c.Type.Contains("phone_number"));
            var firstName = context.Subject.Claims.FirstOrDefault(c => c.Type == "firstName");
            var lastName = context.Subject.Claims.FirstOrDefault(c => c.Type == "lastName");

            var claims = context.Subject.Claims.ToList();
            //Filter for onlyrequested claims
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
            //Now check if required claims are not in the list, add it.
            claims.Add(new Claim("userId", sub));
            if (!claims.Any(c => c.Type.Contains("email")))
                claims.Add(userEmailClaim);
            //if (!claims.Any(c => c.Type.Contains("phone_number")))
            //    claims.Add(userPhone);
            if (!claims.Any(c => c.Type == "firstName"))
                claims.Add(firstName);
            if (!claims.Any(c => c.Type == "lastName"))
                claims.Add(lastName);

            context.IssuedClaims = claims;
            return Task.CompletedTask;

        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
