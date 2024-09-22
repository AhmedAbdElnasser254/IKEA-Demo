using LinkDev.IKEA.DAL.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.DAL.Entities.Department
{
    public class Department : ModelBase
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; } 
        public DateOnly CreationDate { get; set; }

        // Navigational Property [Many]
        public virtual  ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
