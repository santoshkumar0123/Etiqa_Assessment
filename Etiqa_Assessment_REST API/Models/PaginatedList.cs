namespace Etiqa_Assessment_REST_API.Models
{
    public class PaginatedList<T>
    {
        public List<T> UserLists { get; }
        public int PageIndex { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public PaginatedList(List<T> userLists, int pageIndex, int totalPages)
        {
            UserLists = userLists;
            PageIndex = pageIndex;
            TotalPages = totalPages;
        }
    }
}
