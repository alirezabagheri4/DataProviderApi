using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Tools;
using DomainModel.DataModel;
using DomainModel.Interface.DataProviderRepository;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Repository
{
    public class DataProviderRepository : IDataProviderRepository
    {
        private readonly ISqlDataAccessDapper _accessDapper;
        public DataProviderRepository( ISqlDataAccessDapper accessDapper)
        {
            _accessDapper = accessDapper;
        }

        async Task<dynamic> IDataProviderRepository.Get(int id)
        {
            Task<dynamic> result;
            try
            {
                var results = await _accessDapper.LoadData<dynamic, dynamic>
                ("dbo.spUser_Get", new { Id = id });
                result = results.FirstOrDefault()!;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public Task<IEnumerable<dynamic>> GetAll()
        {
            try
            {
                return _accessDapper.LoadData<dynamic, dynamic>
                    ("dbo.spUser_GetAll", new { });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Task Add(dynamic t)
        {
            try
            {
                return _accessDapper.SaveData
                    ("dbo.spUser_Insert", new { Firstname = "user.FirstName", Lastname = "user.LastName" });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Task Update(dynamic id)
        {
            try
            {
                return _accessDapper.SaveData("dbo.spUser_Update", id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<Task> AddBach(List<DynamicObjectDO> t)
        {
            var className = t.FirstOrDefault()?.ClassNameAndId.Item1;
            var record = t.GroupBy(x => x.ClassNameAndId.Item2).Select(c=>new {c}).ToList();

            var result= new List<Task>();
            try
            {
                string useDB = "use DataProvider";
                string ifStatement = $@"IF (EXISTS (SELECT * 
                FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_SCHEMA = 'dbo'
                AND TABLE_NAME = '{className}'))";
                string begin = "Begin";

                string? insrt1 = null; string? property1 = null; string? value1 = null;
                foreach (var item in record)
                {
                    insrt1= $"Insert Into {className}";
                     property1 = "(";
                    foreach (var prop in item.c.ToList())
                    {
                        property1+= $"{prop.PropertyName},";
                    }
                    property1.Remove(property1.Length - 1);
                    property1 += ")";
                    value1 = "VALUES (";
                    foreach (var value in item.c.ToList())
                    {
                        value1 += $"{value.PropertyValue},";
                    }
                    value1.Remove(value1.Length - 1);
                    value1 += ") ";
                }

                string endIf = "END";
                string elseStr = "ELSE BEGIN";

                string? insert2 = null; string? property2 = null; string? value2 = null;
                string? soton = null;
                string? creatTable= $"CREATE TABLE [dbo].[{className}] (";
                foreach (var rec in record)
                {
                    foreach (var prop in rec.c.ToList())
                    {
                        if (prop.PropertyName.ToUpper().Equals("ID"))
                        {
                            soton += "Id INT  PRIMARY KEY ,";
                        }
                        else
                        {
                            soton += $"{prop.PropertyName} Nvarchar(50) ,";
                        }
                    }
                }

                foreach (var item in record)
                {
                    insert2 = $"Insert Into {className}";
                    property2 = "(";
                    foreach (var prop in item.c.ToList())
                    {
                        property2 += $"{prop.PropertyName},";
                    }
                    property2.Remove(property2.Length - 1);
                    property2 += ")";
                    value2 = "VALUES (";
                    foreach (var value in item.c.ToList())
                    {
                        value2 += $"{value.PropertyValue},";
                    }
                    value2.Remove(value2.Length - 1);
                    value2 += ") ";
                }

                string query = useDB + ifStatement + begin + insrt1 + property1 + value1 + endIf +
                               elseStr + insert2 + property2 + value2;

                foreach (var o in t)
                {
                    result.Add(_accessDapper.SaveData
                        ("dbo.spUser_Insert", new {Firstname = o.GetType(), Lastname = "user.LastName"}));
                }

                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
