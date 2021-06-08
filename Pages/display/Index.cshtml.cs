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
        public IList<SitePage> SitePages { get; set; }
        [ViewData]
        public List<MenuItem> MenuItems { get; set; }

        public IndexModel(PublicContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var id = int.Parse(HttpContext.Session.GetString("CurrentProjectID"));
            CurrentProject = _ctx.Projects.Include(p => p.MenuItems).FirstOrDefault(p => p.ID == id);

            MenuItems = CurrentProject.MenuItems;

            SitePages = await _ctx.SitePage
                .Include(s => s.Project)
                .Where(p => p.ProjectID == CurrentProject.ID)
                .ToListAsync();

            if (PageId == 0)
            {
                PageId = CurrentProject.Pages.FirstOrDefault(p => p.IsIndex).ID;
                return Redirect("?pageid=" + PageId);
            }

            if (PageId != 0)
            {


                var p = _ctx.SitePage.Include(p => p.PageComponents).ThenInclude(pc => pc.Columns).FirstOrDefault(sp => sp.ID == PageId);

                if (p.PageComponents != null)
                {
                    PageComponents = p.PageComponents.OrderBy(pc => pc.DisplayOrder).ToList();

                }
            }
            return Page();


        }
    }
}
