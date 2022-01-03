using DManage.Models;
using DManage.Repository.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DManage.Controllers.InventoryManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class PallateController : ControllerBase
    {


        private readonly IPallate _pallaterepo;
        readonly ILogger<PallateController> _log;
        public PallateController(IPallate pallaterepo, ILogger<PallateController> log)
        {
            _pallaterepo = pallaterepo;
            _log = log;
        }


        [HttpPost]
        [Route("update/pallate/{pallateID}/{quantity}")]
        public async Task<IActionResult> ModifyPallateQuantities([Required] Guid pallateID, [Required] int quantity)
        {

            var inventory = await _pallaterepo.ModifyPallateQuantities(pallateID, quantity);
            _log.LogInformation($"pallateID: {pallateID} quantity modified to {quantity}");
            return Ok(inventory);
        }


        [HttpPost]
        [Route("Move/pallate/{pallateID}/{nodeID}")]
        public async Task<IActionResult> MovePallate([Required] Guid pallateID, [Required] int nodeID)
        {
            try
            {
                var pallate = await _pallaterepo.MovePallate(pallateID, nodeID);

                if (pallate != null)
                {
                    return Ok(pallate);
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
