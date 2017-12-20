using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace WcfDemo.ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var svc = new ServiceReference1.UserSvcClient();
            using (var scope = new OperationContextScope(svc.InnerChannel))
            {
                var authHeader = MessageHeader.CreateHeader("Authorization", "http://tempuri.org", "Bearer 3ffd9004-5d52-4603-af40-1f262431f368");
                OperationContext.Current.OutgoingMessageHeaders.Add(authHeader);
                // Do this if you want to use http header instead
                //var httpRequestProperty = new HttpRequestMessageProperty();
                //httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = result.AccessToken;
                //OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                var res = svc.Get("abc@abc.com");
                Console.WriteLine("id : {0} , FirstName : {1}", res.id, res.firstName);
                Console.Read();
            }
        }
    }
}
