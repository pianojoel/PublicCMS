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
        public List<MenuItem> MenuItems { get; set; }
        public bool EnableMenu { get; set; }
        public string MenuType { get; set; }
        public string OwnerName { get; set; }
        public string OwnerID { get; set; }
        public string CoverImageURL { get; set; }
        public string BgColor { get; set; }
        public string TextColor { get; set; }
        public string MenuBgColor { get; set; }
        public string MenuTextColor { get; set; }
        public string Font { get; set; }
        public bool EnableFooter { get; set; }
        public string FooterBgColor { get; set; }
        public string FooterTextColor { get; set; }
        public string FooterContent { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
