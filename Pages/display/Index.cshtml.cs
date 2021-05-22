using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Public.Data;
using Public.Models;

namespace Public.Pages.display
{
    public class IndexModel : PageModel
    {
        private readonly PublicContext _ctx; 
        public Project CurrentProject { get; set; }
       
        public int CurrentProjectID { get; set; }

        public IndexModel(PublicContext ctx)
        {
            _ctx = ctx;
        }
        public void OnGet()
        {
            var id = int.Parse(HttpContext.Session.GetString("CurrentProjectID"));
            CurrentProject = _ctx.Projects.Find(id);
           // CurrentProject = _ctx.Projects.Find(CurrentProjectID);
        }
    }
}
