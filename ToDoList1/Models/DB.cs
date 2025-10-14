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
    }
}
