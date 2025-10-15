using System.Threading.Tasks;
using ToDoList1.Models;

namespace ToDoList1;

public partial class NewProjectPage : ContentPage
{
    private DB db = new DB();
    public NewProjectPage()
    {
        InitializeComponent();
    }

    private async void SaveProject_Clicked(object sender, EventArgs e)
    {
        string name = ProjectNameEntry.Text;
        string description = ProjectDescEntry.Text;
        if (!string.IsNullOrWhiteSpace(name))
        {
            try
            {
                var newProject = new Projects
                {
                    Id = Id
                }
                await db.SaveAllSync();
                await DisplayAlert("Успех", "Проект добавлен", "OK");
                await Navigation.PopAsync();
                return db.
            }
        }
    }
}