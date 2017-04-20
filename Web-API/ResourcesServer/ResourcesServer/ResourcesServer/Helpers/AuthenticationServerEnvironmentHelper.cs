using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourcesServer.Helpers
{
    public class AuthenticationServerEnvironmentHelper
    {
        public static string GetAuthenticationURI(string environment)
        {
            switch (environment)
            {
                case "PRD":
                    return "http://prod:12206/token";
                case "QA":
                    return "http://qa:12206/token";
                default:
                    return "http://localhost:12206/token";  // probably locally-hosted within IIS on developer machine
            }
        }
    }
}