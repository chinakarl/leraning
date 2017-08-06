using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.MapperDelegates
{
    public delegate void ParameterMapper(IParameterSet parameterSet);
    public delegate void RecordMapper<T>(IRecord record,T objectIntance);
}
