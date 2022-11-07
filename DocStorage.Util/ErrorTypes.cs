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
        CouldNotAddUser,
        UserOrPasswordIncorrect,
        GroupNotFound,
        CouldNotAddGroup,
        RelationBetweenUserAndGroupNotFound,
        CouldNotAddUserToGroup,
        DocumentAccessNotFound,
        CouldNotAddDocumentAccess,
        DocumentNotFound,
        DocumentCouldNotBeAdded,
        Unauthorized,
        NeedToSpecifyAtLeatOneAccessType,
        ValidationFailed,
    }
}
