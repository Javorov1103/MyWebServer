using MyWebServer.HTTP.Enums;
using MyWebServer.HTTP.Headers;
using MyWebServer.HTTP.Headers.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebServer.HTTP.Responses.Contracts
{
    public interface IHttpResponse
    {
        HttpResponseStatusCode StatusCode { get; }

        IHttpHeadersCollection Headers { get; }

        byte[] Content { get; set; }

        void AddHeader(HttpHeader header);

        byte[] GetBytes();
    }
}
