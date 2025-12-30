using System.Text.Json.Serialization;

namespace ApplicationCore.Models
{
    public class PageList<T>
    {
        public PageList(List<T> items, int page, int pageSize, int totalCount)
        {
            this.Items = items;
            this.Page = page;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
        }

        public List<T> Items { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public bool HasNextPage => this.Page > 0 ? this.Page * PageSize < TotalCount : false;

        [JsonPropertyName("hasPreviousPage")]
        public bool HasPrevPage => Page > 1;
    }
}
