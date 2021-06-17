using System;
using System.Collections.Generic;
using System.IO;
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

namespace Public.Pages.cp
{
    public class IndexModel : PageModel
    {
        private readonly Public.Data.PublicContext _ctx;
        public HttpClient _client = new HttpClient();
        [TempData]
        public int LastElement { get; set; }
        public int Visitors { get; set; }
        [BindProperty]
        public string PluginString { get; set; }
        public Project CurrentProject { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageId { get; set; }
       [BindProperty]
        public string BgColor { get; set; }
        public List<PageComponent> PageComponents { get; set; }
        [BindProperty]
        public string TextBlock { get; set; }
        [BindProperty]
        public int DeletePageId { get; set; }

        public IndexModel(Public.Data.PublicContext ctx)
        {
            _ctx = ctx;
        }
        public string ImageDescription { get; set; }
        [BindProperty]
        public int ImageSize { get; set; }
        [BindProperty]
        public IFormFile UploadedImage { get; set; }
        public IList<SitePage> SitePages { get; set; }
        [BindProperty]
        public string MapLocation { get; set; }
        
        [BindProperty]
        public string TickerSymbol1 { get; set; }
        [BindProperty]
        public string TickerName1 { get; set; }
        [BindProperty]
        public string TickerSymbol2 { get; set; }
        [BindProperty]
        public string TickerName2 { get; set; }
        [BindProperty]
        public string TickerSymbol3 { get; set; }
        [BindProperty]
        public string TickerName3 { get; set; }
        [BindProperty]
        public string TickerSymbol4 { get; set; }
        [BindProperty]
        public string TickerName4 { get; set; }
        [BindProperty]
        public string TickerSymbol5 { get; set; }
        [BindProperty]
        public string TickerName5 { get; set; }
        public SitePage CurrentPage { get; set; }
        [BindProperty]
        public string VideoString { get; set; }


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


                var p = _ctx.SitePage.Include(p => p.PageComponents).ThenInclude(pc =>pc.Columns).FirstOrDefault(sp => sp.ID == PageId);
                CurrentPage = p;
                
                if (p.PageComponents != null)
                {
                    PageComponents = p.PageComponents.OrderBy(pc => pc.DisplayOrder).ToList();

                 }
            }

          



            return Page();
        }

       

        public async Task<IActionResult> OnPostMoveUpAsync(int order)
        {
            var p = _ctx.SitePage.Include(p => p.PageComponents).FirstOrDefault(sp => sp.ID == PageId);
           
            p.PageComponents = p.PageComponents.OrderBy(pc => pc.DisplayOrder).ToList();
            p.PageComponents[order].DisplayOrder--;
            p.PageComponents[order-1].DisplayOrder++;

            await _ctx.SaveChangesAsync();


            LastElement = p.PageComponents[order].ID;
            return Redirect("?pageid=" + PageId);
        }
        public async Task<IActionResult> OnPostMoveDownAsync(int order)
        {
            var p = _ctx.SitePage.Include(p => p.PageComponents).FirstOrDefault(sp => sp.ID == PageId);
           
            p.PageComponents = p.PageComponents.OrderBy(pc => pc.DisplayOrder).ToList();
            p.PageComponents[order].DisplayOrder++;
            p.PageComponents[order + 1].DisplayOrder--;

            await _ctx.SaveChangesAsync();

            LastElement = p.PageComponents[order].ID;

            return Redirect("?pageid=" + PageId);
        }

        public async Task<IActionResult> OnPostRemoveRowAsync(int deleteid)
        {
            var p = _ctx.SitePage.Include(p => p.PageComponents).ThenInclude(pc => pc.Columns).FirstOrDefault(sp => sp.ID == PageId);




            var toRemove = p.PageComponents.FirstOrDefault(pc => pc.ID == deleteid);

            foreach(var col in toRemove.Columns)
            {
                if (col.ImageURL != null)
                {
                    try
                    {
                        System.IO.File.Delete("./wwwroot/img/" + col.ImageURL);
                    }
                    catch { }
                }

               
            }
            toRemove.Columns.RemoveAll(c => c.PageComponentID == toRemove.ID);

            p.PageComponents.Remove(toRemove);


            for (int i = 0; i < p.PageComponents.Count(); i++)
            {
                p.PageComponents[i].DisplayOrder = i;
                
            }
            
           

            await _ctx.SaveChangesAsync();



            return Redirect("?pageid=" + PageId);
        }

