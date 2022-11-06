using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocStorage.Model
{
    public class Document
    {
        public int? DocumentId { get; set; }
        public string Name { get; set; }   
        public string Description { get; set; } 
        public string Category { get; set; }
        public DateTime PostedDate { get; set; }
        public string Extension { get; set; }

        public string FileName
        {
            get
            {
                return $"{Name}.{Extension}";
            }
        }

        public Document() { }

        public Document(string description, string category, string filename, string fileExtension)
        {
            Description = description;
            Category = category;
            Extension = fileExtension;
            Name = filename;
            PostedDate = DateTime.Now.ToUniversalTime();
        }
    }
}
