using DACN.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACN.Core.IRepository
{
    public interface IDepartmentRepository:IBaseRepository<Department>
    {
        public List<Department> getAllDepartment();
    }
}
