using ToDoList1.Models;

namespace ToDoList1;

public partial class MainPage : ContentPage
{
    private readonly DB db = new DB();

    public MainPage()
    {
        InitializeComponent();
        LoadData();
    }

    private async void LoadData()
    {
        await db.LoadAllAsync();
        ProjectsList.ItemsSource = await db.GetProjectsAsync();
    }

    private async void AddProject_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NewProjectPage());
    }

    private async void OpenProject_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var project = (Project)button.CommandParameter;
        await Navigation.PushAsync(new ProjectDetailsPage(project));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadData();
    }
}