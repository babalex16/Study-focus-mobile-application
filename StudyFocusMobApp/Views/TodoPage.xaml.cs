using StudyFocusMobApp.Models;

namespace StudyFocusMobApp;

public partial class TodoPage : ContentPage
{
	public TodoPage()
	{
		InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        OnGetButtonClicked(this, EventArgs.Empty);
    }

    public async void OnNewButtonClicked(object sender, EventArgs args)
    {

        await App.TodoSvc.AddNewTodoItem(newTodoItem.Text);
        List<TodoItem> todos = await App.TodoSvc.GetAllTodoItems();
        todoList.ItemsSource = todos;
    }

    public async void OnGetButtonClicked(object sender, EventArgs args)
    {
        List<TodoItem> todos = await App.TodoSvc.GetAllTodoItems();
        todoList.ItemsSource = todos;
    }
    
    //TODO
    private async void taskCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        //statusMessage.Text = "";

        
        //int id = int.Parse("1");
        //await App.TodoSvc.DeleteTodoItem(id);
        //statusMessage.Text = App.TodoSvc.StatusMessage;
    }

    private async void deleteButton_Clicked(object sender, EventArgs e)
    {

        int id = int.Parse("10");
        await App.TodoSvc.DeleteTodoItem(id);
    }
}