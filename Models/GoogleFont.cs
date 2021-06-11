using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Public.Models
{
    [NotMapped]
    public class GoogleFont
    {

            public string kind { get; set; }
            public Item[] items { get; set; }
       

        public class Item
        {
            public string family { get; set; }
            public string[] variants { get; set; }
            public string[] subsets { get; set; }
            public string version { get; set; }
            public string lastModified { get; set; }
            public Files files { get; set; }
            public string category { get; set; }
            public string kind { get; set; }
        }

        public class Files
        {
            public string regular { get; set; }
            public string italic { get; set; }
        }


    }
}
