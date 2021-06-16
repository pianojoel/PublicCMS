using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class ProjectSettingsModel : PageModel
    {

        private readonly PublicContext _ctx;
        public HttpClient _client = new HttpClient();
        [BindProperty]
        public int FontSize { get; set; }
        public GoogleFont GoogleFont { get; set; } 
        [BindProperty]
        public List<MenuItem> MenuItems { get; set; }
        [BindProperty]
        [Required]
        public string NewItemText { get; set; }
        [BindProperty]
        [Required]
        public string NewItemURL { get; set; }
        [BindProperty]
        public bool EnableMenu { get; set; }
        public Project CurrentProject { get; set; }
        [BindProperty]
        public string Menutype { get; set; }
        [BindProperty]
        public string BgColor { get; set; }
        [BindProperty]
        public string TextColor { get; set; }
        [BindProperty]
        public string Font { get; set; }
        [BindProperty]
        public string MenuTextColor { get; set; }
        [BindProperty]
        public string MenuBgColor { get; set; }
        [BindProperty]
        public bool EnableFooter { get; set; }
        [BindProperty]
        public string FooterTextColor { get; set; }
        [BindProperty]
        public string FooterBgColor { get; set; }
       [BindProperty]
        public string FooterContent { get; set; }
        [BindProperty]
        public int MenuFontSize { get; set; }
        [BindProperty]
        public string MenuFont { get; set; }
        [BindProperty]
        public int FooterFontSize { get; set; }
        [BindProperty]
        public string FooterFont { get; set; }

        public ProjectSettingsModel(PublicContext ctx)
        {
            _ctx = ctx;
            
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var id = int.Parse(HttpContext.Session.GetString("CurrentProjectID"));
            CurrentProject = _ctx.Projects.Include(p => p.Pages).Include(p => p.MenuItems).FirstOrDefault(p => p.ID == id);
            MenuItems = CurrentProject.MenuItems.OrderBy(m => m.DisplayOrder).ToList();
            Menutype = CurrentProject.MenuType;
            EnableMenu = CurrentProject.EnableMenu;
            EnableFooter = CurrentProject.EnableFooter;
            FontSize = CurrentProject.FontSize;
            MenuFontSize = CurrentProject.MenuFontSize;
            FooterFontSize = CurrentProject.FooterFontSize;

            await OnPostGetFontsAsync();
            return Page();

        }
        public async Task<IActionResult> OnPostAsync()
        {
            var id = int.Parse(HttpContext.Session.GetString("CurrentProjectID"));
            CurrentProject = _ctx.Projects.Include(p => p.Pages).Include(p => p.MenuItems).FirstOrDefault(p => p.ID == id);
            CurrentProject.EnableMenu = EnableMenu;
            CurrentProject.EnableFooter = EnableFooter;
            await _ctx.SaveChangesAsync();
            return Redirect("./ProjectSettings");
        }


        public async Task<IActionResult> OnPostSetLayoutAsync()
        {
            var id = int.Parse(HttpContext.Session.GetString("CurrentProjectID"));
            CurrentProject = _ctx.Projects.Include(p => p.Pages).Include(p => p.MenuItems).FirstOrDefault(p => p.ID == id);
            CurrentProject.MenuType = Menutype;
            await _ctx.SaveChangesAsync();
            return Redirect("./ProjectSettings");
        }



        public async Task<IActionResult> OnPostCreateItemAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var id = int.Parse(HttpContext.Session.GetString("CurrentProjectID"));
            CurrentProject = _ctx.Projects.Include(p => p.Pages).Include(p => p.MenuItems).FirstOrDefault(p => p.ID == id);
            MenuItems = CurrentProject.MenuItems;

            MenuItems.Add(new MenuItem(NewItemURL, NewItemText, MenuItems.Count()));

            await _ctx.SaveChangesAsync();

            return Redirect("./ProjectSettings");
        }

        public async Task<IActionResult> OnPostDeleteItemAsync(int deleteId)
        {
           
            var id = int.Parse(HttpContext.Session.GetString("CurrentProjectID"));
            CurrentProject = _ctx.Projects.Include(p => p.Pages).Include(p => p.MenuItems).FirstOrDefault(p => p.ID == id);
            var item = CurrentProject.MenuItems.FirstOrDefault(m => m.ID == deleteId);
            var subsequent = CurrentProject.MenuItems.Where(m => m.DisplayOrder > item.DisplayOrder);
            foreach (var m in subsequent)
            {
                m.DisplayOrder--;
            }
            
            CurrentProject.MenuItems.Remove(item);           
            
            
            
            
            await _ctx.SaveChangesAsync();
            return Redirect("./ProjectSettings");
        }
        public async Task<IActionResult> OnPostMoveUpAsync(int Id)
        {

            var id = int.Parse(HttpContext.Session.GetString("CurrentProjectID"));
            CurrentProject = _ctx.Projects.Include(p => p.Pages).Include(p => p.MenuItems).FirstOrDefault(p => p.ID == id);
            var item = CurrentProject.MenuItems.FirstOrDefault(m => m.ID == Id);
            var prevItem = CurrentProject.MenuItems.FirstOrDefault(m => m.DisplayOrder == item.DisplayOrder - 1);
            item.DisplayOrder--;
            prevItem.DisplayOrder++;

            await _ctx.SaveChangesAsync();
            return Redirect("./ProjectSettings");
        }
        public async Task<IActionResult> OnPostMoveDownAsync(int Id)
        {

            var id = int.Parse(HttpContext.Session.GetString("CurrentProjectID"));
            CurrentProject = _ctx.Projects.Include(p => p.Pages).Include(p => p.MenuItems).FirstOrDefault(p => p.ID == id);
            var item = CurrentProject.MenuItems.FirstOrDefault(m => m.ID == Id);
            var nextItem = CurrentProject.MenuItems.FirstOrDefault(m => m.DisplayOrder == item.DisplayOrder + 1);
            item.DisplayOrder++;
            nextItem.DisplayOrder--;

            await _ctx.SaveChangesAsync();
            return Redirect("./ProjectSettings");
        }

        public async Task<IActionResult> OnPostChangeSettingsAsync()
        {
            var id = int.Parse(HttpContext.Session.GetString("CurrentProjectID"));
            CurrentProject = _ctx.Projects.Include(p => p.Pages).Include(p => p.MenuItems).FirstOrDefault(p => p.ID == id);
            
            if(BgColor != null)
            {
                CurrentProject.BgColor = BgColor;
            }
            if(TextColor != null)
            {
                CurrentProject.TextColor = TextColor;
            }
            if(Font != null)
            {
                CurrentProject.Font = Font;
            }
            CurrentProject.FontSize = FontSize;
            await _ctx.SaveChangesAsync();
            return Redirect("./ProjectSettings");
        }


        public async Task<IActionResult> OnPostSetMenuColorsAsync()
        {
            var id = int.Parse(HttpContext.Session.GetString("CurrentProjectID"));
            CurrentProject = _ctx.Projects.Include(p => p.Pages).Include(p => p.MenuItems).FirstOrDefault(p => p.ID == id);

            if (MenuBgColor != null)
            {
                CurrentProject.MenuBgColor = MenuBgColor;
            }
            if (MenuTextColor != null)
            {
                CurrentProject.MenuTextColor = MenuTextColor;
            }
            

            await _ctx.SaveChangesAsync();
            return Redirect("./ProjectSettings");
        }

        public async Task<IActionResult> OnPostChangeFooterSettingsAsync()
        {
            var id = int.Parse(HttpContext.Session.GetString("CurrentProjectID"));
            CurrentProject = _ctx.Projects.Include(p => p.Pages).Include(p => p.MenuItems).FirstOrDefault(p => p.ID == id);

            if (FooterBgColor != null)
            {
                CurrentProject.FooterBgColor = FooterBgColor;
            }
            if (FooterTextColor != null)
            {
                CurrentProject.FooterTextColor = FooterTextColor;
            }
            if (FooterContent != null)
            {
                CurrentProject.FooterContent = FooterContent;
            }

            await _ctx.SaveChangesAsync();
            return Redirect("./ProjectSettings");
        }

        public async Task<IActionResult> OnPostGetFontsAsync()
        {
            var requestString = "https://www.googleapis.com/webfonts/v1/webfonts?key=AIzaSyDpngdKx0fxYhAXqllNUhNEdfHxZf46vU4";
            Task<string> getFontsStringTask = _client.GetStringAsync(requestString);
            var getFontsString = await getFontsStringTask;
            Console.WriteLine("Hej");
            GoogleFont = JsonSerializer.Deserialize<GoogleFont>(getFontsString);
            return Page();
        }

        public async Task<IActionResult> OnPostSetMenuFontAsync()
        {
            var id = int.Parse(HttpContext.Session.GetString("CurrentProjectID"));
            CurrentProject = _ctx.Projects.Include(p => p.Pages).Include(p => p.MenuItems).FirstOrDefault(p => p.ID == id);

            if (MenuFont != null)
            {
                CurrentProject.MenuFont = MenuFont;
            }
            if (MenuFontSize > 0)
            {
                CurrentProject.MenuFontSize = MenuFontSize;
            }


            await _ctx.SaveChangesAsync();
            return Redirect("./ProjectSettings");
        }
        public async Task<IActionResult> OnPostSetFooterFontAsync()
        {
            var id = int.Parse(HttpContext.Session.GetString("CurrentProjectID"));
            CurrentProject = _ctx.Projects.Include(p => p.Pages).Include(p => p.MenuItems).FirstOrDefault(p => p.ID == id);

            if (FooterFont != null)
            {
                CurrentProject.FooterFont = FooterFont;
            }
            if (FooterFontSize > 0)
            {
                CurrentProject.FooterFontSize = FooterFontSize;
            }


            await _ctx.SaveChangesAsync();


            return Redirect("./ProjectSettings");
        }
    }
}
