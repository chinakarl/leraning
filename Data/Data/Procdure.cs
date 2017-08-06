using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.MapperDelegates;
using System.Data.SqlClient;
using System.Data;
namespace Data
{
    public class Procdure
    {
        private static CommandCache commandCache = new CommandCache();
        public static IRecordSet Execute(Database database, string procedureName, ParameterMapper parameterMapper)
        {

            SqlConnection connection = database.GetConnection();
            SqlCommand command = CommandFactory.CreateParameterMappedCommand(connection, database.InstanceName, procedureName, parameterMapper);

            //if (log.IsDebugEnabled)
            //    log.MethodDebugFormat(LOG_PREFIX, "Database: {0}, Procedure: {1}, Parameters: {2}", database.InstanceName, procedureName, DebugUtil.GetParameterString(command));

            try
            {

                command.Connection.Open();
                IRecordSet record = new DataRecord(command.ExecuteReader(CommandBehavior.CloseConnection));
                return record;
            }
            catch (Exception ex)
            {
                command.Connection.Close();

                throw new Exception();

            }
        }
    }
}
