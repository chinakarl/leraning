using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Data.MapperDelegates;
using System.Text.RegularExpressions;
namespace Data
{
    public class CommandFactory
    {
        private static CommandCache commandCache = new CommandCache();

        private static Regex forbiddenVarchars = new Regex("[^\u0009-\u00FF]", RegexOptions.Compiled);
        private static readonly string forbiddenVarcharsReplacement = "?";
        internal static SqlCommand CreateParameterMappedCommand(SqlConnection connection, string databaseInstanceName, string procedureName, ParameterMapper parameterMapper)
        {
            //if (procedureName.IndexOf("dbo", StringComparison.OrdinalIgnoreCase) == -1)
                //throw new NoDboException(connection.Database, procedureName);

            SqlCommand command = connection.CreateCommand();
            command.CommandText = procedureName;
            command.CommandType = CommandType.StoredProcedure;

            if (parameterMapper != null)
            {
                ParameterSet pSet = new ParameterSet(command.Parameters);
                parameterMapper(pSet);
            }

            ApplySecurity(command, commandCache.GetCommandCopy(connection, databaseInstanceName, procedureName).Parameters);

            //if (log.IsMoreDebugEnabled)
            //    log.MoreDebug(new StackFrame(1, true), command);
            return command;
        }
        private static void ApplySecurity(SqlCommand command, SqlParameterCollection parameterTypes)
        {
            //if (!MaintenanceConfig.FunctionIsDisabled(1, FunctionType.SecuritySafeProcedureAvoidBestFitChars))
            //{
            foreach (SqlParameter parameter in parameterTypes)
            {
                if (parameter.DbType == DbType.AnsiString)
                {
                    string parameterName = parameter.ParameterName.Replace("@", "").ToLower();
                    foreach (SqlParameter commandParameter in command.Parameters)
                    {
                        if ((commandParameter.ParameterName.Replace("@", "").ToLower() == parameterName) &&
                            ((commandParameter.Value != null) && (commandParameter.Value != DBNull.Value)))
                        {
                            commandParameter.Value = forbiddenVarchars.Replace(commandParameter.Value.ToString(), forbiddenVarcharsReplacement);
                        }
                    }
                }
            }
            //}
        }
    }
}
