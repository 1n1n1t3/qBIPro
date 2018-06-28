using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qBI.Areas.Identity;
using qBIPro.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qBI.Models
{
    public class CustomerRequirement : IAuthorizationRequirement
    {
        public CustomerRequirement()
        {
        }
    }
    public class CustomerAuthorizationHandler : AuthorizationHandler<CustomerRequirement> , IAuthorizationRequirement
    {
        private readonly ApplicationDbContext dbContext;
        private readonly AppUserManager appUserManager;

        public CustomerAuthorizationHandler(ApplicationDbContext dbContext, AppUserManager appUserManager)
        {
            this.dbContext = dbContext;
            this.appUserManager = appUserManager;
        }
        public override Task HandleAsync(AuthorizationHandlerContext context)
        {
            return base.HandleAsync(context);
        }
        protected override  Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomerRequirement requirement)
        {

            if (context.Resource is AuthorizationFilterContext authContext)
            {
                var clientId = authContext.RouteData.Values["Id"].ToString();
                var user =  appUserManager.GetUserAsync(context.User).Result;

                if (dbContext.Customers.Find(Convert.ToInt32(clientId)).AreaId.Equals(user.AreaId))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
            return Task.CompletedTask; 
        }
    }
}
