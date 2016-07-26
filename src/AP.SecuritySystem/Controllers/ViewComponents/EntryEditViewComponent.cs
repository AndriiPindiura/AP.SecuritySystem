using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using AP.SecuritySystem.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AP.SecuritySystem.Controllers.ViewComponents
{
    [Authorize]
    public class EntryEditViewComponent : ViewComponent
    {
        public EntryEditViewComponent(ApplicationDbContext database)
        {
            db = database;
        }

        private readonly ApplicationDbContext db;

        private EntryModel GetEntryInfo(int entryId)
        {
            try
            {
                return new EntryModel
                {
                    Entry = db.Entries.FirstOrDefault(i => i.Id == entryId),
                    Servers = db.Servers.Where(s => s.Mode == 2).ToList()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }


        // GET: /<controller>/
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            Console.WriteLine("Entry Edit View Component with " + id);
            EntryModel entryInfo = new EntryModel();
            int entryId = -1;
            Int32.TryParse(id, out entryId);
            entryInfo = await Task.FromResult(GetEntryInfo(entryId));
            return View("EntryEdit", entryInfo);
        }
    }
}
