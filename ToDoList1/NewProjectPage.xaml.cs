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
                var projects = await db.GetProjectsAsync();
                int newId = projects.Count > 0 ? projects.Max(p => p.Id) + 1 : 1;
                var newProject = new Projects
                {
                    Id = newId,
                    Name = name,
                    Description = description
                };

                await db.AddProjectsAsync(newProject);
                await DisplayAlert("Успех", "Проект добавлен", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "OK");
            } 
        }
        else
        {
            await DisplayAlert("Ошибка", "Введите название проекта", "Ок");
        }
    }
}