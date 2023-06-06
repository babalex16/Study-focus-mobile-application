using SQLite;

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
