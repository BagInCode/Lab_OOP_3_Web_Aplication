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
    public class СтудентыController : Controller
    {
        private readonly LabOOPContext _context;

        public СтудентыController(LabOOPContext context)
        {
            _context = context;
        }

        // GET: Студенты
        public async Task<IActionResult> Index()
        {
            var labOOPContext = _context.Студенты.Include(с => с.Группа);
            return View(await labOOPContext.ToListAsync());
        }

        // GET: Студенты/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var студенты = await _context.Студенты
                .Include(с => с.Группа)
                .FirstOrDefaultAsync(m => m.Mail == id);
            if (студенты == null)
            {
                return NotFound();
            }

            return View(студенты);
        }

        // GET: Студенты/Create
        public IActionResult Create()
        {
            ViewData["ГруппаId"] = new SelectList(_context.Группы, "Id", "Название");
            return View();
        }

        // POST: Студенты/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Mail,Фио,Пароль,ГруппаId")] Студенты студенты)
        {
            if (ModelState.IsValid)
            {
                int passwordHesh = студенты.Пароль.GetHashCode();
                студенты.Пароль = passwordHesh.ToString();

                _context.Add(студенты);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ГруппаId"] = new SelectList(_context.Группы, "Id", "Название", студенты.ГруппаId);
            return View(студенты);
        }

        // GET: Студенты/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var студенты = await _context.Студенты.FindAsync(id);
            if (студенты == null)
            {
                return NotFound();
            }
            ViewData["ГруппаId"] = new SelectList(_context.Группы, "Id", "Название", студенты.ГруппаId);
            return View(студенты);
        }

        // POST: Студенты/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Mail,Фио,Пароль,ГруппаId")] Студенты студенты)
        {
            if (id != студенты.Mail)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(needToCalcHesh(студенты.Пароль))
                    {
                        int passwordHesh = студенты.Пароль.GetHashCode();
                        студенты.Пароль = passwordHesh.ToString();
                    }

                    _context.Update(студенты);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!СтудентыExists(студенты.Mail))
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
            ViewData["ГруппаId"] = new SelectList(_context.Группы, "Id", "Название", студенты.ГруппаId);
            return View(студенты);
        }

        // GET: Студенты/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var студенты = await _context.Студенты
                .Include(с => с.Группа)
                .FirstOrDefaultAsync(m => m.Mail == id);
            if (студенты == null)
            {
                return NotFound();
            }

            return View(студенты);
        }

        // POST: Студенты/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var студенты = await _context.Студенты.FindAsync(id);
            _context.Студенты.Remove(студенты);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool СтудентыExists(string id)
        {
            return _context.Студенты.Any(e => e.Mail == id);
        }

        private bool needToCalcHesh(string value)
        {
            if (value[0] != '-' && (value[0] < '0' || value[0] > '9'))
            {
                return false;
            }

            for (int i = 1; i < value.Length; i++)
            {
                if (value[i] < '0' || value[i] > '9')
                {
                    return true;
                }
            }

            return false;
        }
    }
}
