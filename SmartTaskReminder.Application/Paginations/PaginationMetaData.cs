using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTaskReminder.Application.Paginations
{
    public class PaginationMetaData
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; } 
        public int TotalPageCount { get; set; }
        public int TotalItemCount { get; set; }

        public PaginationMetaData(int pageSize, int pageNumber, int totalItemCount)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalPageCount = (int)Math.Ceiling((double)totalItemCount / pageSize);
            TotalItemCount = totalItemCount;
        }
    }

}
