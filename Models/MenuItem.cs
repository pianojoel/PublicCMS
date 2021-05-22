using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Public.Models
{
    public class MenuItem
    {

        public int ID { get; set; }
        public string ItemName { get; set; }
        public string TargetURL { get; set; }
        public int SitePageId { get; set; } 
        public SitePage SitePage { get; set; }
    }
}