        public async Task<IActionResult> OnPostAddComponentAsync()
        {
            var p = _ctx.SitePage.Include(p => p.PageComponents).FirstOrDefault(sp => sp.ID == PageId);

            PageComponent comp = new PageComponent();

            if (p.PageComponents.Count() == 0)
            {
                comp.DisplayOrder = 0;               
            }
            else
            {
                comp.DisplayOrder = p.PageComponents.Max(pc => pc.DisplayOrder) + 1;
            }                                                
                comp.Columns = new();
                comp.Columns.Add(new ColumnComp(0));
                            
            p.PageComponents.Add(comp);

            await _ctx.SaveChangesAsync();

            LastElement = comp.ID;

            return Redirect("?pageid=" + PageId);
        }
        public async Task<IActionResult> OnPostAddColumnAsync(int compid)
        {
            var p = _ctx.SitePage.Include(p => p.PageComponents).ThenInclude(pc => pc.Columns).FirstOrDefault(sp => sp.ID == PageId);

            var pc = p.PageComponents.FirstOrDefault(pc => pc.ID == compid);
            var displayorder = pc.Columns.Count();
            pc.Columns.Add(new ColumnComp(displayorder));

            await _ctx.SaveChangesAsync();
            
            LastElement = compid;

            return Redirect("?pageid=" + PageId);
        }



        public async Task<IActionResult> OnPostChangeBgColorAsync(int compid)
        {
            var p = _ctx.SitePage.Include(p => p.PageComponents).FirstOrDefault(sp => sp.ID == PageId);

            var pc = p.PageComponents.FirstOrDefault(pc => pc.ID == compid);

            pc.BgColor = BgColor;
           
            await _ctx.SaveChangesAsync();
            LastElement = compid;
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
            LastElement = compid;
            return Redirect("?pageid=" + PageId);


        }
        public async Task<IActionResult> OnPostSaveColumnCompAsync(int compid, int column)
        {
            var p = _ctx.SitePage.Include(p => p.PageComponents).ThenInclude(pc => pc.Columns).FirstOrDefault(sp => sp.ID == PageId);

            var pc = p.PageComponents.FirstOrDefault(pc => pc.ID == compid);

            var cc = pc.Columns.FirstOrDefault(c => c.ID == column);

            var results = HttpContext.Request.Form.ToList();

            var newpc = results.FirstOrDefault(r => r.Key == "" + column);
            cc.Content = newpc.Value;

            await _ctx.SaveChangesAsync();
            LastElement = compid;
            return Redirect("?pageid=" + PageId);


        }

