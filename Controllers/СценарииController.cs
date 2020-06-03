using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClosedXML;

namespace WebLab.Controllers
{
    public class СценарииController : Controller
    {
        private readonly LabOOPContext _context;

        public СценарииController(LabOOPContext context)
        {
            _context = context;
        }

        // GET: Сценарии
        public async Task<IActionResult> Index()
        {
            return View(await _context.Сценарии.ToListAsync());
        }

        // GET: Сценарии/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var сценарии = await _context.Сценарии
                .FirstOrDefaultAsync(m => m.Id == id);
            if (сценарии == null)
            {
                return NotFound();
            }

            return View(сценарии);
        }

        // GET: Сценарии/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Сценарии/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,КВоАктёров,Описание")] Сценарии сценарии)
        {
            if (ModelState.IsValid)
            {
                _context.Add(сценарии);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(сценарии);
        }

        // GET: Сценарии/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var сценарии = await _context.Сценарии.FindAsync(id);
            if (сценарии == null)
            {
                return NotFound();
            }
            return View(сценарии);
        }

        // POST: Сценарии/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,КВоАктёров,Описание")] Сценарии сценарии)
        {
            if (id != сценарии.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(сценарии);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!СценарииExists(сценарии.Id))
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
            return View(сценарии);
        }

        // GET: Сценарии/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var сценарии = await _context.Сценарии
                .FirstOrDefaultAsync(m => m.Id == id);
            if (сценарии == null)
            {
                return NotFound();
            }

            return View(сценарии);
        }

        // POST: Сценарии/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var сценарии = await _context.Сценарии.FindAsync(id);
            _context.Сценарии.Remove(сценарии);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool СценарииExists(int id)
        {
            return _context.Сценарии.Any(e => e.Id == id);
        }
    }
}
