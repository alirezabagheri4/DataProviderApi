using DataAccess.Tools;
using DomainModel.DataModel;
using DomainModel.Interface.DataProviderRepository;

namespace DataAccess.Repository
{
    public class DataProviderRepository : IDataProviderRepository
    {
        private readonly ISqlDataAccessDapper _accessDapper;
        public DataProviderRepository(ISqlDataAccessDapper accessDapper)
        {
            _accessDapper = accessDapper;
        }

        public List<Task> AddBach(List<DynamicObjectDO> t)
        {
            var className = t.FirstOrDefault()?.ClassNameAndId.Item1;
            var record = t.GroupBy(x => x.ClassNameAndId.Item2)
                .Select(c => new { c }).ToList();

            var result = new List<Task>();
            try
            {
                string useDB = "use DataProvider ";
                string ifStatement = $@"
                IF (EXISTS (SELECT * 
                FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_SCHEMA = 'dbo'
                AND TABLE_NAME = '{className}'))";
                string begin = "Begin ";

                string? insrt1 = " "; string? property1 = " "; string? value1 = null;

                foreach (var item in record)
                {
                    insrt1 += $"Insert Into [dbo].[{className}]";
                    property1 += "(";
                    foreach (var prop in item.c.ToList())
                    {
                        property1 += $"[{prop.PropertyName}],";
                    }
                    property1 += "[SubmitDate] ) ";
                    value1 = "VALUES (";
                    foreach (var value in item.c.ToList())
                    {
                        value1 += $" N'{value.PropertyValue}' ,";
                    }
                    value1 += $" '{DateTime.Now}' ) ";
                }

                string endIf = "END ";
                string elseStr = "ELSE BEGIN ";
                string? soton = null;
                string? creatTable = $"CREATE TABLE [dbo].[{className}] (";
                string? insert2 = ""; string? property2 = ""; string? value2 = null;
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
                            soton += $"[{prop.PropertyName}] Nvarchar(50) ,";
                        }
                    }
                    break;
                }

                soton += "[SubmitDate] [datetime2](3) NOT NULL )";

                foreach (var item in record)
                {
                    insert2 += $"Insert Into [dbo].[{className}]";
                    property2 += "(";
                    foreach (var prop in item.c.ToList())
                    {
                        property2 += $"[{prop.PropertyName}],";
                    }
                    property2 += "[SubmitDate] )";
                    value2 = "VALUES (";
                    foreach (var value in item.c.ToList())
                    {
                        value2 += $" N'{value.PropertyValue}' ,";
                    }
                    value2 += $" '{DateTime.Now}' ) ";
                }
                string endElse = "END ";
                string query = useDB + ifStatement + begin
                               + insrt1 + property1 + value1 + endIf +
                               elseStr + creatTable + soton +
                               insert2 + property2 + value2 + endElse;

                foreach (var o in t)
                {
                    result.Add(_accessDapper.SaveData(query, new { }));
                }

                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
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
    }
}
