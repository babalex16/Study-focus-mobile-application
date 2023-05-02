using SQLite;
using StudyFocusMobApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyFocusMobApp.Services
{
    public class TodoService
    {
        string _dbPath;

        public string StatusMessage { get; set; }

        private SQLiteAsyncConnection conn;

        private async Task Init()
        {
            if (conn != null)
                return;

            conn = new SQLiteAsyncConnection(_dbPath);
            await conn.CreateTableAsync<TodoItem>();
        }

        public TodoService(string dbPath)
        {
            _dbPath = dbPath;
        }

        public async Task AddNewTodoItem(string description)
        {
            int result = 0;
            try
            {
                await Init();

                // basic validation to ensure a description was entered
                if (string.IsNullOrEmpty(description))
                    throw new Exception("Valid description required");

                result = await conn.InsertAsync(new TodoItem { Description = description });

                StatusMessage = string.Format("{0} record(s) added [Description: {1})", result, description);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", description, ex.Message);
            }

        }

        public async Task DeleteTodoItem(int id)
        {
            int result = 0;
            try
            {
                await Init();
                result = await conn.DeleteAsync(id);
                StatusMessage = string.Format("{0} record(s) deleted )", result);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to delete . Error: {1}", ex.Message);
            }
        }

        public async Task<List<TodoItem>> GetAllTodoItems()
        {
            try
            {
                await Init();
                return await conn.Table<TodoItem>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<TodoItem>();
        }
    }
}
