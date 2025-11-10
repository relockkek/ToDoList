using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Formats.Tar;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ToDoList1.Models
{

    public class DB
    {

        private List<Project> projects = new();
        private List<Tasks> tasks = new();
        private List<Tags> tags = new();
        private List<PodTasks> podTasks = new();
        public async Task LoadAllAsync()
        {
            await LoadProjectsAsync();
            await LoadTasksAsync();
        }
        public async Task LoadProjectsAsync()
        {
            await Task.Delay(100);
            string projectFile = Path.Combine(FileSystem.AppDataDirectory, "projects.json");
            if (File.Exists(projectFile))
            {
                string json = await File.ReadAllTextAsync(projectFile);
                var loadedProjects = JsonSerializer.Deserialize<List<Project>>(json) ?? new List<Project>();

                projects.Clear();
                projects.AddRange(loadedProjects);
            }
        }

        public async Task SaveProjectsAsync()
        {
            await Task.Delay(100);
            string projectFile = Path.Combine(FileSystem.AppDataDirectory, "projects.json");
            string projectsJson = JsonSerializer.Serialize(projects);
            await File.WriteAllTextAsync(projectFile, projectsJson);
        }

        public async Task<List<Project>> GetProjectsAsync()
        {
            await Task.Delay(100);
            return projects.ToList();
        }
        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            await Task.Delay(100);
            return projects.FirstOrDefault(p => p.Id == id);
        }

        public async Task AddProjectsAsync(Project project)
        {
            await Task.Delay(100);
            projects.Add(project);
            await SaveProjectsAsync();
        }

        public async Task UpdateProjectAsync(Project project)
        {
            await Task.Delay(100);
            await LoadProjectsAsync();
            var index = projects.FindIndex(p => p.Id == project.Id);
            if (index == -1)
                throw new ArgumentException("Проект не найден");

            projects[index] = project;
            await SaveProjectsAsync();
        }

        public async Task DeleteProjectAsync(int projectId)
        {
            await Task.Delay(100);
            await LoadProjectsAsync();
            await LoadTasksAsync();

            if (tasks.Any(t => t.ProjectId == projectId))
            {
                Application.Current.MainPage.DisplayAlert("Ошибка", "У проекта есть задачи", "ОК");
            }
            else
                projects.RemoveAll(p => p.Id == projectId);
            await SaveProjectsAsync();
        }

        public async Task<int> GetNextProjectIdAsync()
        {
            await Task.Delay(100);
            await LoadProjectsAsync();
            return projects.Count > 0 ? projects.Max(p => p.Id) + 1 : 1;
        }



        public async Task LoadTasksAsync()
        {
            await Task.Delay(100);
            string taskFile = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");
            if (File.Exists(taskFile))
            {
                string json = await File.ReadAllTextAsync(taskFile);
                var loadedTasks = JsonSerializer.Deserialize<List<Tasks>>(json) ?? new List<Tasks>();

                tasks.Clear();
                tasks.AddRange(loadedTasks);
            }
        }

        public async Task SaveTasksAsync()
        {
            await Task.Delay(100);
            string taskFile = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");
            string tasksJson = JsonSerializer.Serialize(tasks);
            await File.WriteAllTextAsync(taskFile, tasksJson);
        }

        public async Task<ObservableCollection<Tasks>> GetTasksAsync()
        {
            await Task.Delay(100);
            await LoadTasksAsync();
            return new ObservableCollection<Tasks>(tasks);
        }

        public async Task<Tasks?> GetTaskByIdAsync(int id)
        {
            await Task.Delay(100);
            await LoadTasksAsync();
            return tasks.FirstOrDefault(t => t.Id == id);
        }

        public async Task AddTaskAsync(Tasks task)
        {
            await Task.Delay(100);
            tasks.Add(task);
            await SaveTasksAsync();
        }

        public async Task UpdateTaskAsync(Tasks task)
        {
            await Task.Delay(100);
            await LoadTasksAsync();
            var index = tasks.FindIndex(t => t.Id == task.Id);
            if (index == -1)
                throw new ArgumentException("Задача не найдена");

            tasks[index] = task;
            await SaveTasksAsync();
        }

        public async Task DeleteTaskAsync(int taskId)
        {
            await Task.Delay(100);
            await LoadTasksAsync();

            var taskToRemove = tasks.FirstOrDefault(t => t.Id == taskId);
            if (taskToRemove != null)
            {
                tasks.Remove(taskToRemove);
                await SaveTasksAsync();
            }
        }

        public async Task<int> GetNextTaskIdAsync()
        {
            await Task.Delay(100);
            await LoadTasksAsync();
            return tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;
        }

        public async Task LoadTagsAsync()
        {
            await Task.Delay(100);
            string tagsFile = Path.Combine(FileSystem.AppDataDirectory, "tags.json");
            if (File.Exists(tagsFile))
            {
                string json = await File.ReadAllTextAsync(tagsFile);
                var loadedTags = System.Text.Json.JsonSerializer.Deserialize<List<Tags>>(json) ?? new List<Tags>();
                tags.Clear();
                foreach (var tag in loadedTags)
                {
                    tags.Add(tag);
                }
            }
        }

        public async Task SaveTagsAsync()
        {
            await Task.Delay(100);
            string tagsFile = Path.Combine(FileSystem.AppDataDirectory, "tags.json");
            string tagsJson = System.Text.Json.JsonSerializer.Serialize(tags.ToList());
            await File.WriteAllTextAsync(tagsFile, tagsJson);
        }

        public async Task AddTagAsync (Tags tag)
        {
            await Task.Delay(100);
            tags.Add (tag);
            await SaveTagsAsync();
        }

        public async Task<List<Tags>> GetTagsAsync()
        {
            await LoadTagsAsync();
            return tags.ToList();
        }


        public async Task LoadPodTasksAsync()
        {
            await Task.Delay(100);
            string podTasksFile = Path.Combine(FileSystem.AppDataDirectory, "podtasks.json");
            if (File.Exists(podTasksFile))
            {
                string json = await File.ReadAllTextAsync(podTasksFile);
                var loadedPodTasks = System.Text.Json.JsonSerializer.Deserialize<List<PodTasks>>(json) ?? new List<PodTasks>();
                podTasks.Clear();
                foreach (var podTask in loadedPodTasks)
                {
                    podTasks.Add(podTask);
                }
            }
        }

        public async Task SavePodTasksAsync()
        {
            await Task.Delay(100);
            string podTasksFile = Path.Combine(FileSystem.AppDataDirectory, "podtasks.json");
            string podTasksJson = System.Text.Json.JsonSerializer.Serialize(podTasks.ToList());
            await File.WriteAllTextAsync(podTasksFile, podTasksJson);
        }

        public async Task AddPodTaskAsync(PodTasks podTask)
        {
            await Task.Delay(100);
            podTasks.Add(podTask);
            await SavePodTasksAsync();
        }

        public async Task<List<PodTasks>> GetPodTasksByTaskAsync(int taskId)
        {
            await LoadPodTasksAsync();
            return podTasks.Where(pt => pt.TaskId == taskId).ToList();
        }

        public async Task DeletePodTaskAsync(int podTaskId)
        {
            await Task.Delay(100);
            var podTask = podTasks.FirstOrDefault(pt => pt.Id == podTaskId);
            if (podTask != null)
            {

                await SavePodTasksAsync();
            }
        }
    }

}