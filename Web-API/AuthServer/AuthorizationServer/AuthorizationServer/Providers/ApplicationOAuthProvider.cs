using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using AuthorizationServer.Models;

namespace AuthorizationServer.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                CookieAuthenticationDefaults.AuthenticationType);



            //Adding Claim to recover into Resource Server          
            oAuthIdentity.AddClaim(new Claim("userName", user.UserName));
            oAuthIdentity.AddClaim(new Claim("name", user.Name));
            oAuthIdentity.AddClaim(new Claim("permissionRole", user.PermissionRole));
            oAuthIdentity.AddClaim(new Claim("creationDate", user.CreationDate.ToString()));

            //cookiesIdentity.AddClaim(new Claim("userName", user.UserName));
            //cookiesIdentity.AddClaim(new Claim("name", user.Name));
            //cookiesIdentity.AddClaim(new Claim("permissionRole", user.PermissionRole));
            //cookiesIdentity.AddClaim(new Claim("creationDate", user.CreationDate.ToString()));


            AuthenticationProperties properties = CreateProperties(user.UserName
                                                                    , user.Name
                                                                    , user.PermissionRole
                                                                    , user.CreationDate);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        // Add Properties to User to insert  into context and response
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="name"></param>
        /// <param name="permissionRole"></param>
        /// <param name="creationDate"></param>
        /// <returns></returns>
        public static AuthenticationProperties CreateProperties(string userName, string name, string permissionRole, DateTime creationDate)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName },
                { "name", name},
                { "permissionRole", permissionRole },
                { "creationDate", creationDate.ToString() },

            };
            return new AuthenticationProperties(data);
        }
    }
}