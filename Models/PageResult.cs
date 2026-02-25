using System.Collections.Generic;

namespace GoIoFish.Models
{
    public class PageResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public List<T> Items { get; set; }

        private PageResult()
        {
        }

        public static PageResult<T> Ok(int page, int pageSize, int total, List<T> items)
        {
            return new PageResult<T>
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                Items = items,
            };
        }
    }
}