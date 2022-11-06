using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocStorage.Util
{
    public enum ErrorTypes
    {
        UserNotFound,
        UserOrPasswordIncorrect,
        GroupNotFound,
        RelationBetweenUserAndGroupNotFound,
        DocumentAccessNotFound,
        DocumentNotFound,
        Unauthorized,
        NeedToSpecifyAtLeatOneAccessType,
        ValidationFailed,
    }
}
