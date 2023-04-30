using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACN.Core.Entity
{
    public class ItemCart
    {
        public int IdProduct { get; set; }
        public string NameProduct { get; set; }
        public int IdCategory { get; set; }
        public int Quantity { get; set; }

        public int Price { get; set; }

        public int PaymentItem { get; set; }

        
        
    }
}
