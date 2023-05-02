using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DACN.Core.Entity
{
    public class SessionOrder
    {
        public string? IdOrder { get; set; }
        //public List<Product>? OrderDetailList { get; set; }
        public string? IdUser { get; set; }
        public int Status { get; set; } //  0: chưa thanh toán, 1: Đang vận chuyển, 2: Thành công ,3: Bị lỗi
        public int DiscountCombo { get; set; }
        public float TotalPayment { get; set; }
        public int? PaymentStatus { get; set; } // 0 hoặc null: mới, 1 = chưa thanh toán, 2 : Đã thanh toán, 3 : Chờ chuyển khoản, 4: Hủy
        public int PaymentFee { get; set; } // phí vẩn chuyển
        public int PaymentType { get; set; } // cách thanh toán:// 1: chuyển tiền mặt , 2 : chuyển khoản
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }    
        public string? Address { get; set; }    
        public string? Email { get; set; }

        public dynamic? OrderDetail { get; set; }

        public string? OrderCode { get; set; }

        public DateTime LastTime { get; set; }

        public int? PromotionPercent { get; set; }

        public List<byte[]>? ListImage { get; set; }

    }
}
