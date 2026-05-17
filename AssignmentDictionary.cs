using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace SigmaNotificationApp
{
    public class AssignmentDictionary
    {
        private Dictionary<string, string> tachoBikeAssignments;

        public AssignmentDictionary()
        {
            tachoBikeAssignments = new Dictionary<string, string>();
        }

        public void AddAssignment(string tacho, string bike)
        {
            if (!tachoBikeAssignments.ContainsKey(tacho))
            {
                tachoBikeAssignments.Add(tacho, bike);
            }
            else
            {
                tachoBikeAssignments[tacho] = bike; // Update existing assignment
            }
        }

        public string GetBikeForTacho(string tacho)
        {
            if (tachoBikeAssignments.TryGetValue(tacho, out string bike))
            {
                return bike;
            }
            return null; // No assignment found
        }

        public void RemoveAssignment(string bike)
        {
            if (tachoBikeAssignments.ContainsKey(bike))
            {
                tachoBikeAssignments.Remove(bike);
            }
        }

        public void SaveSettings()
        {
            string json = JsonSerializer.Serialize(tachoBikeAssignments);
            Properties.Settings.Default.AssignmentJson = json;
            Properties.Settings.Default.Save();
        }

        public void LoadSettings()
        {
            string json = Properties.Settings.Default.AssignmentJson;
            if (!string.IsNullOrEmpty(json))
            {
                tachoBikeAssignments = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            }
        }

        public Dictionary<string, string> GetAllAssignments()
        {
            return new Dictionary<string, string>(tachoBikeAssignments);
        }
    }
}
