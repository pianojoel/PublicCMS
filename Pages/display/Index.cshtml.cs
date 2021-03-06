using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
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
        public HttpClient _client = new HttpClient();
        public int Visitors { get; set; }
        public string PageName { get; set; }
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
        public async Task<IActionResult> OnGetAsync(string pagename)
        {
            PageName = pagename;
            var id = int.Parse(HttpContext.Session.GetString("CurrentProjectID"));
            CurrentProject = _ctx.Projects.Include(p => p.MenuItems).FirstOrDefault(p => p.ID == id);

            MenuItems = CurrentProject.MenuItems.OrderBy(m => m.DisplayOrder).ToList();

            SitePages = await _ctx.SitePage
                .Include(s => s.Project)
                .Where(p => p.ProjectID == CurrentProject.ID)
                .ToListAsync();

            if(PageName != null)
            {
                PageId = SitePages.FirstOrDefault(sp => sp.SitePageTitleRoute == PageName).ID;
            }

            if (PageId == 0)
            {
                PageId = CurrentProject.Pages.FirstOrDefault(p => p.IsIndex).ID;
                //return Redirect("?pageid=" + PageId);
            }

            if (PageId != 0)
            {


                var p = _ctx.SitePage.Include(p => p.PageComponents).ThenInclude(pc => pc.Columns).FirstOrDefault(sp => sp.ID == PageId);
                CurrentPage = p;
                if (p.PageComponents != null)
                {
                    PageComponents = p.PageComponents.OrderBy(pc => pc.DisplayOrder).ToList();

                }
            }

            if(CurrentPage.PageComponents.Any(pc => pc.Columns.Any(cc => cc.ComponentType == "visitorcounter")))
            {
                var requestString = "https://publiccmsvisitorcounter.azurewebsites.net/api/visitors/" + CurrentPage.PageGuid;
                Task<string> getVisitorsTask = _client.GetStringAsync(requestString);
                var getVisitorsString = await getVisitorsTask;

                Visitors = JsonSerializer.Deserialize<int>(getVisitorsString);
            }

           


            return Page();


        }
    }
}
