using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList1.Models
{
    public class Projects
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Tasks> Tasks { get; set; } = new();
    }
}
