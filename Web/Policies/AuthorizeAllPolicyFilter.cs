using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CliveBot.Web.Policies
{
    public class AuthorizeAllPolicyFilter : IAsyncAuthorizationFilter
    {
        private readonly IAuthorizationService authorization;
        public string Policies { get; private set; }

        /// <summary>
        /// Initializes a new instance of the AuthorizeAnyPolicyFilter class.
        /// </summary>
        /// <param name="policies">A comma delimited list of policies that are allowed to access the resource.</param>
        /// <param name="authorization">The AuthorizationFilterContext.</param>
        public AuthorizeAllPolicyFilter(string policies, IAuthorizationService authorization)
        {
            Policies = policies;
            this.authorization = authorization;
        }

        /// <summary>
        /// Called early in the filter pipeline to confirm request is authorized.
        /// </summary>
        /// <param name="context">A context for authorization filters i.e. IAuthorizationFilter and IAsyncAuthorizationFilter implementations.</param>
        /// <returns>Sets the context.Result to ForbidResult() if the user fails all of the policies listed.</returns>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var policies = Policies.Split(",").ToList();

            foreach (var policy in policies)
            {
                var authorized = await authorization.AuthorizeAsync(context.HttpContext.User, policy);
                if (!authorized.Succeeded)
                {
                    context.Result = new ForbidResult();
                    return;
                }

            }
            return;
        }
    }
}
