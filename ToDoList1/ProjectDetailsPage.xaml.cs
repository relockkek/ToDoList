using ToDoList1.Models;
using System.Linq;
using System.IO;
using Microsoft.Maui.Storage;
using System.Text.Json;

namespace ToDoList1;

public partial class ProjectDetailsPage : ContentPage
{
    private readonly DB db = new DB();
    private readonly Project currentProject;

    public ProjectDetailsPage(Project project)
    {
        InitializeComponent(); 

        currentProject = project;
        LoadProjectData();
        LoadTasks();
    }

    private void LoadProjectData()
    {
        ProjectTitle.Text = currentProject.Name;
        ProjectDescription.Text = currentProject.Description ?? "Нет описания";
        Title = currentProject.Name;
    }

    private async Task LoadTasks()
    {
        var tasks = await db.GetTasksAsync();
        var projectTasks = tasks.Where(t => t.ProjectId == currentProject.Id).ToList();
        TasksList.ItemsSource = projectTasks;
    }

    private async void AddTask_Clicked(object sender, EventArgs e)
    {
        var newTask = new Tasks
        {
            Id = await GetNextTaskIdAsync(),
            Name = "Новая задача",
            Description = "Описание задачи",
            ProjectId = currentProject.Id,
            IsCompleted = false
        };

        await db.AddTaskAsync(newTask);
        await LoadTasks();
    }

    private async void DeleteTask_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var task = (Tasks)button.CommandParameter;

        bool answer = await DisplayAlert("Удаление",
            $"Удалить задачу '{task.Name}'?", "Да", "Нет");

        if (answer)
        {
            await db.DeleteTaskAsync(task.Id);
            await LoadTasks();
        }
    }

    private void TaskCheckbox_Changed(object sender, CheckedChangedEventArgs e)
    {
      
    }

    private async void Back_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async Task<int> GetNextTaskIdAsync()
    {
        var tasks = await db.GetTasksAsync();
        return tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;
    }

    private void DeleteProject_Clicked(object sender, EventArgs e)
    {

    }
}