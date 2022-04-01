using DomainModel.Interface.DataProviderRepository;

namespace Facade
{
    public class DataProviderFacade:IDataProviderFacade
    {
        private readonly IDataProviderRepository _dataProviderRepository;
        public DataProviderFacade(IDataProviderRepository dataProviderRepository)
        {
            _dataProviderRepository = dataProviderRepository;
        }
        public async Task<dynamic> Get(int id)
        {
            Task<dynamic> result;
            try
            {
                var results = await _dataProviderRepository.Get(id);
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
                return _dataProviderRepository.GetAll();
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
                return _dataProviderRepository.Add(t);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<Task> AddBatch(List<dynamic> t)
        {
            try
            {
                return _dataProviderRepository.AddBach(t);
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
                return _dataProviderRepository.Update(id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}