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
        public int Id { get; set; }
        
        [MaxLength(100)]
        public string Description { get; set; }
        
    }
}
