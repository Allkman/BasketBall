using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BasketBall.Client.Helpers
{
    public class HttpResponseWrapper<T>
    {
        //This class represents a response from the server
        public HttpResponseWrapper(T response, bool success, HttpResponseMessage httpResponseMessage)
        {
            Response = response;
            Success = success;
            HttpResponseMessage = httpResponseMessage;
        }

        public T Response { get; set; }
        public bool Success { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; } //access to the contents of response message 

        public async Task<string> GetBody() //the body of the response of http request
        {
            return await HttpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}
