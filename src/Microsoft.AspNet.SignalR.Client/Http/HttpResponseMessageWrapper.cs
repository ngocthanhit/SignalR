using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Microsoft.AspNet.SignalR.Client.Http
{
    public class HttpResponseMessageWrapper : IResponse
    {
        private HttpResponseMessage _httpResponseMessage;

        public HttpResponseMessageWrapper(HttpResponseMessage httpResponseMessage)
        {
            _httpResponseMessage = httpResponseMessage;
        }

        public string ReadAsString()
        {
            return _httpResponseMessage.Content.ReadAsStringAsync().Result;
        }

        public Stream GetStream()
        {
            return _httpResponseMessage.Content.ReadAsStreamAsync().Result;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _httpResponseMessage.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
