namespace MyWebServer.HTTP.Cookies
{
    using MyWebServer.HTTP.Cookies.Contracts;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class HttpCookieCollection : IHttpCookieCollection
    {
        private readonly IDictionary<string, HttpCookie> cookies;

        public HttpCookieCollection()
        {
            this.cookies = new Dictionary<string, HttpCookie>();
        }

        public void Add(HttpCookie cookie)
        {
            if (cookie == null )
            {
                throw new ArgumentNullException();
            }

            // TODO: Overwrite cookies???

            //if (this.ContainsCookie(cookie.Key))
            //{
            //    throw new Exception();
            //}

            this.cookies[cookie.Key] = cookie;
        }
        
        public HttpCookie GetCookie(string key)
        {
            if (!this.ContainsCookie(key))
            {
                return null;
            }
            return this.cookies[key];
        }

        public bool ContainsCookie(string key)
        {
            return this.cookies.ContainsKey(key);
        }

        public bool HasCookies()
        {
            return this.cookies.Any();
        }

        public override string ToString()
        {
            return string.Join("; ", this.cookies.Values);
        }

        public IEnumerator<HttpCookie> GetEnumerator()
        {
            foreach (var cookie in this.cookies)
            {
                yield return cookie.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
