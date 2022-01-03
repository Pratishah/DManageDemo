using DManage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DManage.Controllers.InventoryManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class PallateController : ControllerBase
    {

        private readonly DManageContext dmanageContext;
        readonly ILogger<PallateController> _log;
        public PallateController(DManageContext dmanageContext, ILogger<PallateController> log)
        {
            _log = log;
            this.dmanageContext = dmanageContext;
        }


        [HttpPost]
        [Route("update/pallate/{pallateID}/{quantity}")]
        public async Task<IActionResult> ModifyPallateQuantities([FromBody] Guid pallateID, int quantity)
        {

            var inventory = dmanageContext.ProductInventories.Where(x => x.PallateId == pallateID).FirstOrDefault();
            inventory.Quantity = quantity;
            await dmanageContext.SaveChangesAsync();
            return Ok(inventory);
        }




        [HttpPost]
        [Route("Move/pallate/{pallateID}/{nodeID}")]
        public async Task<IActionResult> MovePallate([FromBody] Guid pallateID, int nodeID)
        {
            try
            {

                var pallate = dmanageContext.Pallates.Where(pallate => pallate.PallateId == pallateID).FirstOrDefault();
                _log.LogInformation($"Pallate {pallateID} moved from {pallate.NodeId} to new Node : {nodeID}");
                pallate.NodeId = nodeID;
                var m = await dmanageContext.SaveChangesAsync();

                if (m > 0)
                {
                    return Ok(m);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
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
