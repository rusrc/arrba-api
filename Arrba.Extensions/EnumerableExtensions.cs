using System;
using System.Collections.Generic;
using System.Linq;
using Arrba.DTO;

namespace Arrba.Extensions
{
    public static class EnumerableExtensions
    {
        public static PagedListDto<TEntity> ToPagedList<TEntity>(this IEnumerable<TEntity> entities, int pageSize, int totalItemCount, int pageNumber = 1)
        {
            var pagedList = _toPagedList<TEntity>(pageSize, totalItemCount, pageNumber);
            pagedList.Items = entities;

            return pagedList;
        }

        public static PagedListDto<TEntity> ToPagedList<TEntity>(this IQueryable<TEntity> entities, int pageSize, int pageNumber = 1)
        {
            var totalItemCount = entities.Count();
            var pagedList = _toPagedList<TEntity>(pageSize, totalItemCount, pageNumber);

            var subEntities = new List<TEntity>();
            if (totalItemCount > 0)
            {
                subEntities.AddRange(pageNumber == 1
                    ? entities.Skip(0).Take(pageSize).ToList()
                    : entities.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList()
                );
            }

            pagedList.Items = subEntities;

            return pagedList;
        }

        private static PagedListDto<TEntity> _toPagedList<TEntity>(int pageSize, int totalItemCount, int pageNumber = 1)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), pageNumber, "PageNumber cannot be below 1.");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "PageSize cannot be less than 1.");


            var pageCount = (int)Math.Ceiling((double)totalItemCount / pageSize);
            var hasNextPage = pageNumber < pageCount;
            var hasPreviousPage = pageNumber > 1;
            var isFirstPage = pageNumber == 1;
            var isLastPage = pageNumber >= pageCount;

            var firstItemOnPage = (pageNumber - 1) * pageSize + 1;
            var numberOfLastItemOnPage = firstItemOnPage + pageSize - 1;
            var lastItemOnPage = (numberOfLastItemOnPage > totalItemCount) ? totalItemCount : numberOfLastItemOnPage;

            return new PagedListDto<TEntity>
            {
                PageCount = pageCount,
                TotalItemCount = totalItemCount,
                PageSize = pageSize,
                PageNumber = pageNumber,
                HasNextPage = hasNextPage,
                HasPreviousPage = hasPreviousPage,
                IsFirstPage = isFirstPage,
                IsLastPage = isLastPage,
                FirstItemOnPage = firstItemOnPage,
                LastItemOnPage = lastItemOnPage,
                Items = null
            };
        }
    }
}
