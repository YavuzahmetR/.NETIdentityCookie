using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IdentityAppCookie.Web.Requirements
{
    public class ExchangeExpireRequirement : IAuthorizationRequirement
    {
        //Sending parameter - constraint for handler from program.cs : public int Age {get; set;}
    }

    public class ExchangeExpireRequirementHandler : AuthorizationHandler<ExchangeExpireRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ExchangeExpireRequirement requirement)
        {
            //requirement.Age 
            //business codes 
            if (!context.User.HasClaim(x => x.Type == "ExchangeExpireDate"))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            Claim exchangeExpireDate = context.User.FindFirst("ExchangeExpireDate")!;

            if(DateTime.Now > Convert.ToDateTime(exchangeExpireDate.Value)) 
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
