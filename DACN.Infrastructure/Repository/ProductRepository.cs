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
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository() : base() { }

        public void InsertProduct(Product product)
        {
            var sqlConnector = new MySqlConnection(base.connectString);

            var queryProc = $"Proc_Insert_Product";

            var parameters = new DynamicParameters();
            //parameters.Add("v_IdProduct", product.IdProduct); tự sinh GUID36 trong db
            parameters.Add("v_IdCategory", product.IdCategory);
            parameters.Add("v_NameProduct", product.NameProduct);
            parameters.Add("v_TitleProduct", product.TitleProduct);
            parameters.Add("v_DescriptionProduct", product.DescriptionProduct);
            parameters.Add("v_QuantitySock", product.QuantitySock);
            parameters.Add("v_QuantitySold", product.QuantitySold);
            parameters.Add("v_PriceProduct", product.PriceProduct);
            parameters.Add("v_ImageProduct", product.ImageProduct);
            parameters.Add("v_Author", product.Author);
            parameters.Add("v_PageNumber", product.PageNumber);
            parameters.Add("v_PublishingCompany", product.PublishingCompany);
            parameters.Add("v_DiscountSale", product.DiscountSale);




            // connect db

            // query
            sqlConnector.Query(queryProc, param: parameters, commandType: System.Data.CommandType.StoredProcedure);

        }

        public void UpdateProduct(Product product)
        {
            var sqlConnector = new MySqlConnection(base.connectString);

            var queryProc = $"Proc_Update_Product";

            var parameters = new DynamicParameters();
            parameters.Add("v_IdProduct", product.IdProduct);
            parameters.Add("v_IdCategory", product.IdCategory);
            parameters.Add("v_NameProduct", product.NameProduct);
            parameters.Add("v_TitleProduct", product.TitleProduct);
            parameters.Add("v_DescriptionProduct", product.DescriptionProduct);
            parameters.Add("v_QuantitySock", product.QuantitySock);
            parameters.Add("v_QuantitySold", product.QuantitySold);
            parameters.Add("v_PriceProduct", product.PriceProduct);
            parameters.Add("v_ImageProduct", product.ImageProduct);
            parameters.Add("v_Author", product.Author);
            parameters.Add("v_PageNumber", product.PageNumber);
            parameters.Add("v_PublishingCompany", product.PublishingCompany);
            parameters.Add("v_DiscountSale", product.DiscountSale);

            sqlConnector.Query(queryProc, param: parameters, commandType: System.Data.CommandType.StoredProcedure);

        }


        public IEnumerable<Product> SearchProduct(string search)
        {
            var sqlConnector = new MySqlConnection(base.connectString);

            var likeQuery = '%' + search + '%';
            var querySQL = $"Select * from product where product.NameProduct like '{likeQuery}' or product.TitleProduct like '{likeQuery}' or product.DescriptionProduct like '{likeQuery}'";

            var res = sqlConnector.Query<Product>(querySQL);
            return res;
        }

        public object filterByCategory(string filter, int pageNumber, int pageSize, int IdCategory)
        {
            var sqlConnector = new MySqlConnection(base.connectString);
            var parameters = new DynamicParameters();
            parameters.Add("v_filter", filter);
            parameters.Add("v_pageIndex", pageNumber);
            parameters.Add("v_pageSize", pageSize);
            parameters.Add("v_IdCategory", IdCategory);
            parameters.Add("v_TotalRecord", direction: System.Data.ParameterDirection.Output);
            parameters.Add("v_RecordStart", direction: System.Data.ParameterDirection.Output);
            parameters.Add("v_RecordEnd", direction: System.Data.ParameterDirection.Output);

            var queryProc = "Proc_Filter_ProductByCategory";
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

            var queryProc = "Proc_Filter_ProductAll";
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

        public IEnumerable<Product> getByIdCategory(int IdCategory)
        {
            var sqlConnector = new MySqlConnection(base.connectString);

            var querySQL = $"Select * from product where product.IdCategory = {IdCategory} ";

            var res = sqlConnector.Query<Product>(querySQL).ToList();
            return res;
        }

        public void DeleteProduct(string IdProduct)
        {
            var sqlConnector = new MySqlConnection(base.connectString);

            var querySQL = $"DELETE FROM product where product.IdProduct = '{IdProduct}' Limit 1 ";

            var res = sqlConnector.Query<Product>(querySQL);
        }
    }
}
