using DACN.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACN.Core.IRepository
{
    public interface IBaseRepository<T>
    {
        public IEnumerable<T> GetAll();

        public IEnumerable<T> GetById(string id);

        public IEnumerable<T> GetByCategoryId(int idCategory);

        public void Insert(T entity);

        public void Update(T entity);

        public void Delete(T entity);

        public dynamic Count();
        object FilterProduct(int IdCategory,string NameProduct, int pageIndex, int pageSize);
        IEnumerable<T> SearchProduct(string search);
    }
}
