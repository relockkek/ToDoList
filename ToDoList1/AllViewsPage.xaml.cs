using ToDoList1.Models;

namespace ToDoList1;

public partial class AllViewsPage : ContentPage
{
    private readonly DB db = new DB();
    private List<Project> allProjects = new();

    public AllViewsPage()
    {
        InitializeComponent();
        LoadData();
    }

    private async void LoadData()
    {
        await db.LoadAllAsync();
        allProjects = await db.GetProjectsAsync();
        ProjectsListView.ItemsSource = allProjects;
        ProjectsCarousel.ItemsSource = allProjects;
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is Project project)
        {
            ((ListView)sender).SelectedItem = null;
            await Navigation.PushAsync(new ProjectDetailsPage(project));
        }
    }

    private async void Carousel_OpenProject_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var project = (Project)button.CommandParameter;
        await Navigation.PushAsync(new ProjectDetailsPage(project));
    }
}