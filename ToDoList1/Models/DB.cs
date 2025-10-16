using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList1.Models
{
    public class DB
    {
        private List<Projects> projects = new List<Projects>();
        private List<Tasks> tasks = new List<Tasks>();
        private List<Tags> tags = new List<Tags>();
        private List<PodTasks> podTasks = new List<PodTasks>();

        public async Task LoadAllAsync()
        {
            await LoadProjectsAsync();
            await LoadTasksAsync();


        }
        public async Task LoadProjectsAsync()
        {
            await Task.Delay(500);
            string projectFile = Path.Combine(FileSystem.AppDataDirectory, "projects.json");
            if (File.Exists(projectFile))
            {
                string json = await File.ReadAllTextAsync(projectFile);
                var loadedProjects = System.Text.Json.JsonSerializer.Deserialize<List<Projects>>(json) ?? new List<Projects>();

                projects.Clear();
                foreach (var project in loadedProjects)
                {
                    projects.Add(project);
                }
            }
        }

        public async Task SaveProjectsAsync()
        {
            await Task.Delay(500);
            string projectFile = Path.Combine(FileSystem.AppDataDirectory, "projects.json");
            string projectsJson = System.Text.Json.JsonSerializer.Serialize(projects.ToList());
            await File.WriteAllTextAsync(projectFile, projectsJson);
        }

        public async Task AddProjectsAsync(Projects project)
        {
            await Task.Delay(500);
            projects.Add(project);
            await SaveProjectsAsync();
        }
        public async Task LoadTasksAsync()
        {
            await Task.Delay(500);
            string taskFile = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");
            if (File.Exists(taskFile))
            {
                string json = await File.ReadAllTextAsync(taskFile);
                var loadedTasks = System.Text.Json.JsonSerializer.Deserialize<List<Tasks>>(json) ?? new List<Tasks>();

                tasks.Clear();
                foreach (var task in loadedTasks)
                {
                    tasks.Add(task);
                }
            }
        }

        public async Task SaveTasksAsync()
        {
            await Task.Delay(500);
            string taskFile = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");
            string tasksJson = System.Text.Json.JsonSerializer.Serialize(tasks.ToList());
            await File.WriteAllTextAsync(taskFile, tasksJson);
        }
        public async Task AddTaskAsync(Tasks task)
        {
            await Task.Delay(500);
            tasks.Add(task);
            await SaveTasksAsync();
        }

        public async Task<ObservableCollection<Tasks>> GetTasksAsync()
        {
            await Task.Delay(500);
            await LoadTasksAsync();
            return tasks;
        }

        public async Task<int> GetNextProjectIdAsync()
        {
            await Task.Delay(500);
            await LoadProjectsAsync();
            return projects.Count > 0 ? projects.Max(p => p.Id) + 1 : 1;
        }
    }
}

