namespace MyWebServer.HTTP.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MyWebServer.HTTP.Common;
    using MyWebServer.HTTP.Cookies;
    using MyWebServer.HTTP.Cookies.Contracts;
    using MyWebServer.HTTP.Enums;
    using MyWebServer.HTTP.Exceptions;
    using MyWebServer.HTTP.Headers;
    using MyWebServer.HTTP.Headers.Contracts;
    using MyWebServer.HTTP.Requests.Contracts;
    using MyWebServer.HTTP.Sessions.Contracts;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            this.FormData = new Dictionary<string, object>();

            this.QueryData = new Dictionary<string, object>();

            this.Headers = new HttpHeadersCollection();

            this.Cookies = new HttpCookieCollection();

            this.ParseRequest(requestString);
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeadersCollection Headers { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public IHttpCookieCollection Cookies { get; }

        public IHttpSession Session { get ; set; }



        //Parse Mehtods

        private void ParseRequest(string requestString)
        {
            var splitRequestContent = requestString
                .Split(Environment.NewLine, StringSplitOptions.None);

            var requestLine = splitRequestContent[0].Trim()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValidRequestLine(requestLine))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLine);
            this.ParseRequestUrl(requestLine);
            this.ParseRequestPath();

            this.ParseHeaders(splitRequestContent.Skip(1).ToArray());
            this.ParseCookies();

            bool requestHasBody = splitRequestContent.Length > 1;

            this.ParseRequestParameters(splitRequestContent[splitRequestContent.Length - 1], requestHasBody);
        }

        private void ParseCookies()
        {
            if (!this.Headers.ContainsHeader(GlobalConstants.CookieRequestHeaderName))
            {
                return;
            }

            var cookiesString = this.Headers.GetHeader(GlobalConstants.CookieRequestHeaderName).Value;
            if (string.IsNullOrEmpty(cookiesString)) return;
            
            var splitCookies = cookiesString.Split("; ", StringSplitOptions.RemoveEmptyEntries);

            foreach (var splitCookie in splitCookies)
            {
                var cookieKeyValuePair = splitCookie.Split("=",2,StringSplitOptions.RemoveEmptyEntries);
                if (cookieKeyValuePair.Length !=
                    GlobalConstants.ExactNumberOfParametersInRequestParameterKeValuePair)
                {
                    throw new BadRequestException();
                }

                var cookieName = cookieKeyValuePair[0];
                var cookieValue = cookieKeyValuePair[1];

                this.Cookies.Add(new HttpCookie(cookieName, cookieValue));
            }
        }

        private void ParseRequestParameters(string bodyParameters, bool requestHasBody)
        {
            this.ParseQueryParameters(this.Url);
            if (requestHasBody)
            {
                ParseFormDataParameters(bodyParameters);
            }

        }

        private void ParseFormDataParameters(string bodyParameters)
        {
            var formDataParameters = bodyParameters.Split('&', StringSplitOptions.RemoveEmptyEntries);

            ExtractParameters(formDataParameters, this.FormData);
        }

        private void ParseQueryParameters(string url)
        {
            var queryParameters = this.Url?.Split(new [] { '?', '#' })
                .Skip(1).ToArray();

            //if (string.IsNullOrWhiteSpace(queryParameters[1]))
            //{
            //    return;
            //}

            if (IsValidRequestQueryString(queryParameters))
            {
                var queryKeyValuePairs = queryParameters[0]
               .Split('&', StringSplitOptions.RemoveEmptyEntries);

                ExtractParameters(queryKeyValuePairs, this.QueryData);
            }
        }

        private bool IsValidRequestQueryString(string[] queryParameters)
        {
            if (!queryParameters.Any())
            {
                return false;
            }
            return true;
        }

        private void ExtractParameters(string[] paramsKeyValuePairs, Dictionary<string, object> paramsCollection)
        {
            foreach (var kvp in paramsKeyValuePairs)
            {
                var keyValuePair = kvp.Split('=', StringSplitOptions.RemoveEmptyEntries);

                if (keyValuePair.Length != 
                    GlobalConstants.ExactNumberOfParametersInRequestParameterKeValuePair)
                {
                    throw new BadRequestException();
                }

                var paramKey = keyValuePair[0];
                var paramValue = keyValuePair[1];


                paramsCollection[paramKey] = paramValue;
            }
        }

        private void ParseHeaders(string[] requestHeaders)
        {
            if (!requestHeaders.Any())
            {
                throw new BadRequestException();
            }

            foreach (var requestHeader in requestHeaders)
            {
                if (string.IsNullOrEmpty(requestHeader))
                {
                    return;
                }

                var splitRequestHeader = requestHeader.Split(": ", StringSplitOptions.RemoveEmptyEntries);

                var requestHeaderKey = splitRequestHeader[0];
                var requestHeaderValue = splitRequestHeader[1];

                this.Headers.Add(new HttpHeader(requestHeaderKey, requestHeaderValue));
            }

            if (!this.Headers.ContainsHeader("Host"))
            {
                throw new BadRequestException();
            }


        }

        private void ParseRequestPath()
        {
            var path = this.Url?.Split('?').FirstOrDefault();

            if (string.IsNullOrEmpty(path))
            {
                throw new BadRequestException();
            }

            this.Path = path;
        }

        private void ParseRequestUrl(string[] requestLine)
        {
            if (string.IsNullOrEmpty(requestLine[1]))
            {
                throw new BadRequestException();
            }

            this.Url = requestLine[1];

        }

        private void ParseRequestMethod(string[] requestLine)
        {
            var parseResult = Enum.TryParse<HttpRequestMethod>(requestLine[0], out var parsedMethod);
            if (!parseResult)
            {
                throw new BadRequestException();
            }

            this.RequestMethod = parsedMethod;
        }

        private bool IsValidRequestLine(string[] requestLine)
        {
            if (!requestLine.Any())
            {
                throw new BadRequestException();
            }

            if (requestLine.Length == 3 &&
                requestLine[2] == GlobalConstants.HttpOneProtocolFragment)
            {
                return true;
            }

            return false;
        }

        //End of Parse Methods
    }
}
