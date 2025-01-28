public class MapLocation
{
    public string Name { get; }
    public string Description { get; }
    public bool IsDiscovered { get; private set; }
    public bool IsEndLocation { get; }
    public List<NPC> NPCs { get; }
    public Vector2Int Coordinates { get; }

    public MapLocation(string name, string description, Vector2Int coordinates, bool isEndLocation = false)
    {
        Name = name;
        Description = description;
        Coordinates = coordinates;
        IsEndLocation = isEndLocation;
        IsDiscovered = false;
        NPCs = new List<NPC>();
    }

    public void Discover()
    {
        IsDiscovered = true;
    }

    public void AddNPC(NPC npc)
    {
        NPCs.Add(npc);
    }

    private string GetLocationMap()
    {
        return $@"
    [N]
    ^
[W] + [E]  Current Position: ({Coordinates.X}, {Coordinates.Y})
    v
    [S]
";
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"\nLocation: {Name}");
        Console.WriteLine(GetLocationMap());
        Console.WriteLine(Description);
        
        if (NPCs.Count > 0)
        {
            Console.WriteLine("\nPeople in this location:");
            foreach (var npc in NPCs)
            {
                Console.WriteLine($"- {npc.Name}");
            }
        }
    }

    public NPC GetNPC(string name)
    {
        return NPCs.FirstOrDefault(n => n.Name.ToLower() == name.ToLower());
    }
}
