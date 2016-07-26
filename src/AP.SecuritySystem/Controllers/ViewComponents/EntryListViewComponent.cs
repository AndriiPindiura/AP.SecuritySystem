using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AP.SecuritySystem.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Authorization;
using Microsoft.Data.Entity;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AP.SecuritySystem.Controllers.ViewComponents
{
    [Authorize("Supervisor")]
    public class EntryListViewComponent : ViewComponent
    {
        public EntryListViewComponent(ApplicationDbContext database)
        {
            db = database;
        }
        private readonly ApplicationDbContext db;

        // GET: /<controller>/
        public IViewComponentResult Invoke(int id)
        {
            try
            {

                ViewBag.ServerId = id;
                Console.WriteLine("Entries {0}", id);
                IQueryable<Entry> entries;
                if (id == -1)
                {
                    entries = db.Entries.Include(i => i.Server);
                    //return View("EntryList", db.Entries);
                }
                else
                {
                    entries = db.Entries.Where(s => s.Server.Id == id).Include(i => i.Server);
                    //return View("EntryList", db.Entries.Where(s => s.Server.Id == id));
                }
                foreach (Entry entry in entries)
                {
                    Console.WriteLine("Entry {0} in server {1}", entry.Description, entry.Server.Description);
                }
                return View("EntryList", entries);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
