using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Instagram.Core.Models.Response
{
    /// <summary>
    /// Media Response containing a list of media and pagination
    /// </summary>
    public class MediaResponse : Response
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public Media Data { get; set; }
    }
}
