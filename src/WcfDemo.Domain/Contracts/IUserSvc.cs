using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using WcfDemo.API.Domain.Models;

namespace WcfDemo.API.Domain.Contracts
{
    [ServiceContract(SessionMode = SessionMode.NotAllowed)]
    interface IUserSvc
    {
        /// <summary>
        /// APIs this instance.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "/ping", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string Ping();

        /// <summary>
        /// Method for allow Options calls for CORS
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "OPTIONS", UriTemplate = "*")]
        void GetOptions();

        /// <summary>
        /// Gets the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "users/byEmail/{email}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        User Get(string email);
    }
}
