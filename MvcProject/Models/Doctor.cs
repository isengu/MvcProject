using System;
using System.Collections.Generic;

namespace MvcProject.Models
{
    public partial class Doctor
    {
        public Doctor()
        {
            Appointments = new HashSet<Appointment>();
        }

        public int Id { get; set; }
        public string Phone { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int PoliclinicId { get; set; }

        public virtual Policlinic Policlinic { get; set; } = null!;
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
