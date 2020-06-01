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
    public class ПреподавателиController : Controller
    {
        private readonly LabOOPContext _context;

        public ПреподавателиController(LabOOPContext context)
        {
            _context = context;
        }

        // GET: Преподаватели
        public async Task<IActionResult> Index()
        {
            var labOOPContext = _context.Преподаватели.Include(п => п.Вуз);
            return View(await labOOPContext.ToListAsync());
        }

        // GET: Преподаватели/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var преподаватели = await _context.Преподаватели
                .Include(п => п.Вуз)
                .FirstOrDefaultAsync(m => m.Mail == id);
            if (преподаватели == null)
            {
                return NotFound();
            }

            return View(преподаватели);
        }

        // GET: Преподаватели/Create
        public IActionResult Create()
        {
            ViewData["ВузId"] = new SelectList(_context.Вузы, "Id", "НазваниеВуза");
            return View();
        }

        // POST: Преподаватели/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Mail,Фио,Пароль,ВузId")] Преподаватели преподаватели)
        {
            if (ModelState.IsValid)
            {
                int passwordHesh = calcHesh(преподаватели.Пароль);
                преподаватели.Пароль = passwordHesh.ToString();

                _context.Add(преподаватели);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ВузId"] = new SelectList(_context.Вузы, "Id", "НазваниеВуза", преподаватели.ВузId);
            return View(преподаватели);
        }

        // GET: Преподаватели/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var преподаватели = await _context.Преподаватели.FindAsync(id);
            if (преподаватели == null)
            {
                return NotFound();
            }
            ViewData["ВузId"] = new SelectList(_context.Вузы, "Id", "НазваниеВуза", преподаватели.ВузId);
            return View(преподаватели);
        }

        // POST: Преподаватели/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Mail,Фио,Пароль,ВузId")] Преподаватели преподаватели)
        {
            if (id != преподаватели.Mail)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(needToCalcHesh(преподаватели.Пароль))
                    {
                        int passwordHesh = calcHesh(преподаватели.Пароль);
                        преподаватели.Пароль = passwordHesh.ToString();
                    }

                    _context.Update(преподаватели);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ПреподавателиExists(преподаватели.Mail))
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
            ViewData["ВузId"] = new SelectList(_context.Вузы, "Id", "НазваниеВуза", преподаватели.ВузId);
            return View(преподаватели);
        }

        // GET: Преподаватели/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var преподаватели = await _context.Преподаватели
                .Include(п => п.Вуз)
                .FirstOrDefaultAsync(m => m.Mail == id);
            if (преподаватели == null)
            {
                return NotFound();
            }

            return View(преподаватели);
        }

        // POST: Преподаватели/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var преподаватели = await _context.Преподаватели.FindAsync(id);
            _context.Преподаватели.Remove(преподаватели);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ПреподавателиExists(string id)
        {
            return _context.Преподаватели.Any(e => e.Mail == id);
        }

        private bool needToCalcHesh(string value)
        {
            if(value[0] != '-' && (value[0] < '0' || value[0] > '9'))
            {
                return true;
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
        private int calcHesh(string str)
        {
            int key = 2147483647;
            int step = 1;
            int result = 0;

            for (int i = 0; i < str.Length; i++)
            {
                result = result + (str[i] - i) * step;
                step *= key;
            }

            return result;
        }
    }
}
