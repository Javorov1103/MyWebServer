namespace MyWebServer.HTTP.Headers
{
    using MyWebServer.HTTP.Headers.Contracts;
    using System;
    using System.Collections.Generic;

    public class HttpHeadersCollection : IHttpHeadersCollection
    {
        private readonly IDictionary<string, HttpHeader> headers;

        public HttpHeadersCollection()
        {
            this.headers = new Dictionary<string, HttpHeader>();
        }


        public void Add(HttpHeader header)
        {
            if (header == null 
                || string.IsNullOrEmpty(header.Key) 
                || string.IsNullOrEmpty(header.Value) 
                || this.ContainsHeader(header.Key))
            {
                throw new Exception();
            }

            this.headers.Add(header.Key, header);
        }

        public bool ContainsHeader(string key)
        {
            if(string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException($"{nameof(key)} cannot be null!");
            }
            
            return this.headers.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException($"{nameof(key)} cannot be null!");
            }

            if (this.ContainsHeader(key))
            {
                return this.headers[key];
            }

            return null;
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this.headers.Values);
        }
    }
}
