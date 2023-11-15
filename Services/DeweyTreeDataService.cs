using BookBuddy.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookBuddy.Services
{
    public class DeweyTreeDataService
    {
        public List<DeweyTreeNode> LoadData()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFilePath = Path.Combine(baseDirectory, "Resources", "Files", "deweyDataset.json");
            string jsonData = File.ReadAllText(jsonFilePath);
            return JsonConvert.DeserializeObject<List<DeweyTreeNode>>(jsonData);
        }
    }
}
