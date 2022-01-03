using DManage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace DManage.Controllers.InventoryManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class Inventory : ControllerBase
    {

        private readonly DManageContext dmanageContext;
        private readonly ILogger<Inventory> _log;

        public Inventory(DManageContext _dmanageContext, ILogger<Inventory> log)
        {
            dmanageContext = _dmanageContext;
            _log = log;
        }

        [HttpGet]
        [Route("pallates")]
        public async Task<IActionResult> PallateDetails()
        {
            var m = await dmanageContext.Pallates.ToListAsync();
            return Ok(m);
        }

        [HttpGet]
        [Route("products")]
        public async Task<IActionResult> ProductsDetails()
        {
            var m = await dmanageContext.ProductMasters.Include(inv => inv.ProductType).Include(y => y.ProductInventories).ToListAsync();
            return Ok(m);
        }


        [HttpGet]
        [Route("ProductLocation/productID")]


        ///
        public async Task<IActionResult> ProductsLocation(Guid productID)
        {
            try
            {
                var productinfo = await dmanageContext.ProductInventories.Include(inv => inv.Pallate).ThenInclude(pallate => pallate.Node).Where(x => x.ProductId == productID).ToListAsync();

                if (productinfo.Count > 0)
                {
                    return Ok(productinfo);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            catch (Exception excep)
            {
                _log.LogError(excep.GetType().FullName);
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }


        [HttpGet]
        [Route("order-status/OrderID")]
        public async Task<IActionResult> ViewOrderStatus(Guid OrderID)
        {
            try
            {
                var order = await dmanageContext.Orders.Include(order => order.OrderNavigation).Where(x => x.OrderId == OrderID).ToListAsync();
                if (order.Count > 0)
                {
                    return Ok(order);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            catch (Exception excep)
            {
                _log.LogError(excep.GetType().FullName);
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }


        // View all the orders and their status
        [HttpGet]
        [Route("all-order-status")]
        public async Task<IActionResult> ViewAllOrderStatus()
        {
            try
            {
                var order = await dmanageContext.Orders.Include(order => order.OrderNavigation).Select(item => new orderViewModel { OrderID = item.OrderId, OrderStatus = item.OrderStatus }).ToListAsync();
                if (order.Count > 0)
                {
                    return Ok(order);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            catch (Exception excep)
            {
                _log.LogError(excep.GetType().FullName);
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }


        [HttpPost]
        [Route("movepallate/{pallateID}/nodeID")]
        public async Task<IActionResult> MovePallate([FromBody] Guid pallateID, [Required] int nodeID)
        {
            var pallate = dmanageContext.Pallates.Where(x => x.PallateId == pallateID).FirstOrDefault();
            pallate.NodeId = nodeID;
            var m = await dmanageContext.SaveChangesAsync();
            if (m > 0)
            {
                return Ok(pallate);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
