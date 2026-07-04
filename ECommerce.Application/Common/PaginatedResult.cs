using ECommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Common
{
    public sealed class PaginatedResult<TEntity>
    {
        public PaginatedResult(int pageSize, int pageIndex, int pageCount, IReadOnlyList<TEntity> data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            PageCount = pageCount;
            Data = data;
        }

        public int PageSize { get;  }
        public int PageIndex { get; }
        public int PageCount { get; }
        public IReadOnlyList<TEntity> Data { get; }

    }
}
