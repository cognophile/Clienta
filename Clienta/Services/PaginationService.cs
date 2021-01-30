using Clienta.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clienta.Services
{
    public class PaginationService : IPaginationService
    {
        private int _pageSize = 10;
        private readonly int PageLimit = 20;

        public int Page { get; set; } = 1;
        public int PageSize {
            get { return _pageSize; } 
            set { _pageSize = Math.Min(PageLimit, value); } 
        }

        /// <summary>
        /// Calculate the number of pages of items
        /// </summary>
        /// <param name="itemCount">The count of items</param>
        /// <returns>int - Represents the number of pages required for the given items</returns>
        public int GetPageCount(int itemCount)
        {
            if (itemCount <= 1)
            {
                return 1;
            }

            return (itemCount + PageSize - 1) / PageSize;
        }

        /// <summary>
        /// Calculate the previous page index
        /// </summary>
        /// <param name="currentPage">Index of the current page</param>
        /// <returns>int - Represents the index of the previous page, if available</returns>
        public int GetPreviousPageIndex(int currentPage)
        {
            if (currentPage <= 1)
            {
                return 0;
            }

            return currentPage - 1;
        }

        /// <summary>
        /// Calculate the next page index
        /// </summary>
        /// <param name="currentPage">Index of the current page</param>
        /// <returns>int - Represents the index of the next page, if available</returns>
        public int GetNextPageIndex(int currentPage)
        {
            if (currentPage < 1)
            {
                return Page;
            }
            
            if (currentPage >= PageLimit)
            {
                return PageLimit;
            }
            
            return currentPage + 1;
        }

        /// <summary>
        /// Determine whether a previous page should be accessible
        /// </summary>
        /// <param name="previousPageNumber">Index of the previous page</param>
        /// <returns>bool</returns>
        public bool IsPreviousPageAvailable(int previousPageNumber)
        {
            return (previousPageNumber >= 1);
        }

        /// <summary>
        /// Determine whether the next page should be accessible
        /// </summary>
        /// <param name="nextPageNumber">Represents the index of the next page</param>
        /// <param name="pageCount">Represents the total page count</param>
        /// <returns>bool</returns>
        public bool IsNextPageAvailable(int nextPageNumber, int pageCount)
        {
            if (nextPageNumber <= 1)
            {
                return false;
            }

            return (nextPageNumber <= pageCount);
        }
    }
}
