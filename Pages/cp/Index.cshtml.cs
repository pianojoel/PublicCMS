using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Public.Data;
using Public.Models;
using static Public.Models.Misc;

namespace Public.Pages.cp
{
    public class IndexModel : PageModel
    {
        private readonly Public.Data.PublicContext _ctx;

        public Project CurrentProject { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageId { get; set; }
       [BindProperty]
        public string BgColor { get; set; }
        public List<PageComponent> PageComponents { get; set; }
        [BindProperty]
        public string TextBlock { get; set; }

        public IndexModel(Public.Data.PublicContext ctx)
        {
            _ctx = ctx;
        }
        public string ImageDescription { get; set; }
        [BindProperty]
        public IFormFile UploadedImage { get; set; }
        public IList<SitePage> SitePages { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {


            var id = int.Parse(HttpContext.Session.GetString("CurrentProjectID"));
            CurrentProject = _ctx.Projects.Find(id);


            SitePages = await _ctx.SitePage
                .Include(s => s.Project)
                .Where(p => p.ProjectID == CurrentProject.ID)
                .ToListAsync();

            if(PageId == 0)
            {
                PageId = CurrentProject.Pages.FirstOrDefault(p => p.IsIndex).ID;
                return Redirect("?pageid=" + PageId);
            }

            if (PageId != 0)
            {


                var p = _ctx.SitePage.Include(p => p.PageComponents).FirstOrDefault(sp => sp.ID == PageId);
                if (p.PageComponents != null)
                {
                    PageComponents = p.PageComponents.OrderBy(pc => pc.DisplayOrder).ToList();

                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //var results = HttpContext.Request.Form.ToList();
            //results = results.Where(r => r.Key != "__RequestVerificationToken" && r.Key != "BgColor").ToList(); //__RequestVerificationToken

            // var id = int.Parse(HttpContext.Session.GetString("CurrentProjectID"));
            //CurrentProject = _ctx.Projects.Find(id);

            //SitePages = await _ctx.SitePage
            //    .Include(s => s.Project)
            //    .Where(p => p.ProjectID == CurrentProject.ID)
            //    .ToListAsync();
            //var p = _ctx.SitePage.Include(p => p.PageComponents).FirstOrDefault(sp => sp.ID == PageId);
            //p.PageComponents.Clear();
            //for (int i = 0; i < results.Count(); i++)
            //{
            //    p.PageComponents.Add(new PageComponent
            //    {
            //        Content = results[i].Value,
            //        DisplayOrder = i
                    
            //    });
            //}

            

            //await _ctx.SaveChangesAsync();

            return Redirect("?pageid=" + PageId);
        }
        public async Task<IActionResult> OnPostMoveUpAsync(int order)
        {
            var p = _ctx.SitePage.Include(p => p.PageComponents).FirstOrDefault(sp => sp.ID == PageId);
           
            p.PageComponents = p.PageComponents.OrderBy(pc => pc.DisplayOrder).ToList();
            p.PageComponents[order].DisplayOrder--;
            p.PageComponents[order-1].DisplayOrder++;

            await _ctx.SaveChangesAsync();



            return Redirect("?pageid=" + PageId);
        }
        public async Task<IActionResult> OnPostMoveDownAsync(int order)
        {
            var p = _ctx.SitePage.Include(p => p.PageComponents).FirstOrDefault(sp => sp.ID == PageId);
           
            p.PageComponents = p.PageComponents.OrderBy(pc => pc.DisplayOrder).ToList();
            p.PageComponents[order].DisplayOrder++;
            p.PageComponents[order + 1].DisplayOrder--;

            await _ctx.SaveChangesAsync();



            return Redirect("?pageid=" + PageId);
        }

        public async Task<IActionResult> OnPostRemoveAsync(int deleteid)
        {
            var p = _ctx.SitePage.Include(p => p.PageComponents).FirstOrDefault(sp => sp.ID == PageId);
            

            
            
            var toRemove = p.PageComponents.FirstOrDefault(pc => pc.ID == deleteid);
            p.PageComponents.Remove(toRemove);


            for (int i = 0; i < p.PageComponents.Count(); i++)
            {
                p.PageComponents[i].DisplayOrder = i;
                
            }
            
           

            await _ctx.SaveChangesAsync();



            return Redirect("?pageid=" + PageId);
        }

        public async Task<IActionResult> OnPostAddComponentAsync(string compType)
        {
            var p = _ctx.SitePage.Include(p => p.PageComponents).FirstOrDefault(sp => sp.ID == PageId);



            if(p.PageComponents.Count() == 0)
            {
                p.PageComponents.Add(new PageComponent
                {
                    DisplayOrder = 0,
                    Content = compType == "text" ? "I'm text" : compType == "image" ? "<a data-toggle=\"modal\" data-target=\"#exampleModalCenter\">Add Image</a>" : "Other type",
                    ComponentType = compType
                });
            }
            else
            {

            
            p.PageComponents.Add(new PageComponent
            {
                
                DisplayOrder = p.PageComponents.Max(pc => pc.DisplayOrder) + 1,
                Content = compType == "text" ? "I'm text" : compType == "image" ? "<a data-toggle=\"modal\" data-val=\"user1\" data-target=\"#exampleModalCenter\">Add Image</a>" : "Other type",
                ComponentType = compType
            });
            }




            await _ctx.SaveChangesAsync();

            return Redirect("?pageid=" + PageId);
        }

        public async Task<IActionResult> OnPostChangeBgColorAsync(int compid)
        {
            var p = _ctx.SitePage.Include(p => p.PageComponents).FirstOrDefault(sp => sp.ID == PageId);

            var pc = p.PageComponents.FirstOrDefault(pc => pc.ID == compid);

            pc.BgColor = BgColor;
           
            await _ctx.SaveChangesAsync();

            return Redirect("?pageid=" + PageId);


        }

        public async Task<IActionResult> OnPostSaveComponentAsync(int compid)
        {
            var p = _ctx.SitePage.Include(p => p.PageComponents).FirstOrDefault(sp => sp.ID == PageId);

            var pc = p.PageComponents.FirstOrDefault(pc => pc.ID == compid);

            var results = HttpContext.Request.Form.ToList();

            var newpc = results.FirstOrDefault(r => r.Key == ""+compid);
            pc.Content = newpc.Value;

            await _ctx.SaveChangesAsync();

            return Redirect("?pageid=" + PageId);


        }

        public async Task<IActionResult> OnPostSetImageAsync(int compid)
        {
            var p = _ctx.SitePage.Include(p => p.PageComponents).FirstOrDefault(sp => sp.ID == PageId);

            var pc = p.PageComponents.FirstOrDefault(pc => pc.ID == compid);
            if (UploadedImage != null)
            {
                var file = "./wwwroot/img/" + UploadedImage.FileName;
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await UploadedImage.CopyToAsync(fileStream);
                }
            }
            pc.ImageURL = UploadedImage != null ? UploadedImage.FileName : "";

            await _ctx.SaveChangesAsync();

            return Redirect("?pageid=" + PageId);
        }  

    }
}
