using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCAttributeDemo.Models;
using Ninject;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Configuration;
using Autofac;

namespace MVCAttributeDemo.DAL
{
    public class DBManager
    {
        public static T GetAction<T>() where T : class
        {
            var service = DBActionRegistration.kernel.Get<T>();
            return service;
        }

        public static T GetActionByAutoFac<T>() where T : class
        {
            var service = (T)DBActionRegistrationByAutoFac.container.Resolve<T>();
            return service;
        }
    }


    public class DBActionRegistration
    {
        public static IKernel kernel = null;

        public static void Register()
        {
            DBActionRegister();
        }

        private static void DBActionRegister()
        {
            kernel = new StandardKernel();
            kernel.Bind<IPersonRepository>().To<PersonRepository>();
        }   
    }

    public class DBActionRegistrationByAutoFac
    {
        public static IContainer container = null;

        public static void Register()
        {
            container = DBActionRegister().Build();
        }

        private static ContainerBuilder DBActionRegister()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<PersonRepository>().As<PersonRepository>();
            return builder;
        }
    }
    public interface IPersonRepository
    {
        Person GetPersonById(int id);
        List<Person> GetPersonList();
        bool UpdatePersonInfo(Person person);
        bool DeletePerson(int id);
        bool InsertNewPerson(Person person);
        int InsertMultiple<T>(string sql, IEnumerable<T> entities, string connectionName = null) where T : class, new();
    }

    public class PersonRepository : IPersonRepository
    {
        public static string connString = "packet size=4096;integrated security=SSPI;data source=CLYDEGAO-LAP;initial catalog=DB_Test;Connection Lifetime=15;pooling=true; Min Pool Size=5;Max Pool Size=200; Enlist=false;Application Name=Drivecam.Turnstile;Type System Version=SQL Server 2008";

        public List<Person> GetPersonList()
        {
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                var result = conn.Query<Person>(
                "[dbo].[GetPeopleList]",
                 commandType: CommandType.StoredProcedure);
                //var result = conn.Query<Person>("select * from Person");
                conn.Close();
                return result.ToList();
            }
        }

        public Person GetPersonById(int id)
        {
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                var result = conn.Query<Person>(
                    "[dbo].[GetPersonInfoById]", new { Id = id },
                    commandType: CommandType.StoredProcedure
                    );
                //var result = conn.Query<Person>("select * from Person where id=@id", new {@id=id});
                conn.Close();
                return result.ToList()[0];
            }
        }

        public bool DeletePerson(int id)
        {
            bool flag = false;
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                var result = conn.Execute("[dbo].[DeletePerson]", new { @Id = id }, commandType: CommandType.StoredProcedure);
                //var result = conn.Execute(@"delete from Person where id=@id", new { @id = id });
                conn.Close();
                if (result > 0)
                {
                    flag = true;
                }
            }
            return flag;
        }

        public bool InsertNewPerson(Person person)
        {
            bool flag = false;
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                var result = conn.Execute("[dbo].[InsertNewPersonInfo]",
                            new
                            {
                                @username = person.UserName,
                                @password = person.Password,
                                @age = person.Age,
                                @registerDate = person.RegisterDate,
                                @address = person.Address
                            }, commandType: CommandType.StoredProcedure);
                //var result = conn.Execute(@"insert into Person(username, password,age,registerDate,address) values (@a, @b,@c,@d,@e)",
                //new {@a=person.UserName, @b=person.Password, @c=person.Age, @d=person.RegisterDate, @e=person.Address
                //});
                conn.Close();
                if (result > 0)
                {
                    flag = true;
                }
            }
            return flag;
        }

        public bool UpdatePersonInfo(Person person)
        {
            bool resultFlag = false;
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                var result = conn.Execute("[dbo].[UpdatePersonInfo]",
            new
            {
                @Id = person.Id,
                @username = person.UserName,
                @password = person.Password,
                @age = person.Age,
                @registerDate = person.RegisterDate,
                @address = person.Address
            }, commandType: CommandType.StoredProcedure);
                //var result = conn.Execute(@"Update Person set username=@Name, password=@password,age=@age,registerDate=@registerDate,address=@address where Id=@Id", new { @Name = person.UserName, @password = person.Password, @age = person.Age, @registerDate=person.RegisterDate, @address=person.Address, @Id = person.Id});
                conn.Close();
                if (result > 0)
                {
                    resultFlag = true;
                }
            }
            return resultFlag;
        }

        public int InsertMultiple<T>(string sql, IEnumerable<T> entities, string connectionName = null) where T : class, new()
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

        public SqlConnection GetConnection(string name)
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings[name].ConnectionString);
        }
    }
}