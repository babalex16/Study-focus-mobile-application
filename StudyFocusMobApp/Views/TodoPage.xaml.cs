using StudyFocusMobApp.Models;

namespace StudyFocusMobApp;

public partial class TodoPage : ContentPage
{
	public TodoPage()
	{
		InitializeComponent();
	}
    public async void OnNewButtonClicked(object sender, EventArgs args)
    {
        statusMessage.Text = "";

        await App.TodoSvc.AddNewTodoItem(newTodoItem.Text);
        statusMessage.Text = App.TodoSvc.StatusMessage;
    }

    public async void OnGetButtonClicked(object sender, EventArgs args)
    {
        statusMessage.Text = "";

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

    private async void Button_Clicked(object sender, EventArgs e)
    {
        statusMessage.Text = "";


        int id = int.Parse("1");
        await App.TodoSvc.DeleteTodoItem(id);
        statusMessage.Text = App.TodoSvc.StatusMessage;
    }
}