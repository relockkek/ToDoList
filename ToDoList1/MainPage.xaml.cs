
using System.Threading.Tasks;
using ToDoList1.Models;
namespace ToDoList1
{
    public partial class MainPage : ContentPage
    {
       private DB db = new DB();
        public MainPage()
        {
            InitializeComponent();
        }
        async void LoadData()
        {
            var projects = await db.GetProjectsAsync();
            ProjectsList.ItemsSource = projects;
        }

        private async void AddProject_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewProjectPage());
        }

        protected override void OnAppearing()
        {
            LoadData();
        }

        private async void OpenProject(object sender, EventArgs e)
        {
           Project project = (Project)((Button)sender).BindingContext;
           //await Navigation.PushAsync(new WindowProject());
        }
    }
}
