using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcProject.Data;
using MvcProject.Models;

namespace MvcProject.Controllers
{
    public class PoliclinicsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PoliclinicsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Policlinics
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Policlinics.Include(p => p.Major);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Policlinics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Policlinics == null)
            {
                return NotFound();
            }

            var policlinic = await _context.Policlinics
                .Include(p => p.Major)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policlinic == null)
            {
                return NotFound();
            }

            return View(policlinic);
        }

        // GET: Policlinics/Create
        public IActionResult Create()
        {
            ViewData["MajorId"] = new SelectList(_context.Majors, "Id", "Name");
            return View();
        }

        // POST: Policlinics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,MajorId")] Policlinic policlinic)
        {
            _context.Add(policlinic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Policlinics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Policlinics == null)
            {
                return NotFound();
            }

            var policlinic = await _context.Policlinics.FindAsync(id);
            if (policlinic == null)
            {
                return NotFound();
            }
            ViewData["MajorId"] = new SelectList(_context.Majors, "Id", "Name", policlinic.MajorId);
            return View(policlinic);
        }

        // POST: Policlinics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,MajorId")] Policlinic policlinic)
        {
            if (id != policlinic.Id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(policlinic);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PoliclinicExists(policlinic.Id))
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

        // GET: Policlinics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Policlinics == null)
            {
                return NotFound();
            }

            var policlinic = await _context.Policlinics
                .Include(p => p.Major)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policlinic == null)
            {
                return NotFound();
            }

            return View(policlinic);
        }

        // POST: Policlinics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Policlinics == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Policlinics'  is null.");
            }
            var policlinic = await _context.Policlinics.FindAsync(id);
            if (policlinic != null)
            {
                _context.Policlinics.Remove(policlinic);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PoliclinicExists(int id)
        {
            return (_context.Policlinics?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
