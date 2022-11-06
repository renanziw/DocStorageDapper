using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocStorage.Model
{
    public class UserGroup
    {
        public int? UserGroupId { get; set; }
        public User User { get; set; }
        public Group Group { get; set; }
    }
}
