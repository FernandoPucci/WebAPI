using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace ResourcesServer.Helpers
{
    public class AuthenticationHelper
    {

        public const string ADMINISTRATOR_GRANT = "ADMINISTRATOR"; 

        public static bool GetAdministratorPermission(ClaimsIdentity claims)
        {

            //only for debug purposes
            Debug.WriteLine("++ " + claims.FindFirst("userName").Value);
            Debug.WriteLine("++ " + claims.FindFirst("name").Value);
            Debug.WriteLine("++ " + claims.FindFirst("permissionRole").Value);
            Debug.WriteLine("++ " + claims.FindFirst("creationDate").Value);

            return claims.FindFirst("permissionRole").Value == ADMINISTRATOR_GRANT;

        }

    }
}