using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    internal class DatabaseCollection : System.Collections.ObjectModel.KeyedCollection<string, Database>
    {
        public DatabaseCollection() : base(System.StringComparer.OrdinalIgnoreCase)
        {
        }
        protected override string GetKeyForItem(Database item)
        {
            return item.InstanceName;
        }
    }
}
