using Hackathon.Instagram.Core.Extensions;
using Hackathon.Instagram.Core.Models.Response;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Hackathon.Instagram.Core.EndPoint
{
    public abstract class InstagramApi
    {
        private string XInstaForwardedHeader { get; set; }

        /// <summary>
        /// Gets the instagram configuration.
        /// </summary>
        /// <value>
        /// The instagram configuration.
        /// </value>
        public InstagramConfig InstagramConfig { get; private set; }

        /// <summary>
        /// Gets the o authentication response.
        /// </summary>
        /// <value>
        /// The o authentication response.
        /// </value>
        public OAuthResponse OAuthResponse { get; private set; }

        internal HttpClient Client { get; private set; }

        /// <summary>
        ///  IP information: Comma-separated list of one or more IPs; if your app receives requests directly from clients,
        ///  then it should be the client's remote IP as detected by the your app's load balancer; if your app is behind another load balancer (for example, Amazon's ELB),
        ///  this should contain the exact contents of the original X-Forwarded-For header. You can use the 127.0.0.1 loopback address during testing
        /// </summary>
        public string Ips { get; private set; }

        /// <summary>
        /// Requires the use of your Client Secret to sign POST and DELETE API requests. Use this option to instruct Instagram to check requests for the 'X-Insta-Forwarded-For' HTTP header. 
        /// Eligible requests that do not provide this header and a valid signature will fail. This technique helps identify you as the legitimate owner of this OAuth Client. Only enable 
        /// this option for server-to-server calls. See the Restrict API Requests documentation for details. http://instagram.com/developer/restrict-api-requests/
        /// This needs to be configured at application level
        /// </summary>
        public bool EnforceSignedHeader { get; private set; }

        internal InstagramApi(string endpoint, InstagramConfig instagramConfig)
            : this(endpoint, instagramConfig, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstagramApi"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="instagramConfig">The instagram configuration.</param>
        /// <param name="oauthResponse">The oauth response.</param>
        protected InstagramApi(string endpoint, InstagramConfig instagramConfig, OAuthResponse oauthResponse)
        {
            InstagramConfig = instagramConfig;
            OAuthResponse = oauthResponse;

            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip |
                                                 DecompressionMethods.Deflate;
            }

            Client = new HttpClient(handler) { BaseAddress = new Uri(new Uri(InstagramConfig.ApiUri), endpoint) };
        }

        /// <summary>
        /// Asserts if the user is authenticated.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">You are not authenticated</exception>
        protected void AssertIsAuthenticated()
        {
            if (OAuthResponse == null || OAuthResponse.User == null)
            {
                throw new InvalidOperationException("You are not authenticated");
            }
        }

        /// <summary>
        /// Disables Enforced signed header.  See the Restrict API Requests documentation for details. http://instagram.com/developer/restrict-api-requests/
        /// </summary>
        /// <param name="ipAdresses"></param>
        public void DisableEnforceSignedHeader(string ipAdresses)
        {
            Ips = null;
            EnforceSignedHeader = false;
            XInstaForwardedHeader = null;
        }

        internal HttpRequestMessage Request(string fragment, HttpMethod method)
        {
            var request = new HttpRequestMessage(method, new Uri(Client.BaseAddress, fragment));
            AddHeaders(request);
            return AddAuth(request);
        }

        /// <param name="request"></param>
        private void AddHeaders(HttpRequestMessage request)
        {
            if (EnforceSignedHeader && !String.IsNullOrWhiteSpace(InstagramConfig.ClientSecret)
                                    && (request.Method == HttpMethod.Post || request.Method == HttpMethod.Delete))
            {
                request.Headers.Add("X-Insta-Forwarded-For", XInstaForwardedHeader);
            }
        }

        internal HttpRequestMessage Request(string fragment)
        {
            return Request(fragment, HttpMethod.Get);
        }

        internal virtual HttpRequestMessage AddAuth(HttpRequestMessage request)
        {
            if (OAuthResponse == null)
            {
                request.AddParameter("client_id", InstagramConfig.ClientId);
            }
            else
            {
                request.AddParameter("access_token", OAuthResponse.AccessToken);
            }

            return request;
        }
    }
}
