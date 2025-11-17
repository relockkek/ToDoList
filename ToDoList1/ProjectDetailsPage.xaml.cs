using ToDoList1.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

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
        _ = LoadTasksAsync();
        _ = LoadTagsAsync();
    }

    private void LoadProjectData()
    {
        ProjectTitleLabel.Text = currentProject.Name;
        ProjectDescriptionLabel.Text = currentProject.Description ?? "Нет описания";
        Title = currentProject.Name;
    }

    async Task LoadTasksAsync()
    {
        try
        {
            var tasks = await db.GetTasksAsync();
            var projectTasks = tasks.Where(t => t.ProjectId == currentProject.Id).ToList();

            foreach (var task in projectTasks )
            {
                task.PodTasks = await db.GetPodTasksByTaskAsync(task.Id);
            }
            TasksList.ItemsSource = projectTasks;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", ex.Message, "ОК");
        }
    }
    async Task LoadTagsAsync()
    {
        try
        {
            var tags = await db.GetTagsAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", ex.Message, "ОК");
        }
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
        await LoadTasksAsync();
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
        await LoadTasksAsync();
    }

    private async void DeleteTask_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var task = (Tasks)button.CommandParameter;

        var confirm = await DisplayAlert("Удаление", $"Удалить задачу '{task.Name}'?", "Да", "Нет");
        if (confirm)
        {
            await db.DeleteTaskAsync(task.Id);
            await LoadTasksAsync();
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


    private async void AddPodTask_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var task = (Tasks)button.CommandParameter;

        string podTaskTitle = await DisplayPromptAsync("Новая подзадача", "Введите название");
        
        if (!string.IsNullOrWhiteSpace(podTaskTitle))
        {
            var podTasks = await db.GetPodTasksByTaskAsync(task.Id);
            int newId = podTasks.Count > 0 ? podTasks.Max(pt => pt.Id) + 1 : 1;

            var newPodTask = new PodTasks
            {
                Id = newId,
                Title = podTaskTitle,
                IsCompleted = true,
                TaskId = task.Id
            };

            await db.AddPodTaskAsync(newPodTask);
            await LoadTasksAsync();
        }

    }

    private async void Tags_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("TagsPage");
    }

    private async void Back_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void DeletePodTask_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var podTask = (PodTasks)button.CommandParameter;

        bool answer = await DisplayAlert("Удаление",
            $"Удалить задачу '{podTask.Title}'?", "Да", "Нет");

        if (answer)
        {
            await db.DeletePodTaskAsync(podTask.Id);
            await LoadTasksAsync();
        }
    }
    
}