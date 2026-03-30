using System.Text.Json;

namespace SkillTracker;

public class FileSaver {
    public List<T> LoadData<T>(string filePath) {
        if (!File.Exists(filePath)) {
            return new List<T>();
        }

        string json = File.ReadAllText(filePath);

        if (string.IsNullOrWhiteSpace(json)) {
            return new List<T>();
        }

        var data = JsonSerializer.Deserialize<List<T>>(json);

        if (data == null) {
            return new List<T>();
        }

        return data;
    }

    public void SaveData<T>(string filePath, List<T> data) {
        string? directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory)) {
            Directory.CreateDirectory(directory);
        }
        string json = JsonSerializer.Serialize(data);
        File.WriteAllText(filePath, json);
    }
}
