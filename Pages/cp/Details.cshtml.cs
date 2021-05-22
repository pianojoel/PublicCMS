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
    public class DetailsModel : PageModel
    {
        private readonly Public.Data.PublicContext _context;

        public DetailsModel(Public.Data.PublicContext context)
        {
            _context = context;
        }

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
    }
}
