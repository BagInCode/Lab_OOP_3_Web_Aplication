using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Attributes;
using ClosedXML.Excel;
using ClosedXML.Utils;
using WebLab;
using Microsoft.AspNetCore.Http;
using System.IO;

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
                    return RedirectToAction("ErrorScreen", "Home", new { textOfError = "ВУЗ с таким названием уже существует", controllerName = "Вузы" });
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
                return RedirectToAction("ErrorScreen", "Home", new { textOfError = "ВУЗ с таким названием уже существует", controllerName = "Вузы" });
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);

                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                Вузы newUniv;
                                var t = (from temp in _context.Вузы where temp.НазваниеВуза.Contains(worksheet.Name) select temp).ToList();

                                if (t.Count > 0)
                                {
                                    newUniv = t[0];
                                }
                                else
                                {
                                    newUniv = new Вузы();
                                    newUniv.НазваниеВуза = worksheet.Name;
                                    _context.Вузы.Add(newUniv);

                                    await _context.SaveChangesAsync();

                                    newUniv = _context.Вузы.FirstOrDefault(e => e.НазваниеВуза == worksheet.Name);
                                }

                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Группы group = new Группы();
                                        group.ВузId = newUniv.Id;
                                        group.Название = row.Cell(1).Value.ToString();

                                        if (!_context.Группы.Any(e => e.ВузId == newUniv.Id && e.Название == group.Название))
                                        {
                                            _context.Группы.Add(group);
                                        }
                                    }
                                    catch(Exception e)
                                    {
                                        throw;
                                    }
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Export(int? id)
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var group = _context.Группы.Where(e => e.ВузId == id).ToList();

                var worksheet = workbook.Worksheets.Add(_context.Вузы.Find(id).НазваниеВуза);

                worksheet.Cell("A1").Value = "Название";
                worksheet.Row(1).Style.Font.Bold = true;

                for (int i = 0; i < group.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = group[i].Название;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"WebLab_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }
    }
}
