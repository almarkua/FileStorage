﻿using System.Collections.Generic;

namespace FileStorage.Domain
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual List<File> Files { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}