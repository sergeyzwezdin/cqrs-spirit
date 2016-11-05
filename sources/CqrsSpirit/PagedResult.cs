using System;
using System.Collections.Generic;

namespace CqrsSpirit
{
    public class PagedResult<T>
    {
        public PagedResult(IEnumerable<T> items, int totalCount, int page, int pageSize)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            Items = items;
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
        }

        public IEnumerable<T> Items { get; set; }

        public int TotalCount { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}