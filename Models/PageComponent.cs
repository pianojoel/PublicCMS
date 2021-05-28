﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Public.Models
{
    public class PageComponent
    {
        public int ID { get; set; }
        public int DisplayOrder { get; set; }
        public string BgColor { get; set; }
        public string Content { get; set;}
        public int SitePageID { get; set; }
        public SitePage SitePage { get; set; }
    }
}