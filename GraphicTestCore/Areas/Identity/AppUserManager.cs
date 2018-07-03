using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using qBI.Models;
using qBIPro.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qBI.Areas.Identity
{
    public class AppUserManager : UserManager<ApplicationUser>
    {
        private readonly ApplicationDbContext context;
        public AppUserManager(ApplicationDbContext _context, IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<AppUserManager> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            context = _context;
        }

        public override Task<IdentityResult> CreateAsync(ApplicationUser user)
        {

            return base.CreateAsync(user);
        }

        public override Task<IdentityResult> DeleteAsync(ApplicationUser user)
        {
            var area = context.Area.Find(user.AreaId);
            context.Remove(area);
            return base.DeleteAsync(user);
            
        }
    }
}
