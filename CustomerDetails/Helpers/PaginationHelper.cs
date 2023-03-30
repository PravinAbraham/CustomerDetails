using CustomerDetails.Filter;
using CustomerDetails.Wrappers;

namespace CustomerDetails.Helpers
{
    public class PaginationHelper
    {
        public static PagedResponse<List<T>> CreatePagedReponse<T>(List<T> pagedData, PaginationFilter validFilter, int totalRecords)
        {
            var response = new PagedResponse<List<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize);
            var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            response.TotalPages = roundedTotalPages;
            response.TotalRecords = totalRecords;
            response.NextPage = validFilter.PageNumber < totalPages;
            response.PreviousPage = validFilter.PageNumber - 1 >= 1;
            return response;
        }
    }
}
