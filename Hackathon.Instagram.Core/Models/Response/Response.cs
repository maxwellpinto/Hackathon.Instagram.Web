using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Instagram.Core.Models.Response
{
    public abstract class Response
    {
        /// <summary>
        /// Gets or sets the meta.
        /// </summary>
        /// <value>
        /// The meta.
        /// </value>
        public Meta Meta { get; set; }

        /// <summary>
        /// The total number of calls allowed within the 1-hour window
        /// </summary>
        public int RateLimitLimit { get; set; }

        /// <summary>
        /// The remaining number of calls available to your app within the 1-hour window
        /// </summary>
        public int RateLimitRemaining { get; set; }
    }
}
