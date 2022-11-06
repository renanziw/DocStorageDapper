using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocStorage.Model
{
    public class AuthenticateUserResponse : User
    {
        public string Token { get; set; }

        public AuthenticateUserResponse(User user, string token)
        {
            UserId = user.UserId;
            UserName = user.UserName;
            Role = user.Role;
            Token = token;
        }
    }
}
