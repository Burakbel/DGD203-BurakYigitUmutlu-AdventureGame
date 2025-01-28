public class GameMap
{
    private readonly Game _game;
    private readonly MapLocation[,] _locations;
    private readonly int _width;
    private readonly int _height;
    public Vector2Int StartingPosition { get; }
    public Vector2Int EndingPosition { get; }

    public GameMap(Game game, int width, int height, Vector2Int startingPosition)
    {
        _game = game;
        _width = width;
        _height = height;
        StartingPosition = startingPosition;
        EndingPosition = new Vector2Int(width - 1, height - 1);
        _locations = new MapLocation[width, height];
        InitializeMap();
    }

    private void InitializeMap()
    {
        // Starting Location - Village Square (0,0)
        _locations[0, 0] = new MapLocation("Village Square", 
            "A lively village square surrounded by thatched-roof cottages and cobblestone streets. " +
            "The morning sun casts warm light on the central fountain, where villagers gather to share news and trade goods. " +
            "The air is filled with the aroma of freshly baked bread from the nearby bakery, and children play between market stalls. " +
            "However, an undercurrent of worry can be seen on the villagers' faces as they discuss the darkness growing in the northeast.\n" +
            "\nAvailable paths:" +
            "\n- East: Forest Path" +
            "\n- North: Mountain Pass",
            new Vector2Int(0, 0));
        var villageElder = new NPC("Elder Marcus", "A wise elder with deep wrinkles and kind eyes, wearing traditional robes adorned with ancient symbols");
        villageElder.AddDialogue("default", "Listen carefully, brave soul. The Dark Lord was once a guardian of light who fell to darkness. " +
            "His transformation left him with a crucial weakness - he cannot bear the presence of pure light. " +
            "But to defeat him, you'll need more than this knowledge. Seek out Hunter Jane in the forest - she knows of an ancient weapon. " +
            "And Old Pete in the mountains... he guards a secret about the Dark Lord's armor. Only with all three pieces of knowledge " +
            "can you hope to triumph.");
        _locations[0, 0].AddNPC(villageElder);
        _locations[0, 0].Discover();

        // Forest Path (1,0)
        _locations[1, 0] = new MapLocation("Forest Path", 
            "A mysterious path winding through an ancient forest of towering oaks and gnarled birches. " +
            "Shafts of sunlight pierce through the dense canopy, creating dancing patterns on the moss-covered ground. " +
            "The air is thick with the scent of pine and wild mushrooms. Strange sounds echo from deep within the woods, " +
            "and the branches seem to whisper ancient secrets. Colorful forest flowers line the path, but some appear to be wilting " +
            "under an unseen influence.\n" +
            "\nAvailable paths:" +
            "\n- West: Back to Village Square" +
            "\n- North: Dark Castle",
            new Vector2Int(1, 0));
        var hunter = new NPC("Hunter Jane", "A nimble woman in leather armor, with a keen eye and a quiver of arrows at her back");
        hunter.AddDialogue("default", "The Dark Lord's weakness isn't just light - it's where you direct it. " +
            "I've observed his patrols from these woods. His armor has a crystal at its center, " +
            "meant to channel dark energy. But that same crystal, if struck with pure light, would shatter his power. " +
            "Remember this when you face him - aim for the crystal at the center of his chest.");
        _locations[1, 0].AddNPC(hunter);

        // Mountain Pass (0,1)
        _locations[0, 1] = new MapLocation("Mountain Pass", 
            "A treacherous path carved into the face of snow-capped mountains that pierce the clouds. " +
            "The thin air carries the haunting sound of wind through ancient caves and crevices. " +
            "Below, the forest stretches out like a green carpet, while ahead, dark storm clouds gather around distant peaks. " +
            "Stone markers line the path, worn smooth by centuries of harsh weather, their inscriptions barely legible. " +
            "The castle's ominous silhouette can be glimpsed through breaks in the clouds.\n" +
            "\nAvailable paths:" +
            "\n- South: Back to Village Square" +
            "\n- East: Dark Castle",
            new Vector2Int(0, 1));
        var mountaineer = new NPC("Old Pete", "A hardy mountain dweller with weather-beaten features and a thick beard frosted with ice");
        mountaineer.AddDialogue("default", "Aye, the Dark Lord's armor isn't invincible. These ancient runes tell of a ritual - " +
            "you must first use light to blind him, disrupting his dark powers. " +
            "Only then will the crystal in his armor become vulnerable. " +
            "Strike in the wrong order, and his darkness will consume you. " +
            "Remember: first blind, then strike the crystal. That's the key to victory!");
        _locations[0, 1].AddNPC(mountaineer);

        // Dark Castle - End Location (1,1)
        _locations[1, 1] = new MapLocation("Dark Castle", 
            "An imposing fortress of black stone that seems to absorb the very light around it. " +
            "Massive towers claw at the storm-wracked sky, while gargoyles perch on every corner, their stone eyes following your movement. " +
            "The castle gates hang open in silent invitation, the courtyard beyond shrouded in an unnatural mist. " +
            "Lightning flashes illuminate twisted sculptures in the architecture, and a deep thrumming power can be felt in the very air. " +
            "This is where the ancient evil has made its lair, waiting for those brave - or foolish - enough to challenge it.\n" +
            "\nAvailable paths:" +
            "\n- South: Back to Forest Path" +
            "\n- West: Back to Mountain Pass",
            new Vector2Int(1, 1), true);
        var darkLord = new NPC("Dark Lord", "A towering figure in obsidian armor, shadows writhing around their form like living darkness");
        darkLord.AddDialogue("default", "Another hero comes to challenge me? Your light cannot pierce my darkness. " +
            "Many have tried to exploit my supposed weaknesses, but all have failed. " +
            "The crystal in my armor pulses with dark energy - approach if you dare!");
        _locations[1, 1].AddNPC(darkLord);
    }

    public bool IsValidPosition(Vector2Int position)
    {
        return position.X >= 0 && position.X < _width &&
               position.Y >= 0 && position.Y < _height &&
               _locations[position.X, position.Y] != null;
    }

    public MapLocation GetLocation(Vector2Int position)
    {
        if (!IsValidPosition(position))
            return null;

        return _locations[position.X, position.Y];
    }

    public void DisplayCurrentLocation(Vector2Int position)
    {
        var location = GetLocation(position);
        if (location != null)
        {
            location.Discover();
            location.DisplayInfo();
        }
        else
        {
            Console.WriteLine("\nYou cannot go that way!");
        }
    }
}
