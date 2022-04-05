using DataAccess.Tools;
using DomainModel.DataModel;
using DomainModel.Interface.DataProviderRepository;

namespace DataAccess.Repository
{
    public class DataProviderRepository : IDataProviderRepository
    {
        private readonly ISqlDataAccessDapper _accessDapper;
        public DataProviderRepository(ISqlDataAccessDapper accessDapper) => _accessDapper = accessDapper;

        public List<Task> AddBach(List<DynamicObjectDO> t)
        {
            var className = t.FirstOrDefault()?.ClassNameAndId.Item1;
            var record = t.GroupBy(x => x.ClassNameAndId.Item2)
                .Select(c => new { c }).ToList();

            var result = new List<Task>();
            try
            {
                const string useDb = "use DataProvider ";
                var ifStatement = $@"
                IF (EXISTS (SELECT * 
                FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_SCHEMA = 'dbo'
                AND TABLE_NAME = '{className}'))";
                const string begin = "Begin ";
                var insert1 = " "; var property1 = " "; var value1 = " ";
                var i = 0; var s = 0; var h = 0;
                foreach (var item in record)
                {
                    if (i==0)
                    {
                        insert1 += $"Insert Into [dbo].[{className}]";
                        property1 += "(";
                        foreach (var prop in item.c.ToList())
                        {
                            property1 += $"[{prop.PropertyName}],";
                        }
                        property1 += "[SubmitDate] ) ";
                    }
                    i++;
                    switch (i)
                    {
                        case 1:
                            value1 += "VALUES (";
                            break;
                        case > 1:
                            value1 += ",(";
                            break;
                    }
                    i++;
                    var count = item.c.ToList().Count;
                    foreach (var value in item.c.ToList())
                    {
                        if (h == 0 || h % count == 0)
                            value1 += $" {value.PropertyValue} ,";
                        else
                            value1 += $" N'{value.PropertyValue}' ,";
                        h++;
                    }
                    value1 += $" '{DateTime.Now}' ) ";
                }
                value1 += ";";
                const string endIf = "END ";
                const string elseStr = "ELSE BEGIN ";
                string? soton = null;
                var createTable = $"CREATE TABLE [dbo].[{className}] (";
                var insert2 = " "; var property2 = ""; var value2 = " ";
                foreach (var rec in record)
                {
                    foreach (var prop in rec.c.ToList())
                    {
                        if (prop.PropertyName.ToUpper().Equals("ID"))
                            soton += "Id INT  PRIMARY KEY ,";
                        else
                            soton += $"[{prop.PropertyName}] Nvarchar(50) ,";
                    }
                    break;
                }
                soton += "[SubmitDate] [datetime2](3) NOT NULL )";
                foreach (var item in record)
                {
                    if (s == 0)
                    {
                        insert2 += $"Insert Into [dbo].[{className}]";
                        property2 += "(";
                        foreach (var prop in item.c.ToList())
                        {
                            property2 += $"[{prop.PropertyName}],";
                        }
                        property2 += "[SubmitDate] )";
                    }
                    s++;
                    switch (s)
                    {
                        case 1:
                            value2 += "VALUES (";
                            break;
                        case > 1:
                            value2 += ",(";
                            break;
                    }
                    s++;
                    var count = item.c.ToList().Count;
                    foreach (var value in item.c.ToList())
                    {
                        if (h == 1 || h % count == 0)
                            value2 += $" {value.PropertyValue} ,";
                        else
                            value2 += $" N'{value.PropertyValue}' ,";
                        h++;
                    }
                    value2 += $" '{DateTime.Now}' ) ";
                }
                value1 += ";";
                var endElse = "END ";
                var query = useDb + ifStatement + begin
                            + insert1 + property1 + value1 + endIf +
                            elseStr + createTable + soton +
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
