using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Data
{
    [System.Serializable]
    public class Database
    {
        //private static readonly LogWrapper log = new LogWrapper();
        private string connectionString;
        private string connectionStringAsync;
        private long lastTimeout;
        private int timeoutCount;
        private int connectionsDenied;
        private long lastConnectionDenied;
        private int totalTimeoutCount;
        private int totalConnectionsServed;
        private int totalConnectionsDenied;
        private string exceptionLog = string.Empty;
        private object padLock = new object();
        private string instanceName;
        public ConnectivityState ConnectivityState
        {
            get
            {
                if (this.timeoutCount > 0)
                {
                    long downTicks = 1200000000L;
                    if (this.lastTimeout + downTicks > System.DateTime.Now.Ticks)
                    {
                        return ConnectivityState.Down;
                    }
                    this.ResetState();
                }
                return ConnectivityState.Up;
            }
        }
        public string InstanceName
        {
            get
            {
                return this.instanceName;
            }
        }
        internal void SetConnectionString(string connString)
        {
            this.connectionString = connString;
            this.connectionStringAsync = connString + ";async=true;";
        }
        internal Database(string instanceName)
        {
            this.instanceName = instanceName;
            try 
            {
                //this.SetConnectionString(ConnectionStringProvider.GetConnectionString(instanceName));
                this.SetConnectionString("Data Source = test;Initial Catalog = myDataBase;User Id = sa;Password = 123123;");
            }
            catch
            {
                //throw new DatabaseNotConfiguredException(instanceName);
            }
        }
        private Database()
        {
        }
        public void RegisterSqlTimeout(System.Exception e)
        {
            this.exceptionLog = string.Concat(new string[]
            {
            e.ToString(),
            " : ",
            e.Message,
            " : ",
            e.StackTrace
            });
            if ((e is System.InvalidOperationException && e.Message.StartsWith("Timeout expired.")) || (e.InnerException != null && e.InnerException is SqlException && (e.Message.StartsWith("SQL Server does not exist or access denied.") || e.InnerException.Message.StartsWith("An error has occurred while establishing a connection to the server."))) || (e is SqlException && (e.Message.StartsWith("SQL Server does not exist or access denied.") || e.Message.StartsWith("An error has occurred while establishing a connection to the server."))))
            {
                lock (this.padLock)
                {
                    this.lastTimeout = System.DateTime.Now.Ticks;
                    this.timeoutCount++;
                    this.totalTimeoutCount++;
                    if (this.InstanceName.StartsWith("Shared"))
                    {
                        DatabaseManager.Instance.ResetShared();
                    }
                    else if (this.InstanceName.StartsWith("SecurityTracking"))
                    {
                        DatabaseManager.Instance.ResetSecurityTracking();
                    }
                    else
                    {
                        SqlConnection.ClearPool(new SqlConnection(this.GetConnectionString()));
                    }
                }
                //if (Database.log.get_IsErrorEnabled())
                //{
                //    Database.log.Error("Execption while RegisterSqlTimeout", e);
                //}
                //throw new DatabaseDownException("Database " + this.instanceName + " is down.");
            }
        }
        private void CheckConnectivity()
        {
            //if (ConfigurationManager.AppSettings["DatabaseConnectivityState"] == "enabled" && this.ConnectivityState == ConnectivityState.Down)
            //{
                this.connectionsDenied++;
                this.totalConnectionsDenied++;
                this.lastConnectionDenied = System.DateTime.Now.Ticks;
                //throw new DatabaseDownException("Database " + this.instanceName + " is down.");
           // }
            this.totalConnectionsServed++;
        }
        public void ResetState()
        {
            this.timeoutCount = 0;
            this.connectionsDenied = 0;
        }
        public string GetConnectionString()
        {
            return this.connectionString;
        }
        public string GetAsyncConnectionString()
        {
            return this.connectionStringAsync;
        }
        public SqlConnection GetAsyncConnection()
        {
            this.CheckConnectivity();
            return new SqlConnection(this.GetAsyncConnectionString());
        }
        public SqlConnection GetConnection()
        {
            this.CheckConnectivity();
            return new SqlConnection(this.GetConnectionString());
        }
        public SqlConnection GetOpenConnection()
        {
            this.CheckConnectivity();
            SqlConnection connection = new SqlConnection(this.GetConnectionString());
            connection.Open();
            return connection;
        }
        public static Database GetDatabase(string instanceName)
        {
            return DatabaseManager.Instance.GetDatabase(instanceName);
        }
        public static Database GetFederatedProfile(int profileUserID)
        {
            return DatabaseManager.Instance.GetFederatedProfile(profileUserID);
        }
        public static Database GetFederatedProfileByTenant(int tenantId)
        {
            return DatabaseManager.Instance.GetFederatedProfile(tenantId);
        }
        public static string GetFederatedInstanceName(int federatedId)
        {
            return Database.GetFederatedProfile(federatedId).InstanceName;
        }
        public static Database GetTenant()
        {
            return DatabaseManager.Instance.GetTenant();
        }
        public static Database GetAssessmentBasicVersionDB(System.DateTime time)
        {
            return DatabaseManager.Instance.GetAssessmentBasicVersionDB(time);
        }
        public static System.Collections.Generic.List<Database> GetDatabases()
        {
            return new System.Collections.Generic.List<Database>(DatabaseManager.Instance.Databases);
        }
        public static SqlConnection GetConnection(string instanceName)
        {
            return Database.GetDatabase(instanceName).GetConnection();
        }
        public static SqlConnection GetFederatedProfileConnection(int userID)
        {
            return Database.GetFederatedProfile(userID).GetConnection();
        }
        public static SqlConnection GetTenantConnection()
        {
            return Database.GetTenant().GetConnection();
        }
        public string GetHtmlStatus(System.Text.StringBuilder sb)
        {
            if (this.ConnectivityState == ConnectivityState.Down)
            {
                sb.Append("<dl class=\"dangerousServer\">");
            }
            else
            {
                sb.Append("<dl class=\"happyServer\">");
            }
            this.AddHeaderLine(sb, this.InstanceName);
            this.AddPropertyLine(sb, "Total Exceptions", this.totalTimeoutCount);
            this.AddPropertyLine(sb, "Total Connections Served", this.totalConnectionsServed);
            this.AddPropertyLine(sb, "Total Connections Denied", this.totalConnectionsDenied);
            if (this.timeoutCount > 0 && this.connectionsDenied > 0)
            {
                sb.Append("<hr/>");
                this.AddPropertyLine(sb, "Connections Denied Since Downed", this.connectionsDenied);
                if (this.connectionsDenied > 0)
                {
                    this.AddPropertyLine(sb, "Last Connection Denied", new System.DateTime(this.lastConnectionDenied).ToString());
                }
            }
            if (this.lastTimeout > 0L)
            {
                sb.Append("<hr/>");
                this.AddPropertyLine(sb, "Last Exception Caught", new System.DateTime(this.lastTimeout).ToString());
                sb.AppendFormat("<dd class=\"exception\">{0}</dd>", this.exceptionLog);
            }
            sb.Append("</dl>");
            return sb.ToString();
        }
        private void AddPropertyLine(System.Text.StringBuilder sb, string propName, object propValue)
        {
            if (propValue.ToString() == "0")
            {
                return;
            }
            sb.AppendFormat("<dt>{0}:</dt><dd>{1}</dd>", propName, propValue);
        }
        private void AddHeaderLine(System.Text.StringBuilder sb, object propValue)
        {
            sb.AppendFormat("<h3>{0}:</h3>", propValue);
        }
    }
    public enum ConnectivityState
    {
        Up,
        Down
    }
}
