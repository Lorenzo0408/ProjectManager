using Newtonsoft.Json;
using System.IO;
using ProjectManager.Model;

namespace ProjectManager.Services
{
    public static class SaveLoadService
    {
        public static void SaveDashboard(DashboardModel dashboard, string filePath)
        {
            var json = JsonConvert.SerializeObject(dashboard, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static DashboardModel LoadDashboard(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<DashboardModel>(json);
        }
    }
}
