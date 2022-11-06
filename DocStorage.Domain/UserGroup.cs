using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocStorage.Domain
{
    [Table("usergroup")]
    public class UserGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? UserGroupId { get; set; }
        public User User { get; set; }
        public Group Group { get; set; }

        public UserGroup() { }

        public UserGroup(User user, Group group)
        {
            User = user;
            Group = group;
        }
    }
}
