using ToDoList1.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList1;

public partial class ProjectDetailsPage : ContentPage
{
    private readonly DB db = new();
    private Project currentProject; 

    public ProjectDetailsPage(Project project)
    {
        InitializeComponent();
        currentProject = project;
        LoadProjectData();
        _ = LoadTasks(); 
    }

    private void LoadProjectData()
    {
        ProjectTitleLabel.Text = currentProject.Name;
        ProjectDescriptionLabel.Text = currentProject.Description ?? "Нет описания";
        Title = currentProject.Name;
    }

    private async Task LoadTasks()
    {
        var tasks = await db.GetTasksAsync();
        var projectTasks = tasks.Where(t => t.ProjectId == currentProject.Id).ToList();
        TasksList.ItemsSource = projectTasks;
    }

    

    private void EditButton_Clicked(object sender, EventArgs e)
    {
        EditNameEntry.Text = currentProject.Name;
        EditDescEditor.Text = currentProject.Description ?? string.Empty;

        ViewModeGrid.IsVisible = false;
        EditModeGrid.IsVisible = true;
        EditButton.IsVisible = false;
    }

    private async void SaveEdit_Clicked(object sender, EventArgs e)
    {
        var name = EditNameEntry.Text?.Trim();
        if (string.IsNullOrEmpty(name))
        {
            await DisplayAlert("Ошибка", "Название обязательно", "OK");
            return;
        }

        currentProject.Name = name;
        currentProject.Description = EditDescEditor.Text?.Trim() ?? string.Empty;

        try
        {
            await db.UpdateProjectAsync(currentProject);
            LoadProjectData();
            SwitchToViewMode();
            await DisplayAlert("Успех", "Проект обновлён", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", ex.Message, "OK");
        }
    }

    private void CancelEdit_Clicked(object sender, EventArgs e)
    {
        SwitchToViewMode();
    }

    private void SwitchToViewMode()
    {
        ViewModeGrid.IsVisible = true;
        EditModeGrid.IsVisible = false;
        EditButton.IsVisible = true;
    }

    

    private async void AddTask_Clicked(object sender, EventArgs e)
    {
        var newTask = new Tasks
        {
            Id = await db.GetNextTaskIdAsync(),
            Name = "Новая задача",
            Description = "Описание задачи",
            ProjectId = currentProject.Id,
            IsCompleted = false
        };

        await db.AddTaskAsync(newTask);
        await LoadTasks();
    }

    private async void EditTask_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var task = (Tasks)button.CommandParameter;

        var newName = await DisplayPromptAsync("Название", "Введите название:", initialValue: task.Name);
        if (newName == null) return;

        var newDesc = await DisplayPromptAsync("Описание", "Введите описание:", initialValue: task.Description);
        if (newDesc == null) return;

        task.Name = newName;
        task.Description = newDesc;

        await db.UpdateTaskAsync(task);
        await LoadTasks();
    }

    private async void DeleteTask_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var task = (Tasks)button.CommandParameter;

        var confirm = await DisplayAlert("Удаление", $"Удалить задачу '{task.Name}'?", "Да", "Нет");
        if (confirm)
        {
            await db.DeleteTaskAsync(task.Id);
            await LoadTasks();
        }
    }

    private async void TaskCheckbox_Changed(object sender, CheckedChangedEventArgs e)
    {
        var checkBox = (CheckBox)sender;
        var task = (Tasks)checkBox.BindingContext;
        if (task != null)
        {
            task.IsCompleted = checkBox.IsChecked;
            await db.UpdateTaskAsync(task);
        }
    }

  

    private async void DeleteProject_Clicked(object sender, EventArgs e)
    {
        try
        {
            await db.DeleteProjectAsync(currentProject.Id);
            await DisplayAlert("Успех", "Проект удалён", "OK");
            await Navigation.PopAsync();
        }
        catch (InvalidOperationException ex)
        {
            await DisplayAlert("Ошибка", ex.Message, "OK");
        }
    }
}