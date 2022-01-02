using DManage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductInventory : ControllerBase
    {
        private readonly DManageContext dmanageContext;

        public ProductInventory(DManageContext _dmanageContext)
        {
            dmanageContext = _dmanageContext;
        }

        [HttpGet]
        [Route("{productID}/check-quantity")]
        public async Task<IActionResult> CheckAvailableQuantity(Guid productID)
        {
            try
            {
                int availableQuantity = await dmanageContext.ProductInventories.Where(x => x.ProductId == productID).SumAsync(x => x.Quantity);
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
                // log ex as error

                var result = StatusCode(StatusCodes.Status500InternalServerError, ex);
                return result;

            }
        }
    }
}
