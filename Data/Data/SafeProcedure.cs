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
        public static T ExecuteAndGetInstance<T>(Database database,string procedureName, ParameterMapper parameterMapper,RecordMapper<T> recordMapper) where T:new()
        {
            T objectInstance = new T();
            if (!ExecuteAndHydrateInstance(objectInstance, database, procedureName, parameterMapper, recordMapper))
                return default(T);
            return objectInstance;
        }
        //public static bool ExecuteAndHydrateInstance<T>(T objectInstance,Database database, string procedureName, ParameterMapper parameterMapper, RecordMapper<T> recordMapper)
        //{
        //    bool result = false;
        //    return result;
        //}
        public static bool ExecuteAndHydrateInstance<T>(T objectInstance, Database database,
        string procedureName, ParameterMapper parameterMapper, RecordMapper<T> recordMapper)
        {
            bool result = false;
            try
            {
                using (IRecordSet reader = Procdure.Execute(database, procedureName, parameterMapper))
                {
                    if (reader.Read())
                    {
                        recordMapper(reader, objectInstance);
                        result = true;
                    }
                }
            }
            catch (Exception ex) // Procedure class already wrapped all necessary data
            {
                throw ex;
            }
            return result;
        }
    }
}
