using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Public.Data;
using Public.Models;

namespace Public.Pages
{
    public class control_panelModel : PageModel
    {
        private readonly PublicContext _ctx;
        [BindProperty(SupportsGet =true)]
        public string ProjectName { get; set; }
        public Project Project { get; set; }

        public control_panelModel(PublicContext ctx)
        {
            _ctx = ctx;
        }
        public void OnGet()
        {
            var projects = _ctx.Projects.Include(p => p.Pages).ToList();
            
            Project = projects.FirstOrDefault(p => p.ProjectNameRoute.ToLower() == ProjectName.ToLower());
           
        }
    }
}
