using ApplicationCore.Models;

namespace ApplicationCore.Products.Get
{
    public class PaginationHandler<T>
    {
        public static PageList<T> Paginate(IQueryable<T> query, int page, int pageSize)
        {
            int totalCount = query.Count();
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return new PageList<T>(items, page, pageSize, totalCount); // not returning as list to follow envelop structure
        }
    }
}
