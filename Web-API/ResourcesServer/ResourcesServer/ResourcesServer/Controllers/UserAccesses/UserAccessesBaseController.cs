using ResourcesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using static ResourcesServer.Models.UserAccess;

namespace ResourcesServer.Controllers.UserAccess
{
    public class UserAccessesBaseController : ApiController
    {
        #region Database Configurations
        protected UserAccessesContext db = new UserAccessesContext(); 
        private bool UserAcessesExists(Guid key)
        {
            return db.UserAccesses.Any(p => p.Id == key);
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        #endregion
    }
}