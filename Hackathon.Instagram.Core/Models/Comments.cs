﻿using System.Collections.Generic;

namespace Hackathon.Instagram.Core.Models
{
    public class Comments
    {
        /// <summary>
        /// Gets the count of comments.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count { get; set; }

        /// <summary>
        /// Gets a list of comments.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public List<Comment> Data { get; set; }
    }
}