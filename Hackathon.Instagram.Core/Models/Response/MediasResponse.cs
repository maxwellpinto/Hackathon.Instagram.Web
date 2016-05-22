using Hackathon.Instagram.Core.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Instagram.Core.Models.Response
{
    public class MediasResponse : Response, IPagination<Media>
    {
        /// <summary>
        /// Create a MediasResponse object
        /// </summary>
        public MediasResponse()
        {
            Data = new List<Media>();
        }
        /// <summary>
        /// Gets or sets the pagination.
        /// </summary>
        /// <value>
        /// The pagination.
        /// </value>
        public Pagination Pagination { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public List<Media> Data { get; set; }
    }
}
