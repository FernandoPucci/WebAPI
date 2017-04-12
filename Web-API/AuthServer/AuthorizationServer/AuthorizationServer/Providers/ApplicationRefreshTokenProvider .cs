using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AuthorizationServer.Providers
{
    /// <summary>
    /// ApplicationRefreshTokenProvider  prove o refresh_token, que permite solicitação de novo token a partir do mesmo
    /// Mais informações: http://stackoverflow.com/questions/35743945/how-to-update-owin-access-tokens-with-refresh-tokens-without-creating-new-refres
    ///                   http://stackoverflow.com/questions/20637674/owin-security-how-to-implement-oauth2-refresh-tokens
    /// </summary>
    public class ApplicationRefreshTokenProvider : AuthenticationTokenProvider
    {
        private static ConcurrentDictionary<string, AuthenticationTicket> _refreshTokens = new ConcurrentDictionary<string, AuthenticationTicket>();

        public override void Create(AuthenticationTokenCreateContext context)
        {
            var guid = Guid.NewGuid().ToString();


            _refreshTokens.TryAdd(guid, context.Ticket);
            
            context.SetToken(guid);
        }

        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            AuthenticationTicket ticket;

            if (_refreshTokens.TryRemove(context.Token, out ticket))
            {
                context.SetTicket(ticket);
            }
        }

        //public void Create(AuthenticationTokenCreateContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Receive(AuthenticationTokenReceiveContext context)
        //{
        //    throw new NotImplementedException();
        //}

    }
}