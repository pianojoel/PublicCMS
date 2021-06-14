using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Public.Models
{
    public class SitePage
    {
        public int ID { get; set; }
        public string Title { get; set; }
        
        public List<PageComponent> PageComponents { get; set; }
        public bool IsIndex { get; set; }
        public string ImageURL { get; set; }
        public string SitePageTitleRoute => Title.Trim().Replace(' ', '-');
        public string PageGuid { get; set; }

        public string TickerSymbol1 { get; set; }
        public string TickerName1 { get; set; }
        public string TickerSymbol2 { get; set; }
        public string TickerName2 { get; set; }
        public string TickerSymbol3 { get; set; }
        public string TickerName3 { get; set; }
        public string TickerSymbol4 { get; set; }
        public string TickerName4 { get; set; }
        public string TickerSymbol5 { get; set; }
        public string TickerName5 { get; set; }

        public int ProjectID { get; set; }
        [JsonIgnore]
        public Project Project { get; set; }

        public SitePage()
        {
            TickerSymbol1 = "NYSE:GME";
            TickerName1 = "Gamestop Corporation";
            TickerSymbol2 = "NYSE:AMC";
            TickerName2 = "AMC Entertainment";
            TickerSymbol3 = "NASDAQ:MSFT";
            TickerName3 = "Microsoft Corporation";
            TickerSymbol4 = "NASDAQ:TSLA";
            TickerName4 = "Tesla Inc.";
            TickerSymbol5 = "BITSTAMP:BTCUSD";
            TickerName5 = "Bitcoin/USD";
            PageGuid = Guid.NewGuid().ToString();
        }
    }
}
