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
    public class ВузыController : Controller
    {
        private readonly LabOOPContext _context;

        public ВузыController(LabOOPContext context)
        {
            _context = context;
        }

        // GET: Вузы
        public async Task<IActionResult> Index()
        {
            return View(await _context.Вузы.ToListAsync());
        }

        // GET: Вузы/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var вузы = await _context.Вузы
                .FirstOrDefaultAsync(m => m.Id == id);
            if (вузы == null)
            {
                return NotFound();
            }

            return View(вузы);
        }

        // GET: Вузы/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Вузы/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,НазваниеВуза")] Вузы вузы)
        {
            if (ModelState.IsValid)
            {
                if (_context.Вузы.Any(e => e.НазваниеВуза == вузы.НазваниеВуза && e.Id != вузы.Id))
                {
                    return RedirectToAction("ErrorScreen", new { textOfError = "ВУЗ с таким названием уже существует" });
                }

                _context.Add(вузы);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(вузы);
        }

        // GET: Вузы/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var вузы = await _context.Вузы.FindAsync(id);
            if (вузы == null)
            {
                return NotFound();
            }
            return View(вузы);
        }

        // POST: Вузы/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,НазваниеВуза")] Вузы вузы)
        {
            if (id != вузы.Id)
            {
                return NotFound();
            }

            if(_context.Вузы.Any(e => e.НазваниеВуза == вузы.НазваниеВуза && e.Id != вузы.Id))
            {
                return RedirectToAction("ErrorScreen", new { textOfError = "ВУЗ с таким названием уже существует" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(вузы);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ВузыExists(вузы.Id))
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
            return View(вузы);
        }

        // GET: Вузы/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var вузы = await _context.Вузы
                .FirstOrDefaultAsync(m => m.Id == id);
            if (вузы == null)
            {
                return NotFound();
            }

            return View(вузы);
        }

        // POST: Вузы/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var вузы = await _context.Вузы.FindAsync(id);
            _context.Вузы.Remove(вузы);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ВузыExists(int id)
        {
            return _context.Вузы.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ErrorScreen(string? textOfError)
        {
            if(textOfError == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Text = textOfError;

            return View();
        }

        public async Task<IActionResult> DetailsLectors(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var вузы = await _context.Вузы
                .FirstOrDefaultAsync(m => m.Id == id);
            if (вузы == null)
            {
                return NotFound();
            }

            return RedirectToAction("LectorsByUniv", "Преподаватели", new { id = вузы.Id, name = вузы.НазваниеВуза });
        }

        public async Task<IActionResult> DetailsGroups(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var вузы = await _context.Вузы
                .FirstOrDefaultAsync(m => m.Id == id);
            if (вузы == null)
            {
                return NotFound();
            }

            return RedirectToAction("GroupByUniv", "Группы", new { id = вузы.Id, name = вузы.НазваниеВуза });
        }
    }
}
