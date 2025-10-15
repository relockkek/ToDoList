using System;
using System.Collections.Generic;
using System.Linq;
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
            string projectFile = Path.Combine(FileSystem.AppDataDirectory, "projects.json");
            if (File.Exists(projectFile))
            {
                string json = await File.ReadAllTextAsync(projectFile);
                projects = System.Text.Json.JsonSerializer.Deserialize<List<Projects>>(json) ?? new List<Projects>();
            }

            string taskFile = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");
            if (File.Exists(taskFile))
            {
                string json = await File.ReadAllTextAsync(taskFile);
                tasks = System.Text.Json.JsonSerializer.Deserialize<List<Tasks>>(json) ?? new List<Tasks>();
            }
        }
        public async Task SaveAllSync()
        {
            string projectFile = Path.Combine(FileSystem.AppDataDirectory, "projects.json");
            string projectsJson = System.Text.Json.JsonSerializer.Serialize(projects);
            await File.WriteAllTextAsync(projectFile, projectsJson);

            string taskFile = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");
            string tasksJson = System.Text.Json.JsonSerializer.Serialize(tasks);
            await File.WriteAllTextAsync(taskFile, tasksJson);
        }
        //для получения данных
        public List<Projects> GetProjects() => projects;
        public List<Tasks> GetTasks() => tasks;
        public List<Tags> GetTags() => tags;
        public List<PodTasks> GetPodTasks() => podTasks; 
        //для добавления данных
        public void AddProject (Projects project)
        {
            projects.Add(project);
        }

        public void AddTask(Tasks task)
        {
            tasks.Add(task);
        }
        //след. id проекта
        public int GetNextProjectId()
        {
            return projects.Count > 0 ? projects.Max(p => p.Id) + 1 : 1;
        }

        public int GetNextTaskId()
        {
            return tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;
        }
    }

}
