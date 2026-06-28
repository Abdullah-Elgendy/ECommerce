using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    public class ProductsController(IProductService productService) : ApiBaseController
    {
        //Get all products
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts(CancellationToken ct = default)
        {
            var result = await productService.GetAllProductsAsync(ct);
            return ToActionResult(result);
        }


        //Get product by id
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetProduct(int id, CancellationToken ct = default)
        {
            var result = await productService.GetProductByIdAsync(id, ct);
            return ToActionResult(result);
        }
        

        //Get all types
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<TypeDto>>> GetTypes(CancellationToken ct = default)
        {
            var result = await productService.GetAllTypesAsync(ct);
            return ToActionResult(result);
        }


        //Get all brands
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<BrandDto>>> GetBrands(CancellationToken ct = default)
        {
            var result = await productService.GetAllBrandsAsync(ct);
            return ToActionResult(result);
        }
    }
}
