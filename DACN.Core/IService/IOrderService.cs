using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACN.Core.IService
{
    public interface IOrderService
    {
        public Boolean CheckOrderExist(String UserID);
    }
}
