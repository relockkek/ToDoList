using ToDoList1.Models;

namespace ToDoList1;

public partial class NewProjectPage : ContentPage
{
    private readonly DB db = new DB();

    public NewProjectPage()
    {
        InitializeComponent();
    }

    private async void SaveProject_Clicked(object sender, EventArgs e)
    {
        string name = ProjectNameEntry.Text?.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            await DisplayAlert("Ошибка", "Введите название проекта", "OK");
            return;
        }

        try
        {
            int newId = await db.GetNextProjectIdAsync();
            var newProject = new Project
            {
                Id = newId,
                Name = name,
                Description = ProjectDescEntry.Text?.Trim() ?? string.Empty
            };

            await db.AddProjectsAsync(newProject);
            await DisplayAlert("Успех", "Проект создан!", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", ex.Message, "OK");
        }
    }
}