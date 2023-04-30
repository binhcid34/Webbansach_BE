using DACN.Core.Entity;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACN.Core.IRepository
{
    public interface IEmployeeRepository:IBaseRepository<Employee>
    {
        public IEnumerable<Employee> getEmployeeByName();

        public IEnumerable<Employee> sortByList(string listSort);

        public object filter(string filter, int pageNumber, int pageSize);

        public void insertNewEmployee(DynamicParameters parameters);

        public void updateEmployee(DynamicParameters parameters);

        public Employee getDetail(string code);

        public bool checkExtendCode(string code, string email);

        public void deleteEmployee(string ID);


    }
}
