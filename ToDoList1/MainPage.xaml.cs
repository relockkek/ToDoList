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
        var projects = await db.GetProjectsAsync();
        ProjectsList.ItemsSource = projects;
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
    private async void DeleteProject_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var project = (Project)button.CommandParameter;

        var confirm = await DisplayAlert("Удаление", $"Удалить проект '{project.Name}'?", "Да", "Нет");
        if (confirm)
        {
            await db.DeleteProjectAsync(project.Id);
            LoadData();
        }
    }

    private async void TagsRedact_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var tags = (Tags)button.CommandParameter;
        await Navigation.PushAsync(new TagsPage(tags));
    }
}