        public async Task<IActionResult> OnPostSetImageAsync(int compid, int displayOrder)
        {
            var p = _ctx.SitePage.Include(p => p.PageComponents).ThenInclude(pc => pc.Columns).FirstOrDefault(sp => sp.ID == PageId);

            var cc = p.PageComponents.FirstOrDefault(pc => pc.ID == compid).Columns.FirstOrDefault(cc => cc.DisplayOrder == displayOrder);
            
            if (UploadedImage != null)
            {
                var file = "./wwwroot/img/" + UploadedImage.FileName;
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await UploadedImage.CopyToAsync(fileStream);
                }
            }
            cc.ImageURL = UploadedImage != null ? UploadedImage.FileName : "";
            cc.ImageSize = 50;
            await _ctx.SaveChangesAsync();
            LastElement = compid;
            return Redirect("?pageid=" + PageId);
        }  

        public async Task<IActionResult> OnPostSetLinkAsync(int compid, string LinkURL, string LinkText, int displayOrder)
        {
            var p = _ctx.SitePage.Include(p => p.PageComponents).ThenInclude(pc => pc.Columns).FirstOrDefault(sp => sp.ID == PageId);

            var cc = p.PageComponents.FirstOrDefault(pc => pc.ID == compid).Columns.FirstOrDefault(cc => cc.DisplayOrder == displayOrder);

            cc.LinkURL = LinkURL;
            cc.LinkText = LinkText;
            await _ctx.SaveChangesAsync();
            LastElement = compid;
            return Redirect("?pageid=" + PageId);
        }

        public async Task<IActionResult> OnPostSetColumnCompTypeAsync(int compid, string comptype, int displayOrder)
        {
            var p = _ctx.SitePage.Include(p => p.PageComponents).ThenInclude(pc => pc.Columns).FirstOrDefault(sp => sp.ID == PageId);

            var cc = p.PageComponents.FirstOrDefault(pc => pc.ID == compid).Columns.FirstOrDefault(cc => cc.DisplayOrder == displayOrder);

            cc.ComponentType = comptype;
            await _ctx.SaveChangesAsync();
            LastElement = compid;
            return Redirect("?pageid=" + PageId);
        }

        public async Task<IActionResult> OnPostSetImageSizeAsync(int compid, int displayOrder)
        {
            var p = _ctx.SitePage.Include(p => p.PageComponents).ThenInclude(pc => pc.Columns).FirstOrDefault(sp => sp.ID == PageId);

            var cc = p.PageComponents.FirstOrDefault(pc => pc.ID == compid).Columns.FirstOrDefault(cc => cc.DisplayOrder == displayOrder);

            cc.ImageSize = ImageSize;

            await _ctx.SaveChangesAsync();
            LastElement = compid;
            return Redirect("?pageid=" + PageId);
        }

        public async Task<IActionResult> OnPostSetMapLocationAsync(int compid, int displayOrder)
        {
            var p = _ctx.SitePage.Include(p => p.PageComponents).ThenInclude(pc => pc.Columns).FirstOrDefault(sp => sp.ID == PageId);

            var cc = p.PageComponents.FirstOrDefault(pc => pc.ID == compid).Columns.FirstOrDefault(cc => cc.DisplayOrder == displayOrder);

            if(MapLocation != null)
            {
            cc.MapLocation = MapLocation;
            }

            await _ctx.SaveChangesAsync();
            LastElement = compid;
            return Redirect("?pageid=" + PageId);
        }

        public async Task<IActionResult> OnPostSetTickersAsync(int compid, int displayOrder)
        {
            var p = _ctx.SitePage.Include(p => p.PageComponents).ThenInclude(pc => pc.Columns).FirstOrDefault(sp => sp.ID == PageId);

            var cc = p.PageComponents.FirstOrDefault(pc => pc.ID == compid).Columns.FirstOrDefault(cc => cc.DisplayOrder == displayOrder);

            p.TickerSymbol1 = TickerSymbol1;
            p.TickerName1 = TickerName1;
            p.TickerSymbol2 = TickerSymbol2;
            p.TickerName2 = TickerName2;
            p.TickerSymbol3 = TickerSymbol3;
            p.TickerName3 = TickerName3;
            p.TickerSymbol4 = TickerSymbol4;
            p.TickerName4 = TickerName4;
            p.TickerSymbol5 = TickerSymbol5;
            p.TickerName5 = TickerName5;

            await _ctx.SaveChangesAsync();
            LastElement = compid;
            return Redirect("?pageid=" + PageId);
        }

        public async Task<IActionResult> OnPostSetVideoAsync(int compid, int displayOrder)
        {
            var p = _ctx.SitePage.Include(p => p.PageComponents).ThenInclude(pc => pc.Columns).FirstOrDefault(sp => sp.ID == PageId);

            var cc = p.PageComponents.FirstOrDefault(pc => pc.ID == compid).Columns.FirstOrDefault(cc => cc.DisplayOrder == displayOrder);

            if (VideoString != null)
            {
                cc.VideoString = VideoString;
            }

            await _ctx.SaveChangesAsync();
            LastElement = compid;
            return Redirect("?pageid=" + PageId);
        }

        public async Task<IActionResult> OnPostSetPluginAsync(int compid, int displayOrder)
        {
            var p = _ctx.SitePage.Include(p => p.PageComponents).ThenInclude(pc => pc.Columns).FirstOrDefault(sp => sp.ID == PageId);

            var cc = p.PageComponents.FirstOrDefault(pc => pc.ID == compid).Columns.FirstOrDefault(cc => cc.DisplayOrder == displayOrder);

            if (PluginString != null)
            {
                cc.PluginString = PluginString;
            }

            await _ctx.SaveChangesAsync();
            LastElement = compid;
            return Redirect("?pageid=" + PageId);
        }

        public async Task<IActionResult> OnPostDeletePageAsync()
        { 
            var p = _ctx.SitePage.Include(p => p.PageComponents).ThenInclude(pc => pc.Columns).FirstOrDefault(sp => sp.ID == DeletePageId);
            if (!p.IsIndex)
            {               
                foreach (var row in p.PageComponents)
                {
                    foreach (var col in row.Columns)
                    {
                        if (col.ImageURL != null)
                        {
                            try
                            {
                                System.IO.File.Delete("./wwwroot/img/" + col.ImageURL);
                            }
                            catch { }
                        }

                    }
                    row.Columns.Clear();
                }

                p.PageComponents.Clear();

               var id = int.Parse(HttpContext.Session.GetString("CurrentProjectID"));
               CurrentProject = _ctx.Projects.Find(id);
               CurrentProject.Pages.Remove(p);
               
                await _ctx.SaveChangesAsync();

                PageId = _ctx.SitePage.FirstOrDefault(p => p.IsIndex && p.ProjectID == CurrentProject.ID).ID;
            }
            return Redirect("?pageid=" + PageId);
        }
    }
}
