using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Public.Models.Misc;

namespace Public.Models
{
    public class PageComponent
    {
        public int ID { get; set; }
        public int DisplayOrder { get; set; }
        public string BgColor { get; set; }
        public string Content { get; set;}
        public string ComponentType { get; set; }
        public string ImageURL { get; set; }
        public string LinkURL { get; set; }
        public string LinkText { get; set; }
        
        List<PageComponent> Columns { get; set; }
        public int SitePageID { get; set; }
        [JsonIgnore]
        public SitePage SitePage { get; set; }
    }
}
