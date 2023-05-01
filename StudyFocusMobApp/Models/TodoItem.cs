using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyFocusMobApp.Models
{
    [Table("todolist")]
    public class TodoItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        
        [MaxLength(100)]
        public string Description { get; set; }
        //public bool Done { get; set; }
    }
}
