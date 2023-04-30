using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACN.Core.Entity
{
    public class Product
    {
        public string? IdProduct { get; set; }

        public string? NameProduct { get; set; }

        public int IdCategory { get; set; }
        public string? TitleProduct { get; set; }
        public string? Author { get; set; }
        public int PriceProduct { get; set; }
        public int QuantitySold { get; set; }
        // Số lượng trong giỏ hàng
        public int Quantity { get; set; }
        public int QuantitySock { get; set; }
        public DateTime PostDate { get; set; }
        public string? DescriptionProduct { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public byte[]? ImageProduct { get; set; }

        public string? PublishingCompany { get; set; }

        public int? PageNumber { get; set; }

        public string? NameCategory { get; set; }

        public int DiscountSale { get; set; }

    }
}
