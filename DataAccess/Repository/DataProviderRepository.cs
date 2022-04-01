using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Tools;
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

        public List<Task> AddBach(List<object> t)
        {
            var result= new List<Task>();
            try
            {
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
