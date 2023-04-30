using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DACN.Core.Entity;
using DACN.Core.IRepository;
using DACN.Core.IService;

namespace DACN.Infrastructure.Service
{
    public class OrderService : BaseService<SessionOrder>, IOrderService
    {
        public OrderService(IBaseRepository<SessionOrder> baseRepository) : base(baseRepository)
        {
        }

        public bool CheckOrderExist(string UserID)
        {
            throw new NotImplementedException();
        }
    }
}
