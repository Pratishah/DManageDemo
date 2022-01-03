using DManage.Models;
using DManage.Repository.Services;
using DManage.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemManagementController : ControllerBase
    {
        private readonly ISystemManagement _systemManagementRepository;

        readonly ILogger<SystemManagementController> _log;

        public SystemManagementController(ISystemManagement systemManagementRepository, ILogger<SystemManagementController> log)
        {
            _systemManagementRepository = systemManagementRepository;
            _log = log;
        }


        [HttpPost]
        [Route("Create/ProductType")]
        public async Task<IActionResult> ProductType([Required] string productTypeName)
        {
            try
            {
                var productType = await _systemManagementRepository.CreateProductType(productTypeName);

                return Ok(productType);

            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError);

            }
        }


        [HttpPost]
        [Route("Create/Pallate")]
        public async Task<IActionResult> CreatePallate(PallateViewModel pallate)
        {
            try
            {
                var Pallate = await _systemManagementRepository.CreatePallate(pallate);

                return Ok(Pallate);

            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError);

            }
        }


        [HttpGet]
        [Route("Edit/{pallateID}")]
        public async Task<IActionResult> EditPallateCapacity([Required] Guid pallateID, [Required] int newQuantity)
        {
            try
            {
                var Pallate = await _systemManagementRepository.EditPallateCapacity(pallateID, newQuantity);
                return Ok(Pallate);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }



        [HttpGet]
        [Route("Create/Node")]
        public async Task<IActionResult> CreateNode([Required] string nodeName, [Required] string zone)
        {
            try
            {
               Node node= await _systemManagementRepository.DefineNode(nodeName, zone);
                return Ok(node);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
