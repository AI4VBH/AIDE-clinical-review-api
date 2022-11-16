﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aide.ClinicalReview.Service.Filter
{
    /// <summary>
    /// Pagination Filter class.
    /// </summary>
    public sealed class PaginationFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationFilter"/> class.
        /// </summary>
        public PaginationFilter()
        {
            PageNumber = 1;
            PageSize = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationFilter"/> class.
        /// </summary>
        /// <param name="pageNumber">Page size with limit set in the config.</param>
        /// <param name="pageSize">Page size 1 or above.</param>
        /// <param name="maxPageSize">Max page size.</param>
        public PaginationFilter(int pageNumber, int pageSize, int maxPageSize = 10)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > maxPageSize ? maxPageSize : pageSize;
        }

        /// <summary>
        /// Gets or sets page number.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets page size.
        /// </summary>
        public int? PageSize { get; set; }
    }
}
