using Hackathon.Instagram.Core.Extensions;
using Hackathon.Instagram.Core.Models.Response;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Instagram.Core
{
    public class OAuth
    {


        /// <summary>
        /// Response Type
        /// </summary>
        public enum ResponseType
        {
            /// <summary>
            /// The code
            /// </summary>
            Code,
            /// <summary>
            /// The token
            /// </summary>
            Token
        }

        /// <summary>
        /// Scope
        /// </summary>
        public enum Scope
        {
            /// <summary>
            /// basic
            /// </summary>
            Basic,
            /// <summary>
            /// public_content
            /// </summary>
            Public_Content,
            /// <summary>
            /// follower_list
            /// </summary>
            Follower_List,
            /// <summary>
            /// comments
            /// </summary>
            Comments,
            /// <summary>
            /// relationships
            /// </summary>
            Relationships,
            /// <summary>
            /// likes
            /// </summary>
            Likes
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public OAuth(InstagramConfig config)
        {
            this.Config = config;
        }

        public OAuth()
        {
            this.Config = new InstagramConfig(clientId, clientSecret, redirectUri, "");
        }

        public InstagramConfig Config { get; set; }

        static string clientId = ConfigurationManager.AppSettings["InstagramClientId"];
        static string clientSecret = ConfigurationManager.AppSettings["InstagramClientSecret"];
        static string redirectUri = ConfigurationManager.AppSettings["redirectUri"];

        public  string AuthLink()
        {

            var scopes = new List<OAuth.Scope>();
            scopes.Add(OAuth.Scope.Basic);
            scopes.Add(OAuth.Scope.Public_Content);
            scopes.Add(OAuth.Scope.Follower_List);
            scopes.Add(OAuth.Scope.Comments);
            scopes.Add(OAuth.Scope.Relationships);
            scopes.Add(OAuth.Scope.Likes);


            return AuthLink( Config.OAuthUri + "authorize", Config.ClientId, Config.RedirectUri, scopes, ResponseType.Code);
        }

        /// <summary>
        /// Authentications the link.
        /// </summary>
        /// <param name="instagramOAuthUri">The instagram o authentication URI.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="callbackUri">The callback URI.</param>
        /// <param name="scopes">The scopes.</param>
        /// <param name="responseType">Type of the response.</param>
        /// <returns>The authentication url</returns>
        public static string AuthLink(string instagramOAuthUri, string clientId, string callbackUri, List<Scope> scopes, ResponseType responseType = ResponseType.Token)
        {
            var scopesForUri = BuildScopeForUri(scopes);
            return BuildAuthUri(instagramOAuthUri, clientId, callbackUri, responseType, scopesForUri);
        }

        /// <summary>
        /// Get authentication link.
        /// </summary>
        /// <param name="config">Instagram configuration.</param>
        /// <param name="scopes">The scopes.</param>
        /// <param name="responseType">Type of the response.</param>
        /// <returns>The authentication url</returns>
        public static string AuthLink(InstagramConfig config, List<Scope> scopes, ResponseType responseType = ResponseType.Token)
        {
            var scopesForUri = BuildScopeForUri(scopes);
            return BuildAuthUri(config.OAuthUri.TrimEnd('/') + "/authorize",
                config.ClientId,
                config.RedirectUri,
                responseType,
                scopesForUri);
        }

        /// <summary>
        /// Requests the token.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>An OAuthResponse object</returns>
        public Task<OAuthResponse> RequestToken(string code)
        {
            var client = new HttpClient { BaseAddress = new Uri(Config.OAuthUri) };
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(client.BaseAddress, "access_token"));
            //HttpClient client = new HttpClient();
            //var request = new HttpRequestMessage(HttpMethod.Post, "https://api.instagram.com/oauth/access_token");
            var myParameters = string.Format("client_id={0}&client_secret={1}&grant_type={2}&redirect_uri={3}&code={4}",
                Config.ClientId.UrlEncode(),
                Config.ClientSecret.UrlEncode(),
                "authorization_code".UrlEncode(),
                Config.RedirectUri.UrlEncode(),
                code.UrlEncode());

            request.Content = new StringContent(myParameters);

            return client.ExecuteAsync<OAuthResponse>(request);

        }

        /// <summary>
        /// Build scope string for auth uri.
        /// </summary>
        /// <param name="scopes">List of scopes.</param>
        /// <returns>Comma separated scopes.</returns>
        private static string BuildScopeForUri(List<Scope> scopes)
        {
            var scope = new StringBuilder();

            foreach (var s in scopes)
            {
                if (scope.Length > 0)
                {
                    scope.Append("+");
                }
                scope.Append(s);
            }

            return scope.ToString();
        }

        /// <summary>
        /// Build authentication link.
        /// </summary>
        /// <param name="instagramOAuthUri">The instagram o authentication URI.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="callbackUri">The callback URI.</param>
        /// <param name="scopes">The scopes.</param>
        /// <param name="responseType">Type of the response.</param>
        /// <returns>The authentication uri</returns>
        private static string BuildAuthUri(string instagramOAuthUri, string clientId, string callbackUri, ResponseType responseType, string scopes)
        {
            return string.Format("{0}?client_id={1}&redirect_uri={2}&response_type={3}&scope={4}", new object[] {
                instagramOAuthUri.ToLower(),
                clientId.ToLower(),
                callbackUri,
                responseType.ToString().ToLower(),
                scopes.ToLower()
            });
        }
    }
}
