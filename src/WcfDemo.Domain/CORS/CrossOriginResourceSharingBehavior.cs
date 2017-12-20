using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace WcfDemo.API.Domain.CORS
{
    public class CrossOriginResourceSharingBehavior : BehaviorExtensionElement, IEndpointBehavior
    {
        /// <summary>
        /// The _logger
        /// </summary>
        //private static ILogger _logger = LogManager.GetLogger(typeof(CrossOriginResourceSharingBehavior).ToString());

        /// <summary>
        /// Adds the binding parameters.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="bindingParameters">The binding parameters.</param>
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            //no need to implement since we don't add any binding params
        }

        /// <summary>
        /// Applies the client behavior.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="clientRuntime">The client runtime.</param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            //no need to implement since we don't change client behavior
        }

        /// <summary>
        /// Applies the dispatch behavior.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="endpointDispatcher">The endpoint dispatcher.</param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            try
            {
                var requiredHeaders = new Dictionary<string, string>();

                requiredHeaders.Add("Access-Control-Allow-Origin", "*");
                requiredHeaders.Add("Access-Control-Request-Method", "GET,PUT,POST,DELETE,OPTIONS");
                requiredHeaders.Add("Access-Control-Allow-Headers", "Content-Type,Accept,Origin,Referer,User-Agent,Location,X-Authorization,Authorization,X-Requested-With");
                endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new CustomHeaderMessageInspector(requiredHeaders));

            }
            catch (Exception ex)
            {
              //  _logger.Error(ex);
            }
        }

        /// <summary>
        /// Validates the specified endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        public void Validate(ServiceEndpoint endpoint)
        {

        }

        /// <summary>
        /// Gets the type of behavior.
        /// </summary>
        public override Type BehaviorType
        {
            get { return typeof(CrossOriginResourceSharingBehavior); }
        }

        /// <summary>
        /// Creates a behavior extension based on the current configuration settings.
        /// </summary>
        /// <returns>
        /// The behavior extension.
        /// </returns>
        protected override object CreateBehavior()
        {
            return new CrossOriginResourceSharingBehavior();
        }
    }
}
