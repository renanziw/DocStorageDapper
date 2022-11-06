using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocStorage.Domain
{
    [Table("documentaccess")]
    public class DocumentAccess
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentAccessId { get; set; }
        public Document Document { get; set; }
        public Group? Group { get; set; }
        public User? User { get; set; }

        public DocumentAccess() { }

        public DocumentAccess(Document document, Group group, User user)
        {
            Document = document;
            Group = group;
            User = user;
        }
    }
}
