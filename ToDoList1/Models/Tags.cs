using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList1.Models
{
    public class Tags
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}
