using Hackathon.Instagram.Domain.EntityFramework.DataEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Instagram.Core.Models.Response
{
    public class OAuthResponse 
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public UserInfo User { get; set; }
        /// <summary>
        /// Gets or sets the access_ token.
        /// </summary>
        /// <value>
        /// The access_ token.
        /// </value>
        [JsonProperty("Access_Token")]
        public string AccessToken { get; set; }
    }
}
