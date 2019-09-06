using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AllMarkt.Data
{
    public interface IAllMarktQueryContext
    {
        Task<IEnumerable<TResult>> ExecuteStoredProcedureAsync<TResult>(string spName, params SqlParameter[] parameters)
            where TResult : class;
    }
}