using ResourcesServer.Controllers.UserAccess;
using ResourcesServer.Models;
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
        #region Constants
        public const string ADMINISTRATOR_GRANT = "ADMINISTRATOR";
        #endregion

        public static bool GetAdministratorPermission(ClaimsIdentity claims, string operation)
        {

            Boolean authorize = claims.FindFirst("permissionRole").Value == ADMINISTRATOR_GRANT;

            #region Log Access
            // UserAccessesController userController = new UserAccessesController();

            UserAccess user = new UserAccess();
            user.AccessDate = DateTime.Now;
            user.User = claims.FindFirst("name") == null ? "N/A" : claims.FindFirst("name").Value;
            user.UserName = claims.FindFirst("userName") == null ? "N/A" : claims.FindFirst("userName").Value;
            user.PermissionRole = claims.FindFirst("permissionRole").Value;
            user.Operation = operation;
            user.IsGranted = authorize;
            log(user);

            #endregion

            #region Debug Claims
            //only for debug purposes
            Debug.WriteLine("++ " + claims.FindFirst("userName") == null ? "N/A" : claims.FindFirst("userName").Value);
            Debug.WriteLine("++ " + claims.FindFirst("name") == null ? "N/A" : claims.FindFirst("name").Value);
            Debug.WriteLine("++ " + claims.FindFirst("permissionRole") == null ? "N/A" : claims.FindFirst("permissionRole").Value);
            Debug.WriteLine("++ " + claims.FindFirst("ge_usuario") == null ? "N/A" : claims.FindFirst("ge_usuario").Value);
            //Debug.WriteLine("++ " + claims.FindFirst("creationDate") == null ? "N/A" : claims.FindFirst("creationDate").Value);
            #endregion

            return authorize;


        }

        private static async void log(UserAccess user)
        {
            UserAccessesController userController = new UserAccessesController();
            userController.LogAccess(user);
        }


    }
}