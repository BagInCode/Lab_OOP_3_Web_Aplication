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
    public class ПользовательController : Controller
    {
        private readonly LabOOPContext _context;

        public ПользовательController(LabOOPContext context)
        {
            _context = context;
        }

        // GET: Пользователь
        public async Task<IActionResult> Index()
        {
            return View(await _context.Пользователь.ToListAsync());
        }

        // GET: Пользователь/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var пользователь = await _context.Пользователь
                .FirstOrDefaultAsync(m => m.Mail == id);
            if (пользователь == null)
            {
                return NotFound();
            }

            return View(пользователь);
        }

        // GET: Пользователь/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Пользователь/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Mail,Фио,Пароль")] Пользователь пользователь)
        {
            if (ModelState.IsValid)
            {
                if (_context.Преподаватели.Any(d => d.Mail == пользователь.Mail) ||
                   _context.Студенты.Any(e => e.Mail == пользователь.Mail) ||
                   _context.Пользователь.Any(f => f.Mail == пользователь.Mail))
                {
                    return RedirectToAction("ErrorScreen", "Home", new { textOfError = "Такой почтовый адрес уже зарегестрирован", controllerName = "Пользователь" });
                }

                int passwordHesh = calcHesh(пользователь.Пароль);
                пользователь.Пароль = passwordHesh.ToString();

                _context.Add(пользователь);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(пользователь);
        }

        // GET: Пользователь/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var пользователь = await _context.Пользователь.FindAsync(id);
            if (пользователь == null)
            {
                return NotFound();
            }
            return View(пользователь);
        }

        // POST: Пользователь/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Mail,Фио,Пароль")] Пользователь пользователь)
        {
            if (id != пользователь.Mail)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(needToCalcHesh(пользователь.Пароль))
                    {
                        int passwordHesh = calcHesh(пользователь.Пароль);
                        пользователь.Пароль = passwordHesh.ToString();
                    }

                    _context.Update(пользователь);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ПользовательExists(пользователь.Mail))
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
            return View(пользователь);
        }

        // GET: Пользователь/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var пользователь = await _context.Пользователь
                .FirstOrDefaultAsync(m => m.Mail == id);
            if (пользователь == null)
            {
                return NotFound();
            }

            return View(пользователь);
        }

        // POST: Пользователь/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var пользователь = await _context.Пользователь.FindAsync(id);
            _context.Пользователь.Remove(пользователь);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ПользовательExists(string id)
        {
            return _context.Пользователь.Any(e => e.Mail == id);
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

            for(int i = 0; i < str.Length; i++)
            {
                result = result + (str[i] - i) * step;
                step *= key;
            }

            return result;
        }
        public async Task<IActionResult> AllTasks(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var пользователь = await _context.Пользователь
                .FirstOrDefaultAsync(m => m.Mail == id);
            if (пользователь == null)
            {
                return NotFound();
            }

            return RedirectToAction("TasksByUser", "Задачи", new { id = пользователь.Mail});
        }
    }
}
