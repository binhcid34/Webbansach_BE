using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACN.Core.Entity
{
    public class ResponseDashboardModel : ResponseModel
    {
        public dynamic orderDashboard { get; set; }

        public dynamic orderChart { get; set; }

        public dynamic userDashboard { get; set; }

        public dynamic productDashboard { get; set; }



    }
}
