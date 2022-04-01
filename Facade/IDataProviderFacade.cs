using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facade
{
    public interface IDataProviderFacade
    {
        Task<object> Get(int id);

        Task<IEnumerable<object>> GetAll();

        Task Add(object t);

        Task Update(object id);

        List<Task> AddBatch(List<object> command);
    }
}
