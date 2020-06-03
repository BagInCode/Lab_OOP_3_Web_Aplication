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
            var labOOPContext = _context.Студенты.Include(с => с.Группа).Include(c => c.Группа.Вуз);
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
                .Include(с => с.Группа).Include(с => с.Группа.Вуз)
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
                if (_context.Преподаватели.Any(d => d.Mail == студенты.Mail) ||
                   _context.Студенты.Any(e => e.Mail == студенты.Mail) ||
                   _context.Пользователь.Any(f => f.Mail == студенты.Mail))
                {
                    return RedirectToAction("ErrorScreen", "Home", new { textOfError = "Такой почтовый адрес уже зарегестрирован", controllerName = "Студенты" });
                }

                int passwordHesh = calcHesh(студенты.Пароль);
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
                        int passwordHesh = calcHesh(студенты.Пароль);
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
                .Include(с => с.Группа).Include(с => с.Группа.Вуз)
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

        public async Task<IActionResult> StudentsByGroup(int? id, string? name)
        {
            if(id == null)
            {
                return RedirectToAction("Index", "Группы");
            }

            ViewBag.CategoryName = name;
            ViewBag.CategoryId = id;

            var studentsByGroup = _context.Студенты.Where(e => e.ГруппаId == id);

            return View(await studentsByGroup.ToListAsync());
        }

        public async Task<IActionResult> StudentsByTask(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Index", "Задачи");
            }

            ViewBag.CategoryName = id;
            ViewBag.CategoryId = id;
            
            var studentsByTask = _context.Студенты.Where(e => _context.СтудентЗадача.Any(f => f.ЗадачаId == id && f.СтудентId == e.Mail)).Include(e => e.Группа).Include(e => e.Группа.Вуз);    

            return View(await studentsByTask.ToListAsync());
        }

        public async Task<IActionResult> StudentTask(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var студент = await _context.Студенты
                .FirstOrDefaultAsync(m => m.Mail == id);
            if (студент == null)
            {
                return NotFound();
            }

            return RedirectToAction("TasksByStudent", "Задачи", new { id = студент.Mail });
        }

        public async Task<IActionResult> Diagrams(string? Id)
        {
            ViewBag.TempId = Id;

            return View();
        }
    }
}
