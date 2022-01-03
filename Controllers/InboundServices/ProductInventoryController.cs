using DManage.Models;
using DManage.Repository.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductInventoryController : ControllerBase
    {
        private readonly IProductInventoryRepository _ProductInventoryRepository;

        readonly ILogger<ProductInventoryController> _log;
        public ProductInventoryController(IProductInventoryRepository ProductInventoryRepository, ILogger<ProductInventoryController> log)
        {
            _ProductInventoryRepository = ProductInventoryRepository;
            _log = log;
        }

        [HttpGet]
        [Route("{productID}/check-quantity")]
        public async Task<IActionResult> CheckAvailableQuantity(Guid productID)
        {
            try
            {
                int availableQuantity = await _ProductInventoryRepository.CheckAvailableQuantity(productID);
                if (availableQuantity >= 0)
                {
                    return Ok($" Available Quantity of {productID} in Warehouse : {availableQuantity}");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError);

            }
        }
    }
}
