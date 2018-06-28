using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using qBI.Models;
using System;
using System.Threading.Tasks;

namespace qBI.Areas.Identity
{
    public class AppSignInManager : SignInManager<ApplicationUser>
    {
        public AppSignInManager(AppUserManager userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<ApplicationUser>> logger, IAuthenticationSchemeProvider schemes) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        {
        }

        public override Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return base.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var result = base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
            if(result.Result.Succeeded)
            {
                ApplicationUser user =  await UserManager.FindByNameAsync(userName);
                user.LastLogin = DateTime.Now;
                await UserManager.UpdateAsync(user);
                return await result;
            }
            return await result;

        }

        protected override Task<SignInResult> PreSignInCheck(ApplicationUser user)
        {
            return base.PreSignInCheck(user);
        }
    }
}
