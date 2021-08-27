using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Datamodel.Models;

namespace Auth.IdentityServer
{
    public class MyUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public MyUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, 
                                            RoleManager<IdentityRole> roleManager, 
                                            IOptions<IdentityOptions> options) 
                            : base(userManager, roleManager, options)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("firstName", user.FirstName ?? ""));
            identity.AddClaim(new Claim("lastName", user.LastName ?? ""));
            //identity.AddClaim(new Claim("email", user.Email ?? ""));
            return identity;
        }
    }
}
