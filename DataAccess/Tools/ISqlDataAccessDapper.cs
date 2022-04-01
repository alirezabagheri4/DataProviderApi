using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Tools
{
    public interface ISqlDataAccessDapper
    {
         Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters);
         Task  SaveData<T>(string storedProcedure, T parameters);
    }
}
