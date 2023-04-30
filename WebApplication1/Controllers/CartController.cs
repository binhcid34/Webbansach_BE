using DACN.Core.Entity;
using DACN.Core.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using WebApplication1.Filter;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(UserAuthorizationFilterAttribute))]

    public class CartController : ControllerBase
    {
        private readonly IOrderRepository _iOrderRepository;
        public CartController(IOrderRepository iOrderRepository)
        {
            _iOrderRepository = iOrderRepository;
        }   
        [HttpGet]
        public IActionResult GetItems()
        {
            var res = new ResponseModel();
            var SSID = HttpContext.Session.GetString("SSID").ToString();
            res.Data = _iOrderRepository.GetItems(SSID);
            res.Success = true;
            return Ok(res);

        }
        [HttpPost]
        public IActionResult AddItem([FromBody] List<Product> orderItems)
        {
            var res = new ResponseModel();
            var SSID = HttpContext.Session.GetString("SSID").ToString();
            res.Data = _iOrderRepository.AddItems(orderItems, SSID);
            res.Success = true;
            return Ok(res);
        }

        [HttpPost("checkout")]
        public IActionResult Checkout([FromBody] SessionOrder sessionOrder)
        {
            var res = new ResponseModel();
            var SSID = HttpContext.Session.GetString("SSID").ToString();
            res.Data = _iOrderRepository.Checkout(sessionOrder, SSID);
            if(res.Data)
            {
                res.Success = true;
            }
            return Ok(res);
        }

        [HttpGet("history")]
        public IActionResult GetHistory()
        {
            var res = new ResponseModel();
            var SSID = HttpContext.Session.GetString("SSID").ToString();
            res.Data = _iOrderRepository.GetAllOrdersByUserId(SSID);
            res.Success = true;
            return Ok(res);
        }

        [HttpPost("applypromotion")]
        public IActionResult ApplyPromotion(string code)
        {
            var res = new ResponseModel();
            var SSID = HttpContext.Session.GetString("SSID").ToString();
            res.Data = _iOrderRepository.ApplyPromotion(SSID, code);
            res.Success = true;
            return Ok(res);
        }
        /// <summary>
        /// cập nhật đơn hàng: 
        /// </summary>
        /// <param name="idOrder"></param>
        /// <param name="type">1 là mua; 2 là hủy đơn hàng</param>
        /// <returns></returns>
        [HttpPut("updateQuantity/{idOrder}")]
        public IActionResult updateQuantity(string idOrder,  int type)
        {
            var res = new ResponseModel();

            // 1. Lấy orderData từ idOrder;
            SessionOrder sessionOrder = _iOrderRepository.GetOrderByID(idOrder);
            // 2. for từng orderData
            if (sessionOrder == null && sessionOrder.OrderDetail == null)
            {
                res.Success = false;
                return Ok(res);
            } 
            var orderData = JsonSerializer.Deserialize <List<Product>>(sessionOrder.OrderDetail);
            foreach (var item in orderData)
            {
                if (item != null)
                {
                    // 3. Tính lại quantity
                    var quantityProduct = item.Quantity;
                    if (quantityProduct!= null)
                    {
                        _iOrderRepository.updateQuantity(item.IdProduct, quantityProduct, type);
                    }
                }
            }

            res.Success = true;
            return Ok(res);
        }
    }
}
