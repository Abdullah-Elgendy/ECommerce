using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Common
{
    public class ProductQueryParams
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? SearchValue { get; set; }
        public ProductSortingOptions Sort { get; set; }

        #region Pagination Properties
        public int PageIndex { get; set; } = 1;
        private const int DefaultPageSize = 5;
        private const int MaxPageSize = 10;

        //By default it will be DefaultPageSize, once we want to set a new value we will do so
        //through the public PageSize property with it's setter logic.
        private int pageSize = DefaultPageSize;
        //property for setting the pageSize.
        public int PageSize
        {
            get => pageSize;
            //if (value > MaxPageSize) pageSize = MaxPageSize
            //else if (pageSize < 1) pageSize = DefaultPageSize
            //else pageSize = value
            set => pageSize = value > MaxPageSize ? MaxPageSize : (value < 1 ? DefaultPageSize : value);
        } 
        #endregion
    }

}
