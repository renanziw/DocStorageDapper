﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocStorage.Model
{
    public class DocumentAccessCreateModel : BaseModel
    {
        public int DocumentId { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
    }
}
