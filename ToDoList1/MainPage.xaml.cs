using System.Threading.Tasks;

namespace ToDoList1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private async void NewProject_Click(object sender, EventArgs e)
        {
            var newProject = new NewProjectPage();
            await Navigation.PushAsync(newProject);
        }
    }
}
