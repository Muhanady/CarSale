using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CarSale.Persistence;
using CarSaleCore.Models;

namespace CarSale.Extentions {
    public static class IQueryableExtention {
        public static IQueryable<T> ApplyOrdering<T> (this IQueryable<T> query, IQueryObject queryObj,
            Dictionary<string, Expression<Func<T, object>> > columnsMap) {
            if (String.IsNullOrWhiteSpace (queryObj.SortingBy) || !columnsMap.ContainsKey (queryObj.SortingBy))
                return query;
            if (queryObj.IsSortingAscending)
                query = query.OrderBy (columnsMap[queryObj.SortingBy]);
            else
                query = query.OrderByDescending (columnsMap[queryObj.SortingBy]);
            return query;
        }
        public static IQueryable<T> ApplyPaging<T> (this IQueryable<T> query, IQueryObject queryObj) {
            if (queryObj.PageSize <= 0)
                queryObj.PageSize = 10;
            if (queryObj.Page <= 0)
                queryObj.Page = 1;
            return query.Skip ((queryObj.Page - 1) * queryObj.PageSize).Take (queryObj.PageSize);

        }
    }
}