using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

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
            await Task.Delay(500);
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
            await Task.Delay(500);
            string projectFile = Path.Combine(FileSystem.AppDataDirectory, "projects.json");
            string projectsJson = JsonSerializer.Serialize(projects);
            await File.WriteAllTextAsync(projectFile, projectsJson);
        }

        public async Task<List<Project>> GetProjectsAsync()
        {
            await Task.Delay(500);
            await LoadProjectsAsync();
            return projects.ToList();
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            await Task.Delay(500);
            await LoadProjectsAsync();
            return projects.FirstOrDefault(p => p.Id == id);
        }

        public async Task AddProjectsAsync(Project project)
        {
            await Task.Delay(500);
            projects.Add(project);
            await SaveProjectsAsync();
        }

        public async Task UpdateProjectAsync(Project project)
        {
            await Task.Delay(500);
            await LoadProjectsAsync();
            var index = projects.FindIndex(p => p.Id == project.Id);
            if (index == -1)
                throw new ArgumentException("Проект не найден");

            projects[index] = project;
            await SaveProjectsAsync();
        }

        public async Task DeleteProjectAsync(int projectId)
        {
            await Task.Delay(500);
            await LoadProjectsAsync();
            await LoadTasksAsync();

            //if (tasks.Any(t => t.ProjectId == projectId))
            //{
            //    throw new InvalidOperationException("Нельзя удалить проект: у него есть задачи.");
            //}

            projects.RemoveAll(p => p.Id == projectId);
            await SaveProjectsAsync();
        }

        public async Task<int> GetNextProjectIdAsync()
        {
            await Task.Delay(500);
            await LoadProjectsAsync();
            return projects.Count > 0 ? projects.Max(p => p.Id) + 1 : 1;
        }

        

        public async Task LoadTasksAsync()
        {
            await Task.Delay(500);
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
            await Task.Delay(500);
            string taskFile = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");
            string tasksJson = JsonSerializer.Serialize(tasks);
            await File.WriteAllTextAsync(taskFile, tasksJson);
        }

        public async Task<ObservableCollection<Tasks>> GetTasksAsync()
        {
            await Task.Delay(500);
            await LoadTasksAsync();
            return new ObservableCollection<Tasks>(tasks);
        }

        public async Task<Tasks?> GetTaskByIdAsync(int id)
        {
            await Task.Delay(500);
            await LoadTasksAsync();
            return tasks.FirstOrDefault(t => t.Id == id);
        }

        public async Task AddTaskAsync(Tasks task)
        {
            await Task.Delay(500);
            tasks.Add(task);
            await SaveTasksAsync();
        }

        public async Task UpdateTaskAsync(Tasks task)
        {
            await Task.Delay(500);
            await LoadTasksAsync();
            var index = tasks.FindIndex(t => t.Id == task.Id);
            if (index == -1)
                throw new ArgumentException("Задача не найдена");

            tasks[index] = task;
            await SaveTasksAsync();
        }

        public async Task DeleteTaskAsync(int taskId)
        {
            await Task.Delay(500);
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
            await Task.Delay(500);
            await LoadTasksAsync();
            return tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;
        }
    }
}