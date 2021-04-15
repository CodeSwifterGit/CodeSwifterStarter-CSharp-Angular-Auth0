using System;
using System.Collections.Generic;
using System.Linq;
using CodeSwifterStarter.Common.Extensions;

namespace CodeSwifterStarter.Application.DataEngine.DataEngine
{
    public class DataTableInfo<T> : DataTableResponseInfo<T> where T : class
    {
        public string SortByExpression { get; set; }
        public FilteringInfo FilteringInfo { get; set; }

        public static DataTableInfo<T> CreateInstance(bool pagingEnabled, int pageIndex = 1, int pageSize = 10)
        {
            return new()
            {
                SortByExpression = "",
                SummaryInfo = Activator.CreateInstance<T>(),
                PagingInfo = new PagingInfo
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    PagingEnabled = pagingEnabled
                }
            };
        }

        public static DataTableInfo<T> CreateInstance(string[] sortByExpression, string filterQuery,
            string[] filterParameters, int? pageIndex = null,
            int? pageSize = null)
        {
            return new()
            {
                SortByExpression = GetSortedColumns(sortByExpression).Aggregate("",
                    (current, next) => current +
                                       (next.Direction == SortDirection.Ascending
                                           ? next.FullPropertyPath
                                           : next.FullPropertyPath + " DESC")
                                       + ", ",
                    x => x.Length >= 2
                        ? x[..^2]
                        : x),
                FilteringInfo = new FilteringInfo
                {
                    Query = filterQuery,
                    Parameters = filterParameters.Select(p =>
                    {
                        if (decimal.TryParse(p, out var decValue)) return decValue;
                        if (int.TryParse(p, out var intValue)) return intValue;
                        if (bool.TryParse(p, out var boolValue)) return boolValue;
                        if (DateTime.TryParse(p, out var dateTimeValue)) return dateTimeValue;

                        return (object) p;
                    }).ToList()
                },
                PagingInfo = new PagingInfo
                {
                    PageIndex = pageIndex ?? 0,
                    PageSize = pageSize ?? 0,
                    PagingEnabled = pageSize > 0
                },
                SummaryInfo = Activator.CreateInstance<T>()
            };
        }

        private static List<SortedPropertyInfo> GetSortedColumns(string[] sortByExpression)
        {
            return sortByExpression.Select(c =>
            {
                var sortInfo = c.Split('|');

                sortInfo[0] = sortInfo[0].PascalCase();

                return new SortedPropertyInfo
                {
                    Direction = sortInfo[1] == "1" || sortInfo[1].ToLower() == "descending"
                        ? SortDirection.Descending
                        : SortDirection.Ascending,
                    FullPropertyPath = sortInfo[0]
                };
            }).ToList();
        }
    }
}