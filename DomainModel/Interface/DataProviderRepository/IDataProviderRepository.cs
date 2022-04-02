using DomainModel.DataModel;

namespace DomainModel.Interface.DataProviderRepository
{
    public interface IDataProviderRepository
    {
        Task<dynamic> Get(int id);

        Task<IEnumerable<dynamic>> GetAll();

        Task Add(dynamic t);

        Task Update(dynamic id);

        List<Task> AddBach(List<DynamicObjectDO> t);
    }
}
