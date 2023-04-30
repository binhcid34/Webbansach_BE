using DACN.Core.Entity;
using DACN.Core.IRepository;
using DACN.Core.IService;
using DACN.Infrastructure.Repository;
using DACN.Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using WebApplication1.Filter;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[TypeFilter(typeof(UserAuthorizationFilterAttribute))]
    public class ProductController : ControllerBase
    {
        IProductRepository _IProductRepository;

        public ProductController(IProductRepository IProductRepository)
        {
            _IProductRepository = IProductRepository;
        }

        [HttpGet]
        public ResponseModel getAllProduct()
        {
            var res = new ResponseModel();

            res.Data = _IProductRepository.GetAll();

            return res;
        }

        [HttpGet("count")]

        public ResponseModel getCountProduct()
        {
            var res = new ResponseModel();

            res.Data = _IProductRepository.Count();

            return res;
        }

        [HttpGet("detail")]
        public IActionResult GetProductDetailById(string id)
        {
            var res = new ResponseModel();
            res.Data = _IProductRepository.GetById(id);
            res.Success = true;
            return Ok(res);
        }

        /// <summary>
        /// Phân trang + lọc dữ liệu sản phẩm theo trường name và title
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost("filterbycategory")]
        public ResponseModel FilterByCategory(int IdCategory, int pageNumber, int pageSize, [FromBody] string filter)
        {

            ResponseModel response = new ResponseModel();

            response.Data = _IProductRepository.filterByCategory(filter, pageNumber, pageSize, IdCategory);

            return response;
        }



        /// <summary>
        /// Phân trang + lọc dữ liệu sản phẩm theo trường name và title
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost("filter")]
        public ResponseModel Filter(int pageNumber, int pageSize, [FromBody] string filter)
        {

            ResponseModel response = new ResponseModel();

            response.Data = _IProductRepository.filter(filter, pageNumber, pageSize);

            return response;
        }
        [HttpGet("getByCategory/{IdCategory}")]
        public ResponseModel getByIdCategory(int IdCategory)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                response.Data = _IProductRepository.getByIdCategory(IdCategory);
                response.Status = 200;
                response.Success = true;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = 400;
                response.Success = false;
            }
            return response;
        }

        [HttpPost("insert")]
        public ResponseModel insertNewProduct([FromBody] Product product)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                _IProductRepository.InsertProduct(product);
                response.Status = 200;
                response.Success = true;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = 400;
                response.Success = false;
            }

            return response;
        }

        [HttpPost("update")]
        public ResponseModel updateProduct([FromBody] Product product)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                _IProductRepository.UpdateProduct(product);
                response.Status = 200;
                response.Success = true;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = 400;
                response.Success = false;
            }

            return response;
        }

        [HttpDelete("delete/{productID}")]
        public ResponseModel delete(string productID)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                _IProductRepository.DeleteProduct(productID);
                response.Status = 200;
                response.Success = true;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = 400;
                response.Success = false;
            }

            return response;
        }

    }
}
