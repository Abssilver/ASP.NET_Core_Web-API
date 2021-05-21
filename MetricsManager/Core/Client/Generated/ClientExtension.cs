using System.Net.Http;

namespace Core.Client.Generated
{
    public partial class Client
    {
        protected HttpResponseMessage ResponseMessage;
        partial void ProcessResponse(System.Net.Http.HttpClient client, HttpResponseMessage response)
        {
            ResponseMessage = response;
        }
    }
}