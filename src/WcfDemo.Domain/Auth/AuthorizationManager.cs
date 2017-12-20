using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WcfDemo.API.Domain.Helpers;

namespace WcfDemo.API.Domain.Auth
{ 

    public class AuthorizationManager : ServiceAuthorizationManager
    {
        /// <summary>
        /// The configuration
        /// </summary>
        //private static IConfiguration config = ConfigManager.GetConfigManager();
        /// <summary>
        /// The _logger
        /// </summary>
       // private static ILogger _logger = LogManager.GetLogger(typeof(AuthorizationManager).ToString());


        /// <summary>
        /// Checks authorization for the given operation context based on default policy evaluation.
        /// </summary>
        /// <param name="operationContext">The <see cref="T:System.ServiceModel.OperationContext" /> for the current authorization request.</param>
        /// <returns>
        /// true if access is granted; otherwise, false. The default is true.
        /// </returns>
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            //if (_logger.IsTraceEnabled) _logger.Trace("{0} - {1}", MethodBase.GetCurrentMethod().Name, _logger.Start);
            //dev env disable token validation
            //if (config.GetValueBool("Api.IgnoreAuthValidation"))
            //{
            //    return true;
            //}

            bool returnValue = false;
            bool isWebHttp = false;
            string token = "";
            string fullUrl = "";
            try
            {
                try
                {
                    if (operationContext.EndpointDispatcher.ChannelDispatcher.BindingName.ToLower().EndsWith("webhttpbinding"))
                    {
                        isWebHttp = true;
                        System.ServiceModel.Web.WebOperationContext ctx = System.ServiceModel.Web.WebOperationContext.Current;
                        //ignore routes
                        if (ctx.IncomingRequest.Method.ToUpper() == "OPTIONS")
                        {
                            Uri origin = new Uri(ctx.IncomingRequest.Headers["Origin"]);
                            //IList<String> allowedDomains = config.GetList<string>("Api.CORS_AllowedDomains");

                            //if (_logger.IsDebugEnabled) _logger.Debug("Api.CORS_AllowedDomains - {0}", string.Join(",", allowedDomains));
                            ////validate CORS domain contains
                            //if (allowedDomains.Contains(origin.Host))
                            //    returnValue = true;
                            //else
                            //{
                            //    returnValue = false;
                            //    if (_logger.IsDebugEnabled) _logger.Debug("origin.Host - {0}", origin.Host);
                            //}
                        }
                        else
                        {
                            string url = operationContext.IncomingMessageProperties.Via.AbsolutePath.ToLower();
                            if (url.EndsWith("/ping"))
                                returnValue = true;
                            else if (url.EndsWith("/token"))
                                returnValue = true;
                            else if (url.EndsWith("/help"))
                                returnValue = true;
                            else if (url.Contains("/help/operations/"))
                                returnValue = true;
                            else
                            {
                                token = ctx.IncomingRequest.Headers["Authorization"];
                                fullUrl = ctx.IncomingRequest.UriTemplateMatch.RequestUri.PathAndQuery;
                                //if (_logger.IsDebugEnabled)
                                //{
                                //    _logger.Trace("{0} - {1}", "token", token);
                                //    _logger.Trace("{0} - {1}", "fullUrl", fullUrl);
                                //}
                                //fullUrl = operationContext.IncomingMessageHeaders.To.AbsoluteUri;
                            }
                        }
                    }
                    else
                    {
                        //soap
                        if (operationContext.IncomingMessageHeaders.Action.ToLower().EndsWith("/ping"))
                            returnValue = true;
                        else if (operationContext.IncomingMessageHeaders.Action.ToLower().EndsWith("/token"))
                            returnValue = true;
                        else
                            token = operationContext.IncomingMessageHeaders.GetHeader<string>("Authorization", "http://tempuri.org").Trim();
                    }
                }
                catch (Exception ex)
                {
                 //   _logger.Error(ex);
                }

                if (!string.IsNullOrEmpty(token))
                {
                    returnValue = TokenHelper.Validate(token, fullUrl);
                }
            }
            catch (Exception ex)
            {
                returnValue = false;
          //      _logger.Error(ex);
            }
           // if (_logger.IsTraceEnabled) _logger.Trace("{0} - {1}", MethodBase.GetCurrentMethod().Name, _logger.Start);
            if (isWebHttp && !returnValue)
            {
                throw new System.ServiceModel.Web.WebFaultException(System.Net.HttpStatusCode.Unauthorized);
            }
            return returnValue;
        }
    }

}
