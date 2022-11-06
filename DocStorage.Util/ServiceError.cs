using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocStorage.Util
{
    public class ServiceError
    {
        public ErrorTypes Type { get; }
        public string TypeName { get => Type.ToString(); }
        public object[] AdditionalData { get; }
        public ServiceError(ErrorTypes type, params object[] additionalData)
        {
            Type = type;
            AdditionalData = additionalData;
        }
    }
}
