using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLab;

namespace WebLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly LabOOPContext _context;

        public ChartsController(LabOOPContext context)
        {
            _context = context;
        }

        [HttpGet("JsonDataTasksByScripts")]

        public JsonResult JsonDataTasksByScript()
        {
            var scripts = _context.Сценарии.Include(s => s.Задачи).ToList();

            List<object> scriptTask = new List<object>();

            scriptTask.Add(new[] { "Сценарий", "К-во Задач" });

            foreach(var scr in scripts)
            {
                scriptTask.Add(new object[] { scr.Описание.Substring(0, Math.Min(scr.Описание.Length, 20)), scr.Задачи.Count() });
            }

            return new JsonResult(scriptTask);
        }

        [HttpGet("JsonDataTasksByMonth")]
        public JsonResult JsonDataTasksByMonth(string? Id)
        {
            var Jan = _context.Задачи.Where(e => e.Дата.Month == 1).Count();
            var Feb = _context.Задачи.Where(e => e.Дата.Month == 2).Count();
            var Mar = _context.Задачи.Where(e => e.Дата.Month == 3).Count();
            var Apr = _context.Задачи.Where(e => e.Дата.Month == 4).Count();
            var May = _context.Задачи.Where(e => e.Дата.Month == 5).Count();
            var Jun = _context.Задачи.Where(e => e.Дата.Month == 6).Count();
            var Jul = _context.Задачи.Where(e => e.Дата.Month == 7).Count();
            var Aug = _context.Задачи.Where(e => e.Дата.Month == 8).Count();
            var Sep = _context.Задачи.Where(e => e.Дата.Month == 9).Count();
            var Oct = _context.Задачи.Where(e => e.Дата.Month == 10).Count();
            var Nov = _context.Задачи.Where(e => e.Дата.Month == 11).Count();
            var Dec = _context.Задачи.Where(e => e.Дата.Month == 12).Count();

            List<object> monthTask = new List<object>();

            monthTask.Add(new[] { "Месяц", "К-во Задач" });

            monthTask.Add(new object[] { "Янв", Jan });
            monthTask.Add(new object[] { "Фев", Feb });
            monthTask.Add(new object[] { "Мар", Mar });
            monthTask.Add(new object[] { "Апр", Apr });
            monthTask.Add(new object[] { "Май", May });
            monthTask.Add(new object[] { "Июн", Jun });
            monthTask.Add(new object[] { "Июл", Jul });
            monthTask.Add(new object[] { "Авг", Aug });
            monthTask.Add(new object[] { "Сен", Sep });
            monthTask.Add(new object[] { "Окт", Oct });
            monthTask.Add(new object[] { "Ноя", Nov });
            monthTask.Add(new object[] { "Дек", Dec });

            return new JsonResult(monthTask);
        }        
    }
}
