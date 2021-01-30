using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clienta.Services.Interfaces
{
    public interface IPaginationService
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int GetPageCount(int itemCount);
        public int GetPreviousPageIndex(int currentPage);
        public int GetNextPageIndex(int currentPage);
        public bool IsPreviousPageAvailable(int previousPageNumber);
        public bool IsNextPageAvailable(int nextPageNumber, int pageCount);
    }
}
