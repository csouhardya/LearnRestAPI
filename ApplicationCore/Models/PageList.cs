using System.Text.Json.Serialization;

namespace ApplicationCore.Models
{
    /// <summary>
    /// Wrapper for paged result sets.
    /// </summary>
    public class PageList<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageList{T}"/> class.
        /// </summary>
        /// <param name="items">The items on the current page.</param>
        /// <param name="page">The current page number (1-based).</param>
        /// <param name="pageSize">The size of each page.</param>
        /// <param name="totalCount">Total number of items available.</param>
        public PageList(List<T> items, int page, int pageSize, int totalCount)
        {
            this.Items = items;
            this.Page = page;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
        }

        /// <summary>
        /// Items in the current page.
        /// </summary>
        public List<T> Items { get; }

        /// <summary>
        /// Current 1-based page number.
        /// </summary>
        public int Page { get; }

        /// <summary>
        /// Number of items per page.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Total number of items across all pages.
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// Indicates whether there is a next page.
        /// </summary>
        public bool HasNextPage => this.Page > 0 ? this.Page * PageSize < TotalCount : false;

        [JsonPropertyName("hasPreviousPage")]
        /// <summary>
        /// Indicates whether there is a previous page.
        /// </summary>
        public bool HasPrevPage => Page > 1;
    }
}
