using DManage.Models;
using DManage.Repository.Services;
using DManage.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        
        private readonly Iorder _orderrepo;
        readonly ILogger<OrderController> _log;
        public OrderController(Iorder orderrepo, ILogger<OrderController> log)

        {
            _orderrepo = orderrepo;
            _log = log;
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
           var orders= await _orderrepo.GetAllOrders();
            return Ok(orders);
        }


        [HttpGet]
        [Route("{orderId}")]
        public async Task<IActionResult> GetOrd(Guid orderId)
        {
            var order = await _orderrepo.GetOrd(orderId);
            return Ok(order);
        }

        [HttpPost]
        [Route("placeOrder")]
        public async Task<IActionResult> PlaceOrder(PlaceorderView placeorderView)
        {
            Order neworder = await _orderrepo.PlaceOrder(placeorderView);
            return StatusCode(StatusCodes.Status201Created, neworder);

        }


        [HttpGet]
        [Route("verify/{orderID}")]
        public async Task<IActionResult> VerifyOrder(Guid orderID)
        {
            try
            {
                bool SpaceAvailable = await _orderrepo.VerifyOrder(orderID);
                if (SpaceAvailable == true)
                {
                    return Ok("Verification complete");
                }
                else
                {

                    return Ok("Verification rejected , no space available in pallate for this product");
                }
            }
            catch (Exception ex)
            {

                _log.LogError($"error occured {ex.Message}",ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
           
        }


        [HttpPost]
        [Route("Accept-reject-order/{orderId}")]
        public async Task<IActionResult> AcceptRejectOrder(Guid orderId)
        {

            bool result = await _orderrepo.AcceptRejectOrder(orderId);
            if (result)
            {
                return Ok($"Order :{orderId} Accepted");
            }
            else
            {
                return Ok($"Order rejected because pallate capacity is full");
            }
        }
    }
}
