using System;
using System.Collections.Generic;

namespace MvcProject.Models
{
    public partial class Policlinic
    {
        public Policlinic()
        {
            Doctors = new HashSet<Doctor>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int MajorId { get; set; }

        public virtual Major Major { get; set; } = null!;
        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}
