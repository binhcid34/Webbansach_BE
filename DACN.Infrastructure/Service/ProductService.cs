using DACN.Core.Entity;
using DACN.Core.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACN.Infrastructure.Service
{
    public class ProductService : BaseService<Product>
    {
        public ProductService(IBaseRepository<Product> baseRepository) : base(baseRepository)
        {
        }
    }
}
