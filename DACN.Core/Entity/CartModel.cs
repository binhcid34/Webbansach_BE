using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACN.Core.Entity
{
    public class Cart
    {
        public int IdCart { get; set; }

        public string IdUser { get; set; }
        public string CartDetail { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
