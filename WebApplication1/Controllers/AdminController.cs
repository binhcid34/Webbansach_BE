using DACN.Core.Entity;
using DACN.Core.IRepository;
using DACN.Core.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text;
using WebApplication1.Filter;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize("ShouldContainRole")]
    [TypeFilter(typeof(AdminAuthorizationFilterAttribute))]
    public class AdminController : ControllerBase
    {
        IUserRepository _IUserRepository;
        IUserService _IUserService;
        IOrderRepository _IOrderRepository;
        public AdminController(IUserRepository IUserRepository, IUserService userService, IOrderRepository iOrderRepository)
        {
            _IUserRepository = IUserRepository;
            _IUserService = userService;
            _IOrderRepository = iOrderRepository;
        }

        /// <summary>
        ///  Lấy toàn bộ thông tin account
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseModel getAllAccount()
        {
            var response = new ResponseModel();

            try
            {
                response.Data = _IUserRepository.GetAll();
            }
            catch(Exception ex) {
                response.Message = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Thêm tài khoản
        /// </summary>
        /// <returns></returns>
        [HttpPost("insert")]
        public ResponseModel insertAccount(User newUser) {
            var response = new ResponseModel();
            try
            {
                _IUserService.InsertUser(newUser, response);

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <returns></returns>
        [HttpPost("search/{search}")]
        public ResponseModel searchByEmail(string search )
        {
            var response = new ResponseModel();
            try
            {
                response.Data  = _IUserRepository.searchByEmail(search);
                response.Success = true;
                response.Status =   200;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }
        /// <summary>
        /// Xóa nhân viên theo email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpDelete("{IdUser}")]
        public ResponseModel deleteUser(string IdUser)
        {
            var response = new ResponseModel();
            try
            {
                var SSID = HttpContext.Session.GetString("SSID").ToString();
                if (SSID == IdUser)
                {
                    response.Success = false;
                    response.Message = "Không thể xóa chính mình";
                    return response;
                }
                _IUserRepository.DeleteUser(IdUser);
                response.Message = "Đã xóa thành công";

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Xóa toàn bộ nhưng đang fix cứng là xóa 10 bản ghi , Không nên test
        /// </summary>
        /// <returns></returns>
        [HttpDelete("deleteAll")]
        public ResponseModel DeleteAll ()
        {
            var response = new ResponseModel();
            try
            {
                _IUserRepository.DeleteAll();

                response.Message = "Đã xóa thành công";

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }


        [HttpGet("getAllOrder")]
        public ResponseModel getALlOrders()
        {
            var response = new ResponseModel();
            try
            {
                response.Data = _IOrderRepository.GetAllOrder();

                response.Status = 200;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpPost("updateOrder")]
        public ResponseModel updateOrder(string idorder, int paymentSatus)
        {
            var response = new ResponseModel();
            try
            {
                 _IOrderRepository.updateSessionOrder(paymentSatus, idorder);

                response.Status = 200;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpGet("filterOrder")]
        public ResponseModel filterOrder(string filter)
        {
            var response = new ResponseModel();
            try
            {
                response.Data = _IOrderRepository.SearchByNameAndCode(filter);

                response.Status = 200;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpGet("dashboard")]
        public ResponseDashboardModel dashboard()
        {
            var response = new ResponseDashboardModel();
            try
            {
                response.orderDashboard = _IOrderRepository.dashboardOrder();
                response.orderChart = _IOrderRepository.chartOrder();
                response.userDashboard = _IOrderRepository.chartUser();
                response.productDashboard = _IOrderRepository.chartProduct();
                response.Status = 200;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = 400;
                response.Success = false;
                return response;
            }
        }

        [HttpGet("getALlPromotion")]

        public ResponseModel getALlPromotion()
        {
            var response = new ResponseModel();
            try
            {
                response.Data = _IOrderRepository.GetAllPromotion();

                response.Status = 200;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpPost("createNewPromotion/{promotionPercent}")]

        public ResponseModel createNewPromotion(int promotionPercent)
        {
            var response = new ResponseModel();
            try
            {
                var promotionName = GetRandomPassword(8);
                  _IOrderRepository.createNewPromotion(promotionName, promotionPercent);

                response.Status = 200;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }


        [HttpDelete("deletePrmotion/{ID}")]

        public ResponseModel deletePrmotion(string ID)
        {
            var response = new ResponseModel();
            try
            {
                _IOrderRepository.deletePrmotion(ID);

                response.Status = 200;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }


        private string GetRandomPassword(int length)
        {
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = rnd.Next(chars.Length);
                sb.Append(chars[index]);
            }

            return sb.ToString();
        }


    }
}
