using StudyFocusMobApp.Models;

namespace StudyFocusMobApp;

public partial class TodoPage : ContentPage
{
    public TodoPage()
    {
        InitializeComponent();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await RefreshTodoItems();
    }

    private async Task RefreshTodoItems()
    {
        List<TodoItem> todos = await App.TodoSvc.GetAllTodoItems();
        todoList.ItemsSource = todos;
    }

    private async void OnSwipeDeleteClicked(object sender, EventArgs e)
    {
        var swipeItem = (SwipeItem)sender;
        if (swipeItem.CommandParameter is TodoItem todoItem)
        {
            await App.TodoSvc.DeleteTodoItem(todoItem.Id);
            await RefreshTodoItems();
        }
    }

    public async void OnNewButtonClicked(object sender, EventArgs args)
    {
        await App.TodoSvc.AddNewTodoItem(newTodoItem.Text);
        await RefreshTodoItems();
        newTodoItem.Text = ""; // Clear the input field
    }
}