using Aide.ClinicalReview.Service.Filter;

namespace Aide.ClinicalReview.Service.Services
{
    /// <summary>
    /// Uri Service.
    /// </summary>
    public interface IUriService
    {
        /// <summary>
        /// Gets Relative Uri path with filters as a string.
        /// </summary>
        /// <param name="filter">Filters.</param>
        /// <param name="route">Route.</param>
        /// <returns>Relative Uri string.</returns>
        public string GetPageUriString(PaginationFilter filter, string route);
    }
}
