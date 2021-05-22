using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Public.Models
{
    public class Project
    {
        public int ID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectNameRoute => ProjectName.Trim().Replace(' ', '-');
        public List<SitePage> Pages { get; set; } = new();      
        public string OwnerName { get; set; }
        public string OwnerID { get; set; }
        public string CoverImageURL { get; set; }
       
        public DateTime CreatedDate { get; set; }

    }
}
