using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Public.Models
{
    public class SitePage
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public string PageBody { get; set; }
        public bool IsIndex { get; set; }
        public string ImageURL { get; set; }
        public string SitePageTitleRoute => Title.Trim().Replace(' ', '-');
        public int TimesVisited { get; set; }
        public int ProjectID { get; set; }
        public Project Project { get; set; }
    }
}
