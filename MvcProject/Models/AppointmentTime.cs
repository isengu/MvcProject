using System;
using System.Collections.Generic;

namespace MvcProject.Models
{
    public partial class AppointmentTime
    {
        public AppointmentTime()
        {
            Appointments = new HashSet<Appointment>();
        }

        public int Id { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }

        public override string ToString()
        {
            return StartTime.ToString() + " - " + EndTime.ToString();
        }
    }
}
