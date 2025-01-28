using System.Text.Json;

public class SaveManager
{
    private readonly Game _game;
    private Player _player;
    private GameMap _map;
    private const string SaveDirectory = "saves";
    private const string SaveExtension = ".sgf";

    public SaveManager(Game game)
    {
        _game = game;
        // Create saves directory if it doesn't exist
        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }
    }

    public void Initialize(Player player, GameMap map)
    {
        _player = player;
        _map = map;
    }

    public void SaveGame()
    {
        Console.Clear();
        Console.WriteLine("=== Save Game ===\n");
        Console.Write("Enter save name: ");
        string saveName = Console.ReadLine()?.Trim() ?? DateTime.Now.ToString("yyyyMMdd_HHmmss");
        
        // Sanitize filename
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            saveName = saveName.Replace(c, '_');
        }

        var saveData = new SaveData
        {
            SaveName = saveName,
            SaveDate = DateTime.Now,
            PlayerName = _player.Name,
            PlayerPosition = _player.Position,
            CurrentLocation = _map.GetLocation(_player.Position)?.Name ?? "Unknown",
            VisitedLocations = _player.GetVisitedLocations().ToList()
        };

        string savePath = Path.Combine(SaveDirectory, saveName + SaveExtension);
        string jsonString = JsonSerializer.Serialize(saveData, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(savePath, jsonString);
        Console.WriteLine($"\nGame saved successfully as '{saveName}{SaveExtension}'!");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public bool LoadGame()
    {
        var saves = ListSaves();
        if (saves.Count == 0)
        {
            Console.WriteLine("No save files found.");
            return false;
        }

        Console.WriteLine("Available Saves:\n");
        for (int i = 0; i < saves.Count; i++)
        {
            var save = LoadSaveData(saves[i]);
            Console.WriteLine($"{i + 1}. {save.SaveName}");
            Console.WriteLine($"   Date: {save.SaveDate}");
            Console.WriteLine($"   Player: {save.PlayerName}");
            Console.WriteLine($"   Location: {save.CurrentLocation} ({save.PlayerPosition.X}, {save.PlayerPosition.Y})");
            Console.WriteLine($"   Explored: {save.VisitedLocations.Count} locations\n");
        }

        Console.Write("Enter save number to load (or 0 to cancel): ");
        if (int.TryParse(Console.ReadLine()?.Trim(), out int choice) && choice > 0 && choice <= saves.Count)
        {
            var saveData = LoadSaveData(saves[choice - 1]);
            _player.SetUp(saveData.PlayerName);
            _player.Move(saveData.PlayerPosition);
            
            foreach (string location in saveData.VisitedLocations)
            {
                _player.VisitLocation(location);
            }

            Console.WriteLine($"\nLoaded save '{saveData.SaveName}' successfully!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return true;
        }

        return false;
    }

    private List<string> ListSaves()
    {
        return Directory.GetFiles(SaveDirectory, $"*{SaveExtension}")
                       .OrderByDescending(f => File.GetLastWriteTime(f))
                       .ToList();
    }

    private SaveData LoadSaveData(string savePath)
    {
        string jsonString = File.ReadAllText(savePath);
        return JsonSerializer.Deserialize<SaveData>(jsonString) ?? 
               throw new Exception($"Failed to load save file: {savePath}");
    }
}

public class SaveData
{
    public string SaveName { get; set; } = "";
    public DateTime SaveDate { get; set; }
    public string PlayerName { get; set; } = "";
    public Vector2Int PlayerPosition { get; set; }
    public string CurrentLocation { get; set; } = "";
    public List<string> VisitedLocations { get; set; } = new();
}
