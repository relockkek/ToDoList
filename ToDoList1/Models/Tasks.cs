using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList1.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int? ProjectId { get; set; }
        public Projects Projects { get; set; }
        public int? TagId { get; set; }
        public Tags Tags { get; set; }
        
    }
}
