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
    public class СтудентЗадачаController : Controller
    {
        private readonly LabOOPContext _context;

        public СтудентЗадачаController(LabOOPContext context)
        {
            _context = context;
        }

        // GET: СтудентЗадача
        public async Task<IActionResult> Index()
        {
            var labOOPContext = _context.СтудентЗадача.Include(с => с.Задача).Include(с => с.Студент);
            return View(await labOOPContext.ToListAsync());
        }

        // GET: СтудентЗадача/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var студентЗадача = await _context.СтудентЗадача
                .Include(с => с.Задача)
                .Include(с => с.Студент)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (студентЗадача == null)
            {
                return NotFound();
            }

            return View(студентЗадача);
        }

        // GET: СтудентЗадача/Create
        public IActionResult Create()
        {
            ViewData["ЗадачаId"] = new SelectList(_context.Задачи, "Id", "Место");
            ViewData["СтудентId"] = new SelectList(_context.Студенты, "Mail", "Mail");
            return View();
        }

        // POST: СтудентЗадача/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,СтудентId,ЗадачаId")] СтудентЗадача студентЗадача)
        {
            if (ModelState.IsValid)
            {
                _context.Add(студентЗадача);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ЗадачаId"] = new SelectList(_context.Задачи, "Id", "Место", студентЗадача.ЗадачаId);
            ViewData["СтудентId"] = new SelectList(_context.Студенты, "Mail", "Mail", студентЗадача.СтудентId);
            return View(студентЗадача);
        }

        // GET: СтудентЗадача/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var студентЗадача = await _context.СтудентЗадача.FindAsync(id);
            if (студентЗадача == null)
            {
                return NotFound();
            }
            ViewData["ЗадачаId"] = new SelectList(_context.Задачи, "Id", "Место", студентЗадача.ЗадачаId);
            ViewData["СтудентId"] = new SelectList(_context.Студенты, "Mail", "Mail", студентЗадача.СтудентId);
            return View(студентЗадача);
        }

        // POST: СтудентЗадача/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,СтудентId,ЗадачаId")] СтудентЗадача студентЗадача)
        {
            if (id != студентЗадача.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(студентЗадача);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!СтудентЗадачаExists(студентЗадача.Id))
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
            ViewData["ЗадачаId"] = new SelectList(_context.Задачи, "Id", "Место", студентЗадача.ЗадачаId);
            ViewData["СтудентId"] = new SelectList(_context.Студенты, "Mail", "Mail", студентЗадача.СтудентId);
            return View(студентЗадача);
        }

        // GET: СтудентЗадача/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var студентЗадача = await _context.СтудентЗадача
                .Include(с => с.Задача)
                .Include(с => с.Студент)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (студентЗадача == null)
            {
                return NotFound();
            }

            return View(студентЗадача);
        }

        // POST: СтудентЗадача/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var студентЗадача = await _context.СтудентЗадача.FindAsync(id);
            _context.СтудентЗадача.Remove(студентЗадача);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool СтудентЗадачаExists(int id)
        {
            return _context.СтудентЗадача.Any(e => e.Id == id);
        }
    }
}
