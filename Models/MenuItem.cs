 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Public.Models
{
    public class MenuItem
    {

        public int ID { get; set; }
        public string Text { get; set; }
        public string TargetURL { get; set; }
        public int DisplayOrder { get; set; }
        public int ProjectId { get; set; } 
        public Project Project { get; set; }
        public MenuItem()
        {

        }
        public MenuItem(string targetURL, string text, int displayOrder)
        {
            TargetURL = targetURL;
            Text = text;
            DisplayOrder = displayOrder;
        }
    }
}
