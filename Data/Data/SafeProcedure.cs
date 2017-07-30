using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using Data.MapperDelegates;
namespace Data
{
    public class SafeProcedure
    {
        public T ExecuteAndGetInstance<T>(Database database,string procedureName, ParameterMapper parameterMapper,RecordMapper<T> recordMapper) where T:new()
        {
            T objectInstance = new T();

            return default(T);
        }
        public static bool ExecuteAndHydrateInstance<T>(T objectInstance,Database database, string procedureName, ParameterMapper parameterMapper, RecordMapper<T> recordMapper)
        {
            bool result = false;
            return result;
        }
    }
}
