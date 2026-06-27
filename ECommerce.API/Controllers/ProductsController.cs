using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        //Get all products
        [HttpGet]
        public async Task<ActionResult<Result<IReadOnlyList<ProductDto>>>> GetProducts(CancellationToken ct = default)
        {
            var result = await productService.GetAllProductsAsync(ct);
            return Ok(result);
        }

        //Get product by id
        [HttpGet]
        public async Task<ActionResult<Result<ProductDto>>> GetProduct(int id, CancellationToken ct = default)
        {
            var res = await productService.GetProductByIdAsync(id, ct);
            return Ok(res);
        }

        //Get all types
        [HttpGet("types")]
        public async Task<ActionResult<Result<IReadOnlyList<TypeDto>>>> GetTypes(CancellationToken ct = default)
        {
            var result = await productService.GetAllTypesAsync(ct);
            return Ok(result);
        }

        //get all brands
        [HttpGet("brands")]
        public async Task<ActionResult<Result<IReadOnlyList<TypeDto>>>> GetBrands(CancellationToken ct = default)
        {
            var result = await productService.GetAllBrandsAsync(ct);
            return Ok(result);
        }
    }
}
