using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Paging
{
  public static class PagingExtension
    {
        public static IQueryable<T> Paging<T>(this IQueryable<T> query, BasePaging paging)
        {
            return query.Skip(paging.SkipEntity).Take(paging.TakeEntity);
        }
    }
}
