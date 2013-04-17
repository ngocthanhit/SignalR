// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNet.SignalR.Client.Http
{
    /// <summary>
    /// The default <see cref="IHttpClient"/> implementation.
    /// </summary>
    public class DefaultHttpClient : IHttpClient
    {
        /// <summary>
        /// Makes an asynchronous http GET request to the specified url.
        /// </summary>
        /// <param name="url">The url to send the request to.</param>
        /// <param name="prepareRequest">A callback that initializes the request with default values.</param>
        /// <returns>A <see cref="T:Task{IResponse}"/>.</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "To Add")]
        public Task<IResponse> Get(string url, Action<IRequest> prepareRequest)
        {
            var cts = new CancellationTokenSource();
            var handler = new DefaultHttpHandler(prepareRequest, cts.Cancel);
            var client = new HttpClient(handler);
            return client.GetAsync(new Uri(url), HttpCompletionOption.ResponseHeadersRead, cts.Token)
                .Then(responseMessage => (IResponse)new HttpResponseMessageWrapper(responseMessage));
        }

        /// <summary>
        /// Makes an asynchronous http POST request to the specified url.
        /// </summary>
        /// <param name="url">The url to send the request to.</param>
        /// <param name="prepareRequest">A callback that initializes the request with default values.</param>
        /// <param name="postData">form url encoded data.</param>
        /// <returns>A <see cref="T:Task{IResponse}"/>.</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "To Add")]
        public Task<IResponse> Post(string url, Action<IRequest> prepareRequest, IDictionary<string, string> postData)
        {
            var cts = new CancellationTokenSource();
            var handler = new DefaultHttpHandler(prepareRequest, cts.Cancel);
            var client = new HttpClient(handler);
            var req = new HttpRequestMessage(HttpMethod.Post, new Uri(url));

            if (postData == null)
            {
                req.Content = new StringContent(String.Empty);
            }
            else
            {
                req.Content = new FormUrlEncodedContent(postData);
            }

            return client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, cts.Token).
                Then(responseMessage =>
                    (IResponse)new HttpResponseMessageWrapper(responseMessage)
                );
        }
    }
}
