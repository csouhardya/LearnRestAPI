using ApplicationCore.Models;

namespace ApplicationCore.Products.Get
{
    public class PaginationHandler<T>
    {
        /// <summary>
        /// Paginates the provided queryable sequence.
        /// </summary>
        /// <param name="query">The source queryable.</param>
        /// <param name="page">1-based page number.</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <returns>A PageList containing the requested page of items and metadata.</returns>
        public static PageList<T> Paginate(IQueryable<T> query, int page, int pageSize)
        {
            int totalCount = query.Count();
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return new PageList<T>(items, page, pageSize, totalCount); // not returning as list to follow envelop structure
        }
    }
}
