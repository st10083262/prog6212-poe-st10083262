using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Study_Tracker.Data;
using Study_Tracker.Models;
using Module = Study_Tracker.Models.Module;

namespace Study_Tracker.Controllers
{
    public class ModulesController : Controller
    {
        //private readonly User _user;
        private readonly Study_TrackerContext _context;

        public ModulesController(Study_TrackerContext context)
        {
            _context = context;
        }

        // GET: Modules
        public async Task<IActionResult> Index()
        {
            List<Module> modules = await _context.Module.Where(a => a.user.username == User.FindFirstValue(ClaimTypes.NameIdentifier)).ToListAsync();

			return View(modules);
        }

        // GET: Modules/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Module == null)
            {
                return NotFound();
            }

            var @module = await _context.Module.Include("studyDates")
                .FirstOrDefaultAsync(m => m.moduleCode == id);

            if (@module == null)
            {
                return NotFound();
            }


            if(module.studyDates != null)
            {
                List<StudyDate> dates = module.studyDates.OrderBy(o => o.date).ToList();

                List<DataPoint> points = new List<DataPoint>();

                foreach (var d in dates)
                {
                    DataPoint dp = new DataPoint(d.date, d.hoursStudied);
                    points.Add(dp);
                }

                ViewBag.DataPoints = JsonConvert.SerializeObject(points);
            }

            return View(@module);
        }

        // GET: Modules/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("moduleCode,moduleName,credits,classHoursPerWeek,semesterNumOfWeeks,semesterStartDate")] Module @module)
        {
            ModelState.Remove("user");
            if (ModelState.IsValid)
            {     
                User _user = await _context.User.FirstAsync(a => a.username == User.FindFirstValue(ClaimTypes.NameIdentifier));
                @module.user = _user;
                _context.Add(@module);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (UniqueConstraintException ex)
                {
                    ViewData["ValidateMessage"] = ex.Message;
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@module);
        }

        // GET: Modules/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Module == null)
            {
                return NotFound();
            }

            var @module = await _context.Module.FindAsync(id);
            if (@module == null)
            {
                return NotFound();
            }
            return View(@module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("moduleCode,moduleName,credits,classHoursPerWeek,semesterNumOfWeeks,semesterStartDate")] Module @module)
        {
            if (id != @module.moduleCode)
            {
                return NotFound();
            }

            ModelState.Remove("user");
            if (ModelState.IsValid)
            {
                try
                {
                    User _user = await _context.User.FirstAsync(a => a.username == User.FindFirstValue(ClaimTypes.NameIdentifier));
                    module.user = _user;
                    _context.Update(@module);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(@module.moduleCode))
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
            return View(@module);
        }

        // GET: Modules/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Module == null)
            {
                return NotFound();
            }

            var @module = await _context.Module
                .FirstOrDefaultAsync(m => m.moduleCode == id);
            if (@module == null)
            {
                return NotFound();
            }

            return View(@module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Module == null)
            {
                return Problem("Entity set 'Study_TrackerContext.Module'  is null.");
            }
            var @module = await _context.Module.FindAsync(id);
            if (@module != null)
            {
                _context.Module.Remove(@module);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModuleExists(string id)
        {
          return _context.Module.Any(e => e.moduleCode == id);
        }

        public async Task<IActionResult> AddStudyTime(string id)
        {
            if (id == null || _context.Module == null)
            {
                return NotFound();
            }

            var @module = await _context.Module.Include("studyDates")
                .FirstOrDefaultAsync(m => m.moduleCode == id);
            if (@module == null)
            {
                return NotFound();
            }

            ViewData["ModuleHoursThisWeek"] = module.HoursStudiedThisWeek;
            ViewData["ModuleRecommendedHours"] = module.RecommendedStudyHours;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudyTime(string id, [Bind("hoursStudied")] StudyDate studyDate)
        {
            if (id == null || _context.Module == null)
            {
                return NotFound();
            }

            var @module = await _context.Module.FindAsync(id);
            if (@module == null)
            {
                return NotFound();
            }

            ModelState.Remove("module");
            ModelState.Remove("date");

            studyDate.module = module;
            studyDate.date = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(studyDate);
                    await _context.SaveChangesAsync();
                }
                catch (UniqueConstraintException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@studyDate);
        }
    }
}
