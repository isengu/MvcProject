using System;
using System.Collections.Generic;

namespace MvcProject.Models
{
    public partial class Appointment
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public DateOnly Date { get; set; }
        public int AppointmentTimeId { get; set; }

        public virtual AppointmentTime AppointmentTime { get; set; } = null!;
        public virtual Doctor Doctor { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
