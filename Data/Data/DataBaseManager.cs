using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    internal class DatabaseManager
    {
        private static DatabaseManager instance = new DatabaseManager();
        private DatabaseCollection databases;
        private string sharedDbInstance = string.Empty;
        private string securityTrackingDbInstance = string.Empty;
        internal static DatabaseManager Instance
        {
            get
            {
                return DatabaseManager.instance;
            }
        }
        internal DatabaseCollection Databases
        {
            get
            {
                return this.databases;
            }
        }
        internal DatabaseManager()
        {
            this.databases = new DatabaseCollection();
           // ConnectionStringCollection.RegisterConfigChangedNotification(new System.EventHandler(this.OnConfigChanged));
        }
        private void OnConfigChanged(object sender, System.EventArgs args)
        {
            //ConnectionStringCollection collection = (ConnectionStringCollection)sender;
            //foreach (string instanceName in collection)
            //{
            //    string connString = collection.get_Item(instanceName);
            //    if (this.databases.Contains(instanceName))
            //    {
            //        Database db = this.databases[instanceName];
            //        db.SetConnectionString(connString);
            //    }
            //}
        }
        internal Database GetDatabase(string instanceName)
        {
            if (!this.databases.Contains(instanceName))
            {
                lock (this.databases)
                {
                    Database database = new Database(instanceName);
                    if (!this.databases.Contains(instanceName))
                    {
                        this.databases.Add(database);
                    }
                }
            }
            return this.databases[instanceName];
        }
        internal Database GetFederatedProfile(int userID)
        {
            //return this.GetDatabase(DatabaseInstanceNameProvider.GetFederatedProfileDataBlockDB(userID));
            return this.GetDatabase("");
        }
        internal Database GetTenant()
        {
            //return this.GetDatabase(DatabaseInstance.Tenant);
            return this.GetDatabase("");
        }
        internal Database GetAssessmentBasicVersionDB(System.DateTime time)
        {
            //return this.GetDatabase(DatabaseInstanceNameProvider.GetAssessmentBasicVersionDBString(time));
            return this.GetDatabase("");
        }
        internal void ResetShared()
        {
            this.sharedDbInstance = string.Empty;
        }
        internal void ResetSecurityTracking()
        {
            this.securityTrackingDbInstance = string.Empty;
        }
    }
}
