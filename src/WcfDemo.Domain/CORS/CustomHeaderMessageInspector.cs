using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace WcfDemo.API.Domain.CORS
{
    public class CustomHeaderMessageInspector : IDispatchMessageInspector
    {
        /// <summary>
        /// The required headers
        /// </summary>
        Dictionary<string, string> requiredHeaders;

        /// <summary>
        /// The _logger
        /// </summary>
        //private static ILogger _logger = LogManager.GetLogger(typeof(CustomHeaderMessageInspector).ToString());

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomHeaderMessageInspector"/> class.
        /// </summary>
        /// <param name="headers">The headers.</param>
        public CustomHeaderMessageInspector(Dictionary<string, string> headers)
        {
            requiredHeaders = headers ?? new Dictionary<string, string>();
        }

        /// <summary>
        /// Called after an inbound message has been received but before the message is dispatched to the intended operation.
        /// </summary>
        /// <param name="request">The request message.</param>
        /// <param name="channel">The incoming channel.</param>
        /// <param name="instanceContext">The current service instance.</param>
        /// <returns>
        /// The object used to correlate state. This object is passed back in the <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.BeforeSendReply(System.ServiceModel.Channels.Message@,System.Object)" /> method.
        /// </returns>
        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            return null;
        }

        /// <summary>
        /// Called after the operation has returned but before the reply message is sent.
        /// </summary>
        /// <param name="reply">The reply message. This value is null if the operation is one way.</param>
        /// <param name="correlationState">The correlation object returned from the <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.AfterReceiveRequest(System.ServiceModel.Channels.Message@,System.ServiceModel.IClientChannel,System.ServiceModel.InstanceContext)" /> method.</param>
        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            try
            {
                var httpHeader = reply.Properties["httpResponse"] as HttpResponseMessageProperty;
                foreach (var item in requiredHeaders)
                {
                    httpHeader.Headers.Add(item.Key, item.Value);
                }
            }
            catch (Exception ex)
            {
              //  _logger.Error(ex);
            }

        }
    }
}
