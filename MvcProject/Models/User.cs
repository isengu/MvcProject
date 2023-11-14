using System;
using System.Collections.Generic;

namespace MvcProject.Models
{
    public partial class User
    {
        public User()
        {
            Appointments = new HashSet<Appointment>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        /// <summary>
        /// 0 -&gt; admin, 1 -&gt; patient
        /// </summary>
        public int Type { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
