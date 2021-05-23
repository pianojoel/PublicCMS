using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Public.Data;
using Public.Models;

namespace Public.Pages.cp
{
    public class CreateModel : PageModel
    {
        private readonly Public.Data.PublicContext _context;
        public Project CurrentProject { get; set; }

        public CreateModel(Public.Data.PublicContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ProjectID"] = new SelectList(_context.Projects, "ID", "ID");
            return Page();
        }

        [BindProperty]
        public SitePage SitePage { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var id = int.Parse(HttpContext.Session.GetString("CurrentProjectID"));
            CurrentProject = _context.Projects.Find(id);
            SitePage.TimesVisited = 0;
            SitePage.ProjectID = CurrentProject.ID;
            SitePage.PageBody = "Hello World!";
            _context.SitePage.Add(SitePage);




            await _context.SaveChangesAsync();

            return Redirect("/cp/" + CurrentProject.ProjectNameRoute);
        }
    }
}
