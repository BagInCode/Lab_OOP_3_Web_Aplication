﻿using System;
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
                int passwordHesh = пользователь.Пароль.GetHashCode();
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
                        int passwordHesh = пользователь.Пароль.GetHashCode();
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
