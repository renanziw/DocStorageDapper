using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocStorage.Model
{
    public class DocumentAccess
    {
        public int? DocumentAccessId { get; set; }
        public Document Document { get; set; }
        public User User { get; set; }
        public Group Group { get; set; }
    }
}
