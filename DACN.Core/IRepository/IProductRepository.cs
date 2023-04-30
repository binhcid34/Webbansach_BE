using DACN.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACN.Core.IRepository
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        public void InsertProduct(Product product) { }

        public void UpdateProduct(Product product) { }

        public void DeleteProduct(string IdProduct) { }

        public object filterByCategory(string filter, int pageNumber, int pageSize, int IdCategory);

        public object filter(string filter, int pageNumber, int pageSize);


        public IEnumerable<Product> getByIdCategory(int IdCategory);
    }
}
