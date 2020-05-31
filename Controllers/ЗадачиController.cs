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
    public class ЗадачиController : Controller
    {
        private readonly LabOOPContext _context;

        public ЗадачиController(LabOOPContext context)
        {
            _context = context;
        }

        // GET: Задачи
        public async Task<IActionResult> Index()
        {
            var labOOPContext = _context.Задачи.Include(з => з.Пользователь).Include(з => з.Сценарий);
            return View(await labOOPContext.ToListAsync());
        }

        // GET: Задачи/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var задачи = await _context.Задачи
                .Include(з => з.Пользователь)
                .Include(з => з.Сценарий)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (задачи == null)
            {
                return NotFound();
            }

            return View(задачи);
        }

        // GET: Задачи/Create
        public IActionResult Create()
        {
            ViewData["ПользовательId"] = new SelectList(_context.Пользователь, "Mail", "Mail");
            ViewData["СценарийId"] = new SelectList(_context.Сценарии, "Id", "Описание");
            return View();
        }

        // POST: Задачи/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Место,Дата,Описание,ПользовательId,СценарийId")] Задачи задачи)
        {
            if (ModelState.IsValid)
            {
                _context.Add(задачи);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ПользовательId"] = new SelectList(_context.Пользователь, "Mail", "Mail", задачи.ПользовательId);
            ViewData["СценарийId"] = new SelectList(_context.Сценарии, "Id", "Описание", задачи.СценарийId);
            return View(задачи);
        }

        // GET: Задачи/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var задачи = await _context.Задачи.FindAsync(id);
            if (задачи == null)
            {
                return NotFound();
            }
            ViewData["ПользовательId"] = new SelectList(_context.Пользователь, "Mail", "Mail", задачи.ПользовательId);
            ViewData["СценарийId"] = new SelectList(_context.Сценарии, "Id", "Описание", задачи.СценарийId);
            return View(задачи);
        }

        // POST: Задачи/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Место,Дата,Описание,ПользовательId,СценарийId")] Задачи задачи)
        {
            if (id != задачи.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(задачи);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ЗадачиExists(задачи.Id))
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
            ViewData["ПользовательId"] = new SelectList(_context.Пользователь, "Mail", "Mail", задачи.ПользовательId);
            ViewData["СценарийId"] = new SelectList(_context.Сценарии, "Id", "Описание", задачи.СценарийId);
            return View(задачи);
        }

        // GET: Задачи/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var задачи = await _context.Задачи
                .Include(з => з.Пользователь)
                .Include(з => з.Сценарий)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (задачи == null)
            {
                return NotFound();
            }

            return View(задачи);
        }

        // POST: Задачи/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var задачи = await _context.Задачи.FindAsync(id);
            _context.Задачи.Remove(задачи);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ЗадачиExists(int id)
        {
            return _context.Задачи.Any(e => e.Id == id);
        }
    }
}
