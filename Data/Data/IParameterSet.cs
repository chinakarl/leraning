using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;

namespace Data
{
    public interface IParameterSet
    {
        void AddWithValue(string key, string value);
    }
    internal class ParameterSet : Data.IParameterSet
    {
        private SqlParameterCollection pm;
        internal ParameterSet(SqlParameterCollection sqlParameterCollection)
        {

            pm = sqlParameterCollection;
        }
        /// <summary>
        /// 添加参数 addvithvalu(少个防sql注入的)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddWithValue(string key, string value)
        {
            int index = pm.IndexOf(key);
            if (index == -1)
            {
                pm.AddWithValue(key,value);
            }
            else
            {
                pm[index].Value = value;
            }
        }
    }
}
