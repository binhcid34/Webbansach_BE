using DACN.Core.Entity;
using DACN.Core.IRepository;
using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACN.Infrastructure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public string connectString = "Server=us-cdbr-east-06.cleardb.net;User=bc388b14261edc;Password=3435ccbb;Database=heroku_8237f81fa52e4e9";

        public dynamic Count()
        {
            var sqlConnector = new MySqlConnection(connectString);
            var sqlQuery = "Select count(*) as total from " + typeof(T).Name;
            var count = sqlConnector.Query(sqlQuery);
            return count;
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            var sqlConnector = new MySqlConnection(connectString);
            var sqlQuery = "Select * from " + typeof(T).Name + "";
            var res = sqlConnector.Query<T>(sqlQuery);
            return res;
        }

        public IEnumerable<T> GetByCategoryId(int idCategory)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetById(string id)
        {
            var sqlConnector = new MySqlConnection(connectString);
            string idQuery = $"{id}";
            var sqlQuery = $"Select * from {typeof(T).Name} where Id{typeof(T).Name} = '{idQuery}'";
            var res = sqlConnector.Query<T>(sqlQuery);
            return res;
        }

        public void Insert(DynamicParameters arrParam)
        {
            var sqlConnector = new MySqlConnection(connectString);
            var sqlQuery = "Proc_Insert_" + typeof(T).Name;
            var res = sqlConnector.Query<T>(sqlQuery, arrParam, commandType: System.Data.CommandType.StoredProcedure);
            //return res;
        }

        public void Insert(T entity)
        {
            var sqlConnector = new MySqlConnection(connectString);
            var sqlQuery = "Proc_Insert_" + typeof(T).Name;
            var res = sqlConnector.Query<T>(sqlQuery, entity, commandType: System.Data.CommandType.StoredProcedure);
            //return res;
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public object FilterProduct(int IdCategory, string NameProduct, int pageIndex, int pageSize)
        {
            var sqlConnector = new MySqlConnection(connectString);
            var parameters = new DynamicParameters();
            NameProduct = '%' + NameProduct + '%';
            parameters.Add("v_IdCategory", IdCategory);
            parameters.Add("v_NameProduct", NameProduct);
            parameters.Add("v_pageIndex", pageIndex);
            parameters.Add("v_pageSize", pageSize);
            parameters.Add("v_TotalRecord", direction: System.Data.ParameterDirection.Output);
            parameters.Add("v_RecordStart", direction: System.Data.ParameterDirection.Output);
            parameters.Add("v_RecordEnd", direction: System.Data.ParameterDirection.Output);

            var queryProc = $"Proc_Filter_Product";
            var res = sqlConnector.Query<Product>(queryProc, param: parameters, commandType: System.Data.CommandType.StoredProcedure);

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

        public IEnumerable<T> SearchProduct(string search)
        {
            throw new NotImplementedException();
        }

        public object FilterEmployee( string filter, int pageIndex, int pageSize)
        {
            var sqlConnector = new MySqlConnection(connectString);
            var parameters = new DynamicParameters();
            parameters.Add("v_filter", filter);
            parameters.Add("v_pageIndex", pageIndex);
            parameters.Add("v_pageSize", pageSize);
            parameters.Add("v_TotalRecord", direction: System.Data.ParameterDirection.Output);
            parameters.Add("v_RecordStart", direction: System.Data.ParameterDirection.Output);
            parameters.Add("v_RecordEnd", direction: System.Data.ParameterDirection.Output);

            var queryProc = $"Proc_Filter_Employee";
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
    }
}
