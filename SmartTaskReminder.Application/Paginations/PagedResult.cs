using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTaskReminder.Application.Paginations
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public PaginationMetaData MetaData { get; set; }

        public PagedResult(List<T> items,PaginationMetaData metaData)
        {
            Items = items;
            MetaData = metaData;
        }
    }

}
