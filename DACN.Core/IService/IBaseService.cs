using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACN.Core.IService
{
    public interface IBaseService<T>
    {
        public void Insert(T entity);

        public void Update(T entity);
    }
}
