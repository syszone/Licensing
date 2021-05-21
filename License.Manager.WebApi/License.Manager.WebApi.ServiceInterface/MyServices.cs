using License.Manager.WebApi.ServiceModel;
using ServiceStack;

namespace License.Manager.WebApi.ServiceInterface
{
    public class MyServices : Service
    {
        public object Any(Hello request)
        {
            return new HelloResponse { Result = $"Hello, {request.Name}!" };
        }
    }
}