using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Public.Models
{
    public class ColumnComp 
    {

        public int ID { get; set; }
        public int DisplayOrder { get; set; }
        public string BgColor { get; set; }
        public string Content { get; set; }
        public string ComponentType { get; set; }
        public string ImageURL { get; set; }
        public int ImageSize { get; set; }
        public string LinkURL { get; set; }
        public string LinkText { get; set; }
        public string MapLocation { get; set; }
        public int Mapsize { get; set; }
        public string VideoString { get; set; }
        public string PluginString { get; set; }




        public int PageComponentID { get; set; }
        [JsonIgnore]
        public PageComponent PageComponent { get; set; }
        public ColumnComp() { }
       
        public ColumnComp(int displayOrder)
        {
            ComponentType = "default";
            DisplayOrder = displayOrder;

        }
    }
}
