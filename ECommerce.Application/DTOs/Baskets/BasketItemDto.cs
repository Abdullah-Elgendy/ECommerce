using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Baskets
{
    public class BasketItemDto
    {
        [Required(ErrorMessage = "Product Id Is Required")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Product Name Is Required")]
        public string ProductName { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;

        [Range(1, double.MaxValue, ErrorMessage = "Price Must Be A Positive Number")]
        public decimal Price { get; set; }

        [Range(1, 50, ErrorMessage = "Quantity Must Be A Positive Number")]
        public int Quantity { get; set; }
    }
}
