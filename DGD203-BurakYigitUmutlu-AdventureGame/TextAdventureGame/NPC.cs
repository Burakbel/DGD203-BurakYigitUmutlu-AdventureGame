public class NPC
{
    public string Name { get; }
    public string Description { get; }
    private readonly Dictionary<string, string> _dialogues;

    public NPC(string name, string description)
    {
        Name = name;
        Description = description;
        _dialogues = new Dictionary<string, string>();
    }

    public void AddDialogue(string key, string dialogue)
    {
        _dialogues[key] = dialogue;
    }

    public string GetDialogue(string key)
    {
        return _dialogues.TryGetValue(key, out string dialogue) ? dialogue : "...";
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"\n{Name} - {Description}");
    }

    public void Talk()
    {
        DisplayInfo();
        string dialogue = GetDialogue("default");
        Console.WriteLine($"\n{Name} says: {dialogue}");
    }
}
