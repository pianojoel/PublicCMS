using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Public.Data;
using Public.Models;

namespace Public.Pages.cp
{
    public class EditModel : PageModel
    {
        private readonly Public.Data.PublicContext _context;

        public EditModel(Public.Data.PublicContext context)
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
           ViewData["ProjectID"] = new SelectList(_context.Projects, "ID", "ID");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SitePage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SitePageExists(SitePage.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SitePageExists(int id)
        {
            return _context.SitePage.Any(e => e.ID == id);
        }
    }
}
