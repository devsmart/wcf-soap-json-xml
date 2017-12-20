using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfDemo.API.Domain.Helpers
{
    internal class TokenHelper
    {
        /// <summary>
        /// Validates the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static bool Validate(string token, string url)
        {
            //implement validating token
            if (token.Length > 6 && token.ToLower().StartsWith("bearer"))
            {
                token = token.Substring(6, token.Length - 6).Trim();
                if (token == "3ffd9004-5d52-4603-af40-1f262431f368")
                {
                    return true;
                }
            }
            return false;
            //return AuthenticationSvcFactory.GetIUserAuthenticationManagerSvc(AuthenticationType.LinkedIn).Validate(token, url);

        }
    }
}
