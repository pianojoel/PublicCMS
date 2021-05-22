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

namespace Public.Pages.cp
{
    public class IndexModel : PageModel
    {
        private readonly Public.Data.PublicContext _ctx;

        public Project CurrentProject { get; set; }
        
        public int CurrentProjectID { get; set; }
        [BindProperty]
        public string TextBlock { get; set; }

        public IndexModel(Public.Data.PublicContext ctx)
        {
            _ctx = ctx;
        }

        public IList<SitePage> SitePages { get;set; }

        public async Task OnGetAsync()
        {

           
            var id = int.Parse(HttpContext.Session.GetString("CurrentProjectID"));
            CurrentProject = _ctx.Projects.Find(id);


            SitePages = await _ctx.SitePage
                .Include(s => s.Project).ToListAsync();

            

            
        }

        public async Task OnPostAsync()
        {
            
            var id = int.Parse(HttpContext.Session.GetString("CurrentProjectID"));
            CurrentProject = _ctx.Projects.Find(id);
            
            SitePages = await _ctx.SitePage
                .Include(s => s.Project).ToListAsync();

        }
    }
}
