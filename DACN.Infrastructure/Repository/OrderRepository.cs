using DACN.Core.Entity;
using DACN.Core.IRepository;
using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace DACN.Infrastructure.Repository
{
    public class OrderRepository : BaseRepository<SessionOrder>, IOrderRepository
    {
        public SessionOrder AddItems(List<Product> orderItems, string userId)
        {
            var sqlConnector = new MySqlConnection(connectString);
            try
            {
                var orderDetail = JsonSerializer.Serialize(orderItems).ToString();
                //byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(orderItems);
                //var orderDetail = System.Text.Encoding.UTF8.GetString(jsonUtf8Bytes);
                var order = GetItems(userId);
                if (CheckOrderExist(userId))
                {
                    var queryProc = "Proc_Update_SessionOrder";
                    var parameters = new DynamicParameters();
                    var afterDiscount = CalculationItem(orderItems) - CalculationItem(orderItems) * order.PromotionPercent / 100;
                    order.TotalPayment = afterDiscount;
                    order.OrderDetail = orderItems;
                    parameters.Add("v_OrderDetail", orderDetail);
                    parameters.Add("v_IdUser", userId);
                    parameters.Add("v_TotalPayment", afterDiscount);
                    sqlConnector.Query(queryProc, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
                }
                else
                {
                    var queryProc = "Proc_Insert_SessionOrder";
                    var parameters = new DynamicParameters();
                    parameters.Add("v_OrderDetail", orderDetail);
                    parameters.Add("v_IdUser", userId);
                    parameters.Add("v_DiscountCombo", 1);
                    parameters.Add("v_TotalPayment", CalculationItem(orderItems));
                    parameters.Add("v_PaymentType", 2);
                    parameters.Add("v_PaymentStatus", null);
                    parameters.Add("v_LastTime", DateTime.Today);
                    parameters.Add("v_PaymentFee", 30000);

                    sqlConnector.Query(queryProc, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
                }
                return order;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public SessionOrder GetItems(string userId)
        {
            var sqlConnector = new MySqlConnection(connectString);
            var sqlQuery = $"Select * from SessionOrder where IdUser = '{userId}' and PaymentStatus is null";
            var res = sqlConnector.Query(sqlQuery).FirstOrDefault();
            if(res != null)
            {
                var sessionOrder = new SessionOrder();
                sessionOrder.IdOrder = res.IdOrder;
                sessionOrder.PaymentStatus = res.PaymentStatus;
                sessionOrder.DiscountCombo = res.DiscountCombo; 
                sessionOrder.TotalPayment = res.TotalPayment;
                sessionOrder.PaymentType = res.PaymentType;
                sessionOrder.PaymentFee = res.PaymentFee;
                sessionOrder.PromotionPercent = res.PromotionPercent != null ? Int32.Parse(res.PromotionPercent) : 0;
                //byte[] jsonUtf8Bytes = System.Text.Encoding.UTF8.GetBytes(res.OrderDetail);
                //var utf8Reader = new Utf8JsonReader(jsonUtf8Bytes);
                //sessionOrder.OrderDetail = JsonSerializer.Deserialize<List<Product>>(ref utf8Reader)!;
                //JObject json = JObject.Parse(res.OrderDetail);
                sessionOrder.OrderDetail = JsonSerializer.Deserialize<List<Product>>(res.OrderDetail);
                sessionOrder.listImage = new List<byte[]>();
                // Lấy ảnh
                if (sessionOrder.OrderDetail != null)
                {
                    foreach(var item in sessionOrder.OrderDetail )
                    {
                        if (item != null)
                        {
                            var idProduct = item.IdProduct;
                            var sqlQueryImg = $"SELECT p.ImageProduct from product p WHERE p.IdProduct = '{idProduct}' ";
                            var ImgProduct = sqlConnector.Query<byte[]>(sqlQueryImg).FirstOrDefault();
                            sessionOrder.listImage.Add(ImgProduct);
                        }
                    }
                }
                return sessionOrder;
            }
            return null;
        }

        public bool Checkout(SessionOrder sessionOrder, string userID)
        {
            var sqlConnector = new MySqlConnection(connectString);
            int option = sessionOrder.PaymentType == 1 ? 1 : 3;
            var sqlQuery = "Update SessionOrder " +
                                   $"Set PaymentStatus = '{option}', PaymentType = '{sessionOrder.PaymentType}'" +
                                   $", FullName = '{sessionOrder.FullName}', PhoneNumber = '{sessionOrder.PhoneNumber}'" +
                                   $", Address = '{sessionOrder.Address}', Email = '{sessionOrder.Email}'" +
                                   $"Where IdUser = '{userID}' And PaymentStatus is null";
            sqlConnector.Query(sqlQuery);
            var listProduct = JsonSerializer.Deserialize<List<Product>>(sessionOrder.OrderDetail);
            foreach (var item in listProduct)
            {
                if (item != null)
                {
                    // 3. Tính lại quantity
                    var quantityProduct = item.Quantity;
                    if (quantityProduct != null)
                    {
                        updateQuantity(item.IdProduct, quantityProduct, 1);
                    }
                }
            }
            return true;
        }

        private Boolean CheckOrderExist(string userId)
        {
            var sqlConnector = new MySqlConnection(connectString);
            var sqlQuery = $"Select * from SessionOrder where IdUser = '{userId}' and PaymentStatus is null";
            var res = sqlConnector.Query(sqlQuery);
            if(res.Count() > 0)
            {
                return true;
            }
            return false;
        }

        private static float CalculationItem(List<Product> orderItems)
        {
            var totalAmount = 0;
            foreach (Product product in orderItems)
            {
                totalAmount += (product.PriceProduct - product.PriceProduct * product.DiscountSale / 100) * 
                    product.Quantity;
            }
            return totalAmount;
        }

        public IEnumerable<SessionOrder> GetAllOrder()
        {
            var sqlConnector = new MySqlConnection(connectString);
            var sqlQuery = $"Select * from SessionOrder order by LastTime";
            var res = sqlConnector.Query<SessionOrder>(sqlQuery).ToList();
            return res;
        }

        public IEnumerable<SessionOrder> SearchByNameAndCode(string searchString)
        {
            var sqlConnector = new MySqlConnection(connectString);
            var sqlQuery = $"SELECT * from sessionorder s WHERE s.OrderCode  = {searchString} OR s.Fullname LIKE '%{searchString}%'";
            var res = sqlConnector.Query<SessionOrder>(sqlQuery).ToList();
            return res;
        }

        public void updateSessionOrder(int paymentStatus, string OrderID)
        {
            var sqlConnector = new MySqlConnection(connectString);
            var sqlQuery = $"UPDATE sessionorder s SET PaymentStatus = {paymentStatus} WHERE IdOrder = '{OrderID}'";
            sqlConnector.Query<SessionOrder>(sqlQuery);
        }

        public IEnumerable<SessionOrder> GetAllOrdersByUserId(string userId)
        {
            var sqlConnector = new MySqlConnection(connectString);
            var sqlQuery = $"Select * from SessionOrder where IdUser = '{userId}' and PaymentStatus is not null";
            var res = sqlConnector.Query<SessionOrder>(sqlQuery).ToList();
            return res;
        }

        public SessionOrder ApplyPromotion(string userId, string code)
        {
            var sqlConnector = new MySqlConnection(connectString);
            var sqlQuery = $"Select * from PromotionCode where PromotionName = '{code}' and (IsUsed = 0 or IsUsed is Null)";
            var promotion = sqlConnector.Query(sqlQuery).FirstOrDefault();
            if(promotion == null)
            {
                return null;
            }
            var order = GetItems(userId);
            string promotionInfo = $"Giảm giá {promotion.PromotionPercent}% đơn hàng";
            order.TotalPayment = order.TotalPayment - order.TotalPayment * promotion.PromotionPercent / 100;
            order.PromotionPercent = promotion.PromotionPercent;
            sqlQuery = $"Update SessionOrder Set TotalPayment = {order.TotalPayment}, PromotionPercent = '{promotion.PromotionPercent}' Where IdUser = '{userId}' And PaymentStatus is null";
            sqlConnector.Query(sqlQuery);
            sqlQuery = $"Update PromotionCode Set IsUsed = 1";
            sqlConnector.Query(sqlQuery);
            return order;
        }

        public dynamic dashboardOrder()
        {
            var sqlConnector = new MySqlConnection(connectString);
            var queryProc = "Proc_Dashboard_Order";
            var res = sqlConnector.Query(queryProc, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return res;

        }

        public dynamic chartOrder()
        {
            var sqlConnector = new MySqlConnection(connectString);
            var queryProc = "Proc_Chart_Order";
            var res = sqlConnector.Query(queryProc, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return res;
        }

        public dynamic chartUser()
        {
            var sqlConnector = new MySqlConnection(connectString);
            var queryProc = "Proc_Chart_User";
            var res = sqlConnector.Query(queryProc, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return res;
        }

        public dynamic chartProduct()
        {
            var sqlConnector = new MySqlConnection(connectString);
            var queryProc = "Proc_Chart_Product";
            var res = sqlConnector.Query(queryProc, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return res;
        }

        public void updateQuantity(string idProduct, int quantity, int type)
        {
            var sqlConnector = new MySqlConnection(connectString);
            
            if (type == 1)
            {
                var sqlQuery = $"UPDATE product p SET    QuantitySold = p.QuantitySold + {quantity},    QuantitySock = p.QuantitySock - {quantity} WHERE IdProduct = '{idProduct}';";
                sqlConnector.Query<SessionOrder>(sqlQuery).FirstOrDefault();
            }
            else if (type == 2)
            {
              
                    var sqlQuery = $"UPDATE product p SET    QuantitySold = p.QuantitySold - {quantity},    QuantitySock = p.QuantitySock + {quantity} WHERE IdProduct = '{idProduct}';";
                    sqlConnector.Query<SessionOrder>(sqlQuery).FirstOrDefault();
                
            } 
        }

        public SessionOrder GetOrderByID(string idOrder)
        {
            var sqlConnector = new MySqlConnection(connectString);
            var sqlQuery = $"Select * from SessionOrder where IdOrder = '{idOrder}'";
            var res = sqlConnector.Query<SessionOrder>(sqlQuery).FirstOrDefault();
            return res;
        }

        public IEnumerable<Promotion> GetAllPromotion()
        {
            var sqlConnector = new MySqlConnection(connectString);
            var sqlQuery = $"Select * from promotioncode order by CreatedDate";
            var res = sqlConnector.Query<Promotion>(sqlQuery).ToList();
            return res;
        }

        public void createNewPromotion(string promotionName, int promotionPercent)
        {
            var sqlConnector = new MySqlConnection(connectString);
            var sqlQuery = $"INSERT promotioncode (ID, PromotionName, IsUsed, PromotionPercent, CreatedDate) VALUES (UUID(), '{promotionName}', null, {promotionPercent}, CurDate());";
            var res = sqlConnector.Query<Promotion>(sqlQuery);
        }

        public void deletePrmotion(string ID)
        {
            var sqlConnector = new MySqlConnection(connectString);
            var sqlQuery = $"DELETE FROM promotioncode WHERE ID = '{ID}' LIMIT 1";
            var res = sqlConnector.Query<Promotion>(sqlQuery);
        }
    }
}
