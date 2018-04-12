using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.ObjectModel;

namespace WebGridSampleApplication.Models
{
    public class DataBaseHelper
    {
        public static string connString = "packet size=4096;integrated security=SSPI;data source=CLYDEGAO-LAP;initial catalog=DB_Test;Connection Lifetime=15;pooling=true; Min Pool Size=5;Max Pool Size=200; Enlist=false;Application Name=Drivecam.Turnstile;Type System Version=SQL Server 2008";

        public static ObservableCollection<Product> GetProductList()
        {
            ObservableCollection<Product> productList = new ObservableCollection<Product>();
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                var result = conn.Query<Product>("select * from Products").ToList();
                conn.Close();
                result.ForEach(x=> productList.Add(x));
                return productList;
            }
        }

        public static bool UpdateProduct(Product product)
        {
            bool resultFlag = false;
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                var result = conn.Execute(@"Update Products set Name=@Name, Description=@Description,Quantity=@Quantity where Id=@Id",new { @Name = product.Name, Description = product.Description, @Quantity=product.Quantity, @Id = product.Id});
                conn.Close();
                if (result > 0)
                {
                    resultFlag = true;
                }
            }
            return resultFlag;
        }

        public static int InsertMultiple<T>(string sql, IEnumerable<T> entities, string connectionName = null) where T : class, new()
        {
            using (SqlConnection cnn = GetConnection(connectionName))
            {
                int records = 0;
                using (var trans = cnn.BeginTransaction())
                {
                    try
                    {
                        cnn.Execute(sql, entities, trans, 30, CommandType.Text);
                    }
                    catch (DataException ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                    trans.Commit();
                }
                //foreach (T entity in entities)
                //{
                //    records += cnn.Execute(sql, entity);
                //}
                return records;
            }
        }

        public static SqlConnection GetConnection(string name)
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings[name].ConnectionString);
        }
    }
}