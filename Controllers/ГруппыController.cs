using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebLab;

namespace WebLab.Controllers
{
    public class ГруппыController : Controller
    {
        private readonly LabOOPContext _context;

        public ГруппыController(LabOOPContext context)
        {
            _context = context;
        }

        // GET: Группы
        public async Task<IActionResult> Index()
        {
            var labOOPContext = _context.Группы.Include(г => г.Вуз);
            return View(await labOOPContext.ToListAsync());
        }

        // GET: Группы/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var группы = await _context.Группы
                .Include(г => г.Вуз)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (группы == null)
            {
                return NotFound();
            }

            return RedirectToAction("StudentsByGroup","Студенты",new { id = группы.Id, name = группы.Название});
        }

        // GET: Группы/Create
        public IActionResult Create()
        {
            ViewData["ВузId"] = new SelectList(_context.Вузы, "Id", "НазваниеВуза");
            return View();
        }

        // POST: Группы/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Название,ВузId")] Группы группы)
        {
            if (ModelState.IsValid)
            {
                _context.Add(группы);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ВузId"] = new SelectList(_context.Вузы, "Id", "НазваниеВуза", группы.ВузId);
            return View(группы);
        }

        // GET: Группы/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var группы = await _context.Группы.FindAsync(id);
            if (группы == null)
            {
                return NotFound();
            }
            ViewData["ВузId"] = new SelectList(_context.Вузы, "Id", "НазваниеВуза", группы.ВузId);
            return View(группы);
        }

        // POST: Группы/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Название,ВузId")] Группы группы)
        {
            if (id != группы.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(группы);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ГруппыExists(группы.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ВузId"] = new SelectList(_context.Вузы, "Id", "НазваниеВуза", группы.ВузId);
            return View(группы);
        }

        // GET: Группы/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var группы = await _context.Группы
                .Include(г => г.Вуз)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (группы == null)
            {
                return NotFound();
            }

            return View(группы);
        }

        // POST: Группы/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var группы = await _context.Группы.FindAsync(id);
            _context.Группы.Remove(группы);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ГруппыExists(int id)
        {
            return _context.Группы.Any(e => e.Id == id);
        }
    }
}
