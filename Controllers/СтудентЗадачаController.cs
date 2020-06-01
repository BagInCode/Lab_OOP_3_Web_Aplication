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
            ViewData["ЗадачаId"] = new SelectList(_context.Задачи, "Id", "Id");
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
                if (_context.СтудентЗадача.Any(e => e.СтудентId == студентЗадача.СтудентId &&
                                                    e.ЗадачаId == студентЗадача.ЗадачаId &&
                                                    e.Id != студентЗадача.Id))
                {
                    return RedirectToAction("ErrorScreen", new { textOfError = "Даный Студент уже принял к исполнению даную Задачу" });
                }

                var task = _context.Задачи.Find(студентЗадача.ЗадачаId);
                var script = _context.Сценарии.Find(task.СценарийId);
                int countActors = script.КВоАктёров;

                if (_context.СтудентЗадача.Where(f => f.ЗадачаId == студентЗадача.ЗадачаId).Count() >= countActors)
                {
                    return RedirectToAction("ErrorScreen", new { textOfError = "Все роли для этой Задачи уже заняты" });
                }

                if (task.Дата < DateTime.Now)
                {
                    return RedirectToAction("ErrorScreen", new { textOfError = "Вы опоздали, данная задача уже в прошлом" });
                }

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

        public async Task<IActionResult> ErrorScreen(string? textOfError)
        {
            if (textOfError == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Text = textOfError;

            return View();
        }
    }
}
