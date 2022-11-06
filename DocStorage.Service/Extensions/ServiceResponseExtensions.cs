using DocStorage.Domain;
using DocStorage.Util;
using DocStorage.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocStorage.Service.Extensions
{
    public static class ServiceResponseExtensions
    {
        public static ServiceResponse<T> AddObjectValidation<T>(this ServiceResponse<T> result, object obj)
        {
            var validation = obj.Validate();
            if (validation?.Any() ?? false)
            {
                result.Errors.Add(new ServiceError(ErrorTypes.ValidationFailed, validation));
            }
            return result;
        }
    }
}
