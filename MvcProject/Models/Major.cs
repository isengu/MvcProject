using System;
using System.Collections.Generic;

namespace MvcProject.Models
{
    public partial class Major
    {
        public Major()
        {
            Policlinics = new HashSet<Policlinic>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Policlinic> Policlinics { get; set; }
    }
}
