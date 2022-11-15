using Aide.ClinicalReview.Configuration;
using Aide.ClinicalReview.Service.Filter;
using Aide.ClinicalReview.Service.Services;
using Aide.ClinicalReview.Service.Wrappers;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace Aide.ClinicalReview.Service.Controllers
{
    /// <summary>
    /// Base Api Controller.
    /// </summary>
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        private readonly IOptions<AideClinicalReviewServiceOptions> _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiControllerBase"/> class.
        /// </summary>
        /// <param name="options">Workflow manager options.</param>
        public ApiControllerBase(IOptions<AideClinicalReviewServiceOptions> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Gets internal Server Error 500.
        /// </summary>
        public static int InternalServerError => (int)HttpStatusCode.InternalServerError;

        /// <summary>
        /// Gets bad Request 400.
        /// </summary>
        public new static int BadRequest => (int)HttpStatusCode.BadRequest;

        /// <summary>
        /// Gets notFound 404.
        /// </summary>
        public new static int NotFound => (int)HttpStatusCode.NotFound;

        /// <summary>
        /// Creates a pagination paged response.
        /// </summary>
        /// <typeparam name="T">Data set type.</typeparam>
        /// <param name="pagedData">Data set.</param>
        /// <param name="validFilter">Filters.</param>
        /// <param name="totalRecords">Total records.</param>
        /// <param name="uriService">Uri service.</param>
        /// <param name="route">Route.</param>
        /// <returns>Returns <see cref="PagedResponse{T}"/>.</returns>
        public PagedResponse<List<T>> CreatePagedReponse<T>(List<T> pagedData, PaginationFilter validFilter, long totalRecords, IUriService uriService, string route)
        {
            Guard.Against.Null(pagedData);
            Guard.Against.Null(validFilter);
            Guard.Against.Null(route);
            Guard.Against.Null(uriService);

            var pageSize = validFilter.PageSize ?? _options.Value.EndpointSettings.DefaultPageSize;
            var respose = new PagedResponse<List<T>>(pagedData, validFilter.PageNumber, pageSize);
            var totalPages = (double)totalRecords / pageSize;
            var roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

            respose.NextPage =
                validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
                ? uriService.GetPageUriString(new PaginationFilter(validFilter.PageNumber + 1, pageSize), route)
                : null;

            respose.PreviousPage =
                validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
                ? uriService.GetPageUriString(new PaginationFilter(validFilter.PageNumber - 1, pageSize), route)
                : null;

            respose.FirstPage = uriService.GetPageUriString(new PaginationFilter(1, pageSize), route);
            respose.LastPage = uriService.GetPageUriString(new PaginationFilter(roundedTotalPages, pageSize), route);
            respose.TotalPages = roundedTotalPages;
            respose.TotalRecords = totalRecords;

            return respose;
        }
    }
}
