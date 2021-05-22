using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Public.Data;
using Public.Models;

namespace Public.Pages.cp
{
    public class DeleteModel : PageModel
    {
        private readonly Public.Data.PublicContext _context;

        public DeleteModel(Public.Data.PublicContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SitePage SitePage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SitePage = await _context.SitePage
                .Include(s => s.Project).FirstOrDefaultAsync(m => m.ID == id);

            if (SitePage == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SitePage = await _context.SitePage.FindAsync(id);

            if (SitePage != null)
            {
                _context.SitePage.Remove(SitePage);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
