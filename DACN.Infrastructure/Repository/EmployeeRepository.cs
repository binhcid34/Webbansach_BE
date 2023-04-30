using DACN.Core.Entity;
using DACN.Core.IRepository;
using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DACN.Infrastructure.Repository
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
           public EmployeeRepository() :base() { }

        public object filter(string filter, int pageNumber, int pageSize)
        {
            var sqlConnector = new MySqlConnection(base.connectString);
            var parameters = new DynamicParameters();
            parameters.Add("v_filter", filter);
            parameters.Add("v_pageIndex", pageNumber);
            parameters.Add("v_pageSize", pageSize);
            parameters.Add("v_TotalRecord", direction: System.Data.ParameterDirection.Output);
            parameters.Add("v_RecordStart", direction: System.Data.ParameterDirection.Output);
            parameters.Add("v_RecordEnd", direction: System.Data.ParameterDirection.Output);

            var queryProc = "Proc_Filter_Employee";
            var res = sqlConnector.Query<Employee>(queryProc, param: parameters, commandType: System.Data.CommandType.StoredProcedure);

            int totalRecord = parameters.Get<int>("@v_TotalRecord");
            int recordStart = parameters.Get<int>("@v_RecordStart");
            int recordEnd = parameters.Get<int>("@v_RecordEnd");

            return new
            {
                TotalRecord = totalRecord,
                RecordStart = recordStart,
                RecordEnd = recordEnd,
                Data = res
            };
        }

        public IEnumerable<Employee> getEmployeeByName()
        {
            var sqlConnector = new MySqlConnection(base.connectString);
            var sqlQuery = "Select * from Employee where Name like '%b%'";
            var res = sqlConnector.Query<Employee>(sqlQuery);
            return res;
        }

        public IEnumerable<Employee> sortByList(string listSort)
        {
            var sqlConnector = new MySqlConnection(base.connectString);
                string orderQuery = " order by " + listSort;
                var sqlQuery = "Select * from Employee " + orderQuery;
                var res = sqlConnector.Query<Employee>(sqlQuery).ToList();
                return res;
        }

        public void insertNewEmployee(DynamicParameters parameters)
        {
            var sqlConnector = new MySqlConnection(base.connectString);
            var sqlQuery = "Proc_Insert_Employee";
            sqlConnector.Query<Employee>(sqlQuery, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
        }

        public void updateEmployee(DynamicParameters parameters)
        {
            var sqlConnector = new MySqlConnection(base.connectString);
            var sqlQuery = "Proc_Update_Employee";
            sqlConnector.Query<Employee>(sqlQuery, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
        }

        public Employee getDetail(string code)
        {
            var sqlConnector = new MySqlConnection(base.connectString);
            var sqlQuery = $"Select * from Employee e where e.ID = '{code}'";
            var res = sqlConnector.Query<Employee>(sqlQuery).FirstOrDefault();
            return res;
        }


        /// <summary>
        /// nếu đã tồn tại trả về true
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool checkExtendCode(string code , string email)
        {
            var sqlConnector = new MySqlConnection(base.connectString);
            var sqlQuery = $"Select * from Employee e where e.EmployeeCode = '{code}' or e.Email = '{email}'";
            var res = sqlConnector.Query<Employee>(sqlQuery).FirstOrDefault();
            if (res != null)
            {
                return true;
            }
            else return false;
        }

        public void deleteEmployee(string ID)
        {
            var sqlConnector = new MySqlConnection(base.connectString);
            var sqlQuery = $"DELETE FROM employee  WHERE employee.ID = '{ID}' Limit 2";
            sqlConnector.Query<Employee>(sqlQuery).FirstOrDefault();
        }
    }
}
