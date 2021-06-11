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
            Project.BgColor = "#ffffff";
            Project.TextColor = "#000000";
            

            SitePage index = new()
            {
                IsIndex = true,
                Title = "Index",

            };
            SitePage privacy = new()
            {
                IsIndex = false,
                Title = "Privacy"
            };
            SitePage about = new()
            {
                IsIndex = false,
                Title = "About"
            };
            Project.Pages.Add(index);
            Project.Pages.Add(privacy);
            Project.Pages.Add(about);
            Project.MenuItems = new();
            Project.MenuItems.Add(new MenuItem("/index", "Home", 0));
            Project.MenuItems.Add(new MenuItem("/privacy", "Privacy", 1));
            Project.MenuItems.Add(new MenuItem("/about", "About", 2));
            Project.MenuType = "h";

            Project.FooterContent = "(c) " + DateTime.Now.Year + " " + Project.OwnerName;  

            _ctx.Projects.Add(Project);
            await _ctx.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostEditAsync(int ID)
        {
            CurrentProject = await _ctx.Projects.FindAsync(ID);
            HttpContext.Session.SetString("CurrentProjectID", $"{ID}");
           
            return Redirect("/cp/" + CurrentProject.ProjectNameRoute);

        }

        public async Task<IActionResult> OnPostDisplayAsync(int ID)
        {
            CurrentProject = await _ctx.Projects.FindAsync(ID);
            HttpContext.Session.SetString("CurrentProjectID", $"{ID}");

            return Redirect("/display/" + CurrentProject.ProjectNameRoute);
        }

        public async Task<IActionResult> OnPostDeleteProjectAsync(int deleteID)
        {
            
            CurrentProject = _ctx.Projects
                .Include(p => p.Pages)
                .ThenInclude(p => p.PageComponents)
                .ThenInclude(pc => pc.Columns)
                .FirstOrDefault(p => p.ID == deleteID);

            if (!String.IsNullOrEmpty(CurrentProject.CoverImageURL))
            {
                System.IO.File.Delete(CurrentProject.CoverImageURL);
            }
            foreach(var sp in CurrentProject.Pages)
            {
                foreach(var pc in sp.PageComponents)
                {
                    foreach(var cc in pc.Columns)
                    {
                        if (!String.IsNullOrEmpty(cc.ImageURL))
                        {
                            System.IO.File.Delete(cc.ImageURL);
                        }
                    }
                }
            }
            _ctx.Projects.Remove(CurrentProject);
            _ctx.SaveChanges();
            return Redirect("/");
        }
    }
}
