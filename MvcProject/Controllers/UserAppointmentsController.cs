using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcProject.Data;
using MvcProject.DTO;
using MvcProject.Models;

namespace MvcProject.Controllers
{
    [Authorize(Roles = "Patient")]
    public class UserAppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserAppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");

            return _context.Appointments != null ?
                          View(await _context.Appointments
                          .Where(e => e.UserId == userId)
                          .Include(e => e.AppointmentTime)
                          .Include(e => e.Doctor)
                          .Include(e => e.Doctor.Policlinic)
                          .Include(e => e.Doctor.Policlinic.Major)
                          .ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Appointments'  is null.");
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(e => e.AppointmentTime)
                .Include(e => e.Doctor)
                .Include(e => e.Doctor.Policlinic)
                .Include(e => e.Doctor.Policlinic.Major)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Search(LoadAppointmentsDTO request)
        {
            List<AppointmentDateDTO> dates = new();

            List<AppointmentDoctorDTO> doctors = (from doctor in _context.Doctors
                                                  where doctor.PoliclinicId == request.PoliclinicId
                                                  select new AppointmentDoctorDTO
                                                  {
                                                      Id = doctor.Id,
                                                      Name = doctor.Name
                                                  }).ToList();

            List<AppointmentTimeDTO> allTimes = (from time in _context.AppointmentTimes
                                                 select new AppointmentTimeDTO
                                                 {
                                                     Id = time.Id,
                                                     Time = time.StartTime.ToString() + " - " + time.EndTime.ToString()
                                                 }).ToList();

            var allAppointments = (from apn in _context.Appointments
                                   where apn.Date >= request.StartDate
                                   && apn.Date <= request.EndDate
                                   select apn);

            for (DateOnly i = request.StartDate; i <= request.EndDate; i = i.AddDays(1))
            {
                foreach (var doctor in doctors)
                {
                    var appointments = allAppointments
                        .Where(e => e.Date.Equals(i) && e.DoctorId == doctor.Id)
                        .Select(e => e.AppointmentTimeId);
                    doctor.Times = allTimes.Where(e => !appointments.Contains(e.Id)).ToList();
                }

                var policlinic = _context.Policlinics.Where(e => e.Id == request.PoliclinicId).Include(e => e.Major).FirstOrDefault();

                dates.Add(new AppointmentDateDTO
                {
                    Major = policlinic?.Major?.Name,
                    Policlinic = policlinic.Name,
                    Date = i,
                    Doctors = doctors.Select(j => new AppointmentDoctorDTO { Id = j.Id, Name = j.Name, Times = j.Times }).ToList()
                });
            }

            return View(dates);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            var applicationDbContext = _context.Appointments.Include(a => a.AppointmentTime).Include(a => a.Doctor).Include(a => a.User);
            ViewData["PoliclinicId"] = new SelectList(_context.Policlinics, "Id", "Name", null, "MajorId");
            ViewData["MajorId"] = new SelectList(_context.Majors, "Id", "Name");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentDTO appointment)
        {
            _context.Add(new Appointment
            {
                Id = appointment.Id,
                Date = appointment.Date,
                AppointmentTimeId = appointment.AppointmentTimeId,
                DoctorId = appointment.DoctorId,
                UserId = appointment.UserId
            });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["AppointmentTimeId"] = new SelectList(_context.AppointmentTimes, "Id", "Id", appointment.AppointmentTimeId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", appointment.DoctorId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", appointment.UserId);
            ViewData["PoliclinicId"] = new SelectList(_context.Policlinics, "Id", "Name");
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DoctorId,UserId,Date,AppointmentTimeId")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
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
            ViewData["AppointmentTimeId"] = new SelectList(_context.AppointmentTimes, "Id", "Id", appointment.AppointmentTimeId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", appointment.DoctorId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", appointment.UserId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(e => e.AppointmentTime)
                .Include(e => e.Doctor)
                .Include(e => e.Doctor.Policlinic)
                .Include(e => e.Doctor.Policlinic.Major)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Appointments == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Appointments'  is null.");
            }
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return (_context.Appointments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
