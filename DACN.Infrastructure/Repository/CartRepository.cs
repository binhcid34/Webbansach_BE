using DACN.Core.Entity;
using DACN.Core.IRepository;
using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DACN.Infrastructure.Repository
{
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        public CartRepository() : base() { }

        public IEnumerable<Cart> AddToCart(string orderDeatil)
        {
            //orderDeatil : "[{idProduct,nameProduct, idCategory, quantity, price ,paymentItem},....]"
            var sqlConnector = new MySqlConnection(base.connectString);

            // Không có dấu phảy ở cuối trong string orderDetail

            List<ItemCart> listItemCart = JsonSerializer.Deserialize<List<ItemCart>>(orderDeatil);

            // TODO: tính tiền từ orderDeatail : all of totalpayment
            var totalPayment = 0;
            foreach (ItemCart item in listItemCart)
            {
                totalPayment += item.Quantity * item.Price;
            }
            var idUser = "B-1009";
            var parameters = new DynamicParameters();
            parameters.Add("v_TotalPayment", totalPayment);
            parameters.Add("v_OrderDeatil", orderDeatil);
            parameters.Add("v_IdUser", idUser);


            // Update or Insert nếu chưa có vào cart gồm orderDeatil, totalPayment, idUser
            var procSQL = "Proc_Insert_Cart";
            var res = sqlConnector.Query<Cart>(procSQL, param: parameters, commandType: System.Data.CommandType.StoredProcedure);

            return res;
        }
    }
}
