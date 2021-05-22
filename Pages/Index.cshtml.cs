using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Public.Data;
using Public.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Public.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PublicContext _ctx;
        //[TempData]
        //public int CurrentProjectID { get; set; }
        public Project CurrentProject { get; set; }
        public List<Project> Projects { get; set; }
        [BindProperty]
        public IFormFile UploadedImage { get; set; }
        [BindProperty]
        public Project Project { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ProjectName { get; set; } = "";
        
        
        public IndexModel(PublicContext ctx)
        {
            _ctx = ctx;
        }

        public void OnGet()
        {
           
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            
            Projects = _ctx.Projects
                .Where(p => p.OwnerID == userID)
                .ToList();
           
           

        }

       
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (UploadedImage != null)
            {
                var file = "./wwwroot/img/" + UploadedImage.FileName;
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await UploadedImage.CopyToAsync(fileStream);
                }
            }
            Project.CoverImageURL = UploadedImage != null ? UploadedImage.FileName : "";
            Project.CreatedDate = DateTime.Now;
            Project.OwnerID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _ctx.Projects.Add(Project);
            await _ctx.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostEditAsync(int ID)
        {
            CurrentProject = await _ctx.Projects.FindAsync(ID);
            //CurrentProjectID = ID;
            HttpContext.Session.SetString("CurrentProjectID", $"{ID}");
            var s = HttpContext.Session.GetString("CurrentProjectID");
            return Redirect("/cp/" + CurrentProject.ProjectNameRoute);

        }

        public async Task<IActionResult> OnPostDisplayAsync(int ID)
        {
            CurrentProject = await _ctx.Projects.FindAsync(ID);
            //CurrentProjectID = ID;
            return Redirect("/display/" + CurrentProject.ProjectNameRoute);
        }
    }
}
