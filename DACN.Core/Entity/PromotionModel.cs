using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACN.Core.Entity
{
    public class Promotion
    {
        public string? ID {get; set;}

        public string? PromotionName { get; set;}

        public int? IsUsed { get; set;}

        public int? PromotionPercent { get; set;}

        public DateTime? CreatedDate { get; set;}


    }
}
