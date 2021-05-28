using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Public.Data;
using Public.Models;

namespace Public.Pages.display
{
    public class IndexModel : PageModel
    {
        private readonly PublicContext _ctx; 
        public Project CurrentProject { get; set; }
        [BindProperty(SupportsGet=true)]
        public int PageId { get; set; }
        public SitePage CurrentPage { get; set; }
        public List<PageComponent> PageComponents { get; set; }


        public IndexModel(PublicContext ctx)
        {
            _ctx = ctx;
        }
        public void OnGet()
        {
             var id = int.Parse(HttpContext.Session.GetString("CurrentProjectID"));
            CurrentProject = _ctx.Projects.Include(p => p.Pages).FirstOrDefault(p => p.ID == id);
            
            if(PageId == 0)
            {
                PageId = CurrentProject.Pages.FirstOrDefault(p => p.IsIndex).ID;
            }
            if(PageId != 0)
            {
                CurrentPage = _ctx.SitePage.Include(p => p.PageComponents).FirstOrDefault(p => p.ID == PageId);
                   
            }

           
        }
    }
}
