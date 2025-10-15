
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
            LoadData();
        }
        async void LoadData()
        {
           await db.LoadAllAsync();
            ProjectsList.ItemsSource = db.GetProjects();
        }

        private async void AddProject_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewProjectPage());
        }
    }
}
