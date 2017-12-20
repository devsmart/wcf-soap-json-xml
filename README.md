# WCF Service with JSON, XML and SOAP

Basic useful features:

 * Support JSON, XML and SOAP in single implementation
 * Custom authorization or any oAuth/token based or any other implementation
 * CORS support with configurable domain names 

To run the application you may need

 * Visual Studio 2015

## Keep in Mind
 * This sample having hard coded Authorization token "3ffd9004-5d52-4603-af40-1f262431f368"
 * web.config is just only for debuging, change as Web.Release.config as you prefer :) I suggest to remove access HTTP and set only HTTPS ;)

### Service Metadata?
Why Not, Simply visit [http://server/service/help](http://server/service/help) 

E.G. [http://localhost:52308/User.svc/help](http://localhost:52308/User.svc/help)

### Service definition to support JSON/XML & SOAP in web.config
When you adding new service just add below configs to new service
```xml
<service name="WcfDemo.API.Domain.UserSvc" behaviorConfiguration="DefaultServiceBehavior">
        <!--Added Service with DefaultServiceBehavior as behaviorConfiguration-->
        <endpoint address="soap" binding="wsHttpBinding" contract="WcfDemo.API.Domain.Contracts.IUserSvc" />
        <!--Added basicHttpBinding as SoapService (wsHttpBinding)-->
        <endpoint address="" binding="webHttpBinding" behaviorConfiguration="RESTEndPointBehavior" contract="WcfDemo.API.Domain.Contracts.IUserSvc" />
        <!--Added webHttpBinding as RestService with RESTEndPointBehavior as behaviorConfiguration-->
      </service>
```

### Implement ServiceAuthorizationManager as you wish
```csharp
namespace WcfDemo.API.Domain.Auth
{
    public class AuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
			//write your own logic, see the code for sample implementation
                if (operationContext.EndpointDispatcher.ChannelDispatcher.BindingName.ToLower().EndsWith("webhttpbinding"))
                    {
                        isWebHttp = true;
                        System.ServiceModel.Web.WebOperationContext ctx = System.ServiceModel.Web.WebOperationContext.Current;
                        //ignore routes
                        if (ctx.IncomingRequest.Method.ToUpper() == "OPTIONS")
                        {
                            Uri origin = new Uri(ctx.IncomingRequest.Headers["Origin"]);
                            ////validate CORS domain contains
                            if (allowedDomains.Contains(origin.Host))
                            	return true;
                            else
                                return false;
                        }
                    }
        }
    }
}
    
```

### CORS support
```csharp
namespace WcfDemo.API.Domain.CORS
{
    public class CrossOriginResourceSharingBehavior : BehaviorExtensionElement, IEndpointBehavior
    { 
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
            }
        }
	........
    }
}


```
### API calls
![fiddler capture](https://raw.githubusercontent.com/devsmart/wcf-soap-json-xml/master/images/fiddler.png)
```
GET http://localhost:52308/User.svc/ping -> Pong

GET http://localhost:52308/User.svc/users/byEmail/abc@abc.com -> 401 Unauthorized 

GET http://localhost:52308/User.svc/users/byEmail/abc@abc.com with auth header "Authorization: Bearer 3ffd9004-5d52-4603-af40-1f262431f368" -> {"email":"abc@abc.com","firstName":"FirstName","id":726405102,"lastName":"LastName"}

GET http://localhost:52308/User.svc/ping with header "Content-Type: application/xml" -> <string xmlns="http://schemas.microsoft.com/2003/10/Serialization/">Pong</string>

GET http://localhost:52308/User.svc/users/byEmail/abc@abc.com with  headers "Authorization: Bearer 3ffd9004-5d52-4603-af40-1f262431f368" and "Content-Type: application/xml" -> <user xmlns="http://schemas.datacontract.org/2004/07/WcfDemo.API.Domain.Models" xmlns:i="http://www.w3.org/2001/XMLSchema-instance"><email>abc@abc.com</email><firstName>FirstName</firstName><id>40504766</id><lastName>LastName</lastName></user>

```
### SOAP client 
```csharp
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
```

