using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IRecord
    {
        T Get<T>(int i);
        T GetOrDefault<T>(int i,int n);
        T Get<T>(string parameterName);
        T GetOrDefault<T>(string parameterName, T defaultName);

        T GerOrNull<T>(int fieldIndex) where T : class;
    }
}
