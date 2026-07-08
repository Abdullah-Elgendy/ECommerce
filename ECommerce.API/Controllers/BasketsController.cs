using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.Baskets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{

    public class BasketsController : ApiBaseController
    {
        private readonly IBasketService _basketService;
        public BasketsController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        //GET BaseUrl/Api.Baskets/Id
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BasketDto),StatusCodes.Status200OK)]
        public async Task<ActionResult<BasketDto>> GetBasketByIdAsync(string id, CancellationToken ct = default)
        {
            var res = await _basketService.GetBasketAsync(id, ct);
            return ToActionResult(res);
        }

        //PSOT BasketUrl/Api/Baskets -> body with {BasketDto}
        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasketAsync (BasketDto basket, CancellationToken ct)
        {
            var res = await _basketService.CreateOrUpdateBasketAsync(basket, ct: ct);
            return ToActionResult(res);
        }

        //DELETE BaseUrl/Api/Baskets/Id
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<bool>> RemoveBasketAsync(string id, CancellationToken ct = default)
        {
            var res = await _basketService.DeleteBasketAsync(id, ct);
            return ToActionResult(res);
        }

    }
}
