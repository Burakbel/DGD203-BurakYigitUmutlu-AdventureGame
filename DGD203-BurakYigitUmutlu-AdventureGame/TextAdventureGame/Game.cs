public class Game
{
    #region REFERENCES
    private Player _player;
    private GameMap _map;
    private SaveManager _saveManager;
    #endregion

    #region CONSTANTS
    private const int MapWidth = 2;
    private const int MapHeight = 2;
    public static readonly Vector2Int DefaultStartingCoordinates = new Vector2Int(0, 0);
    private const string CommandSeparator = "--------------------------";
    #endregion

    #region GAME STATE
    private bool _isRunning;
    private bool _hasWon;
    #endregion

    public Game()
    {
        GenerateMap();
        GenerateStartingInstances();
        InitializeStartingInstances();
    }

    private void GenerateMap()
    {
        _map = new GameMap(this, MapWidth, MapHeight, DefaultStartingCoordinates);
    }

    private void GenerateStartingInstances()
    {
        _player = new Player();
        _saveManager = new SaveManager(this);
    }

    private void InitializeStartingInstances()
    {
        _saveManager.Initialize(_player, _map);
    }

    public void StartGame()
    {
        _isRunning = true;
        while (_isRunning)
        {
            ShowMainMenu();
        }
    }

    private void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("=== The Dark Castle Adventure ===");
        Console.WriteLine("\n1. New Game");
        Console.WriteLine("2. Load Game");
        Console.WriteLine("3. Credits");
        Console.WriteLine("4. Exit");
        Console.Write("\nEnter your choice: ");

        string choice = Console.ReadLine()?.Trim();
        switch (choice)
        {
            case "1":
                StartNewGame();
                break;
            case "2":
                if (_saveManager.LoadGame())
                {
                    StartGameLoop();
                }
                else
                {
                    Console.WriteLine("\nPress any key to return to main menu...");
                    Console.ReadKey();
                }
                break;
            case "3":
                ShowCredits();
                break;
            case "4":
                _isRunning = false;
                break;
            default:
                Console.WriteLine("\nInvalid choice. Press any key to continue...");
                Console.ReadKey();
                break;
        }
    }

    private void ShowCredits()
    {
        Console.Clear();
        Console.WriteLine("=== Credits ===\n");
        Console.WriteLine("The Dark Castle Adventure");
        Console.WriteLine("A Text-Based Adventure Game\n");
        Console.WriteLine("DGD-203-fınal\n");
        Console.WriteLine("Special Thanks to:");
        Console.WriteLine("- All the brave adventurers who dared to challenge the Dark Lord");
        Console.WriteLine("- The villagers who shared their wisdom");
        Console.WriteLine("- And you, for playing our game!\n");
        Console.WriteLine("\nPress any key to return to main menu...");
        Console.ReadKey();
    }

    private void StartNewGame()
    {
        Console.Clear();
        Console.WriteLine("=== The Dark Castle Adventure ===");
        Console.WriteLine("\nIn a peaceful village, an ancient evil has awakened in the Dark Castle.");
        Console.WriteLine("The villagers seek a hero to confront this darkness and restore peace to the land.");
        Console.WriteLine("\nPlease enter your name, brave adventurer:");
        
        string playerName = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Hero";
        }
        _player.SetUp(playerName);
        
        Console.WriteLine($"\nWelcome, {_player.Name}! Your journey begins in the Village Square.");
        Console.WriteLine("Type 'help' for available commands.");
        Console.WriteLine("\nPress any key to start your adventure...");
        Console.ReadKey();
        
        StartGameLoop();
    }

    private void StartGameLoop()
    {
        _hasWon = false;
        GameLoop();
    }

    private void GameLoop()
    {
        while (_isRunning && !_hasWon)
        {
            Console.Clear(); 
            Console.WriteLine(CommandSeparator);
            _map.DisplayCurrentLocation(_player.Position);
            ProcessCommand(Console.ReadLine()?.ToLower().Trim());
            CheckWinCondition();
            
            if (_isRunning && !_hasWon)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }

    private void CheckWinCondition()
    {
        var currentLocation = _map.GetLocation(_player.Position);
        if (currentLocation?.IsEndLocation == true)
        {
            Console.Clear();
            Console.WriteLine("\n=== The Final Confrontation ===");
            Console.WriteLine("\nYou stand before the Dark Lord in his throne room.");
            Console.WriteLine("Dark energy crackles around his obsidian armor, and you can see");
            Console.WriteLine("the crystal at its center pulsing with an ominous light.");
            
            Console.WriteLine("\nThe Dark Lord rises from his throne: 'So, you've gathered the courage to face me.");
            Console.WriteLine("Show me what you've learned on your journey!'");

            if (StartCombat())
            {
                ShowVictoryEnding();
            }
            else
            {
                ShowDefeatEnding();
            }
            
            Console.WriteLine("\nPress any key to return to main menu...");
            Console.ReadKey();
            _hasWon = true;
        }
    }

    private bool StartCombat()
    {
        int totalPoints = 0;
        int round = 1;

        // Define point values for each choice in each round
        Dictionary<int, Dictionary<string, int>> roundPoints = new Dictionary<int, Dictionary<string, int>>
        {
            { 1, new Dictionary<string, int> { {"1", 30}, {"2", 20}, {"3", 10} } },
            { 2, new Dictionary<string, int> { {"1", 20}, {"2", 30}, {"3", 10} } },
            { 3, new Dictionary<string, int> { {"1", 10}, {"2", 20}, {"3", 30} } }
        };

        while (round <= 3)
        {
            Console.WriteLine($"\n=== Round {round} ===");
            Console.WriteLine("\nChoose your attack strategy:");
            
            switch (round)
            {
                case 1:
                    Console.WriteLine("1. Channel pure light into a focused beam");
                    Console.WriteLine("2. Attempt to break through his dark barrier");
                    Console.WriteLine("3. Search for a weakness in his defenses");
                    break;

                case 2:
                    Console.WriteLine("1. Strike at his armor's weak points");
                    Console.WriteLine("2. Unleash a powerful light explosion");
                    Console.WriteLine("3. Try to outmaneuver him");
                    break;

                case 3:
                    Console.WriteLine("1. Make a defensive stance");
                    Console.WriteLine("2. Launch a series of quick attacks");
                    Console.WriteLine("3. Gather all your remaining power for one final strike");
                    break;
            }

            string choice = Console.ReadLine()?.Trim() ?? "";
            if (roundPoints[round].ContainsKey(choice))
            {
                int points = roundPoints[round][choice];
                totalPoints += points;

                // Show combat narrative based on choice
                switch (points)
                {
                    case 30:
                        Console.WriteLine("\nYour attack lands with devastating effect!");
                        Console.WriteLine("The Dark Lord staggers backward, clearly wounded.");
                        break;
                    case 20:
                        Console.WriteLine("\nA solid strike! The Dark Lord grunts in pain.");
                        Console.WriteLine("You seem to have found a weakness.");
                        break;
                    case 10:
                        Console.WriteLine("\nYour attack barely affects the Dark Lord.");
                        Console.WriteLine("He laughs at your attempt.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("\nInvalid choice! The Dark Lord takes advantage of your hesitation.");
                Console.WriteLine("This mistake will cost you dearly...");
            }

            round++;
        }

        Console.WriteLine("\nThe battle concludes...");
        return totalPoints >= 60; // Need 60 points to win
    }

    private void ShowVictoryEnding()
    {
        Console.Clear();
        Console.WriteLine("\n=== Victory ===");
        Console.WriteLine("\nYour well-chosen attacks proved too much for the Dark Lord.");
        Console.WriteLine("The crystal in his armor shatters completely, and the darkness that");
        Console.WriteLine("once consumed him dissipates into the air.");
        Console.WriteLine("\nAs the shadows fade, you see his true form - a fallen guardian");
        Console.WriteLine("finally freed from the darkness that corrupted him.");
        Console.WriteLine($"\nCongratulations, {_player.Name}! You have saved the land and become a legend!");
    }

    private void ShowDefeatEnding()
    {
        Console.Clear();
        Console.WriteLine("\n=== Defeat ===");
        Console.WriteLine("\nDespite your best efforts, your attacks weren't enough to defeat the Dark Lord.");
        Console.WriteLine("His dark power proves too strong, and your light begins to fade...");
        Console.WriteLine("\nThe darkness continues to spread across the land, awaiting another hero");
        Console.WriteLine("who might prove strong enough to challenge the Dark Lord.");
    }

    private void ProcessCommand(string command)
    {
        if (string.IsNullOrEmpty(command)) return;

        Console.Clear();
        switch (command)
        {
            case "help":
                DisplayHelp();
                break;
            case "north":
            case "n":
                Move(new Vector2Int(0, 1));
                break;
            case "south":
            case "s":
                Move(new Vector2Int(0, -1));
                break;
            case "east":
            case "e":
                Move(new Vector2Int(1, 0));
                break;
            case "west":
            case "w":
                Move(new Vector2Int(-1, 0));
                break;
            case "look":
            case "l":
                _map.DisplayCurrentLocation(_player.Position);
                break;
            case "clear":
            case "cls":
                Console.Clear();
                break;
            case "save":
                _saveManager.SaveGame();
                break;
            case "status":
                ShowPlayerStatus();
                break;
            case "quit":
            case "exit":
                _isRunning = false;
                Console.WriteLine("Thanks for playing! Goodbye!");
                break;
            default:
                if (command.StartsWith("talk "))
                {
                    TalkToNPC(command.Substring(5));
                }
                else
                {
                    Console.WriteLine("Unknown command. Type 'help' for available commands.");
                }
                break;
        }
    }

    private void Move(Vector2Int direction)
    {
        Vector2Int newPosition = _player.Position + direction;
        if (_map.IsValidPosition(newPosition))
        {
            var targetLocation = _map.GetLocation(newPosition);
            
            // Check if trying to enter Dark Castle
            if (targetLocation.Name == "Dark Castle")
            {
                if (!_player.CanEnterDarkCastle())
                {
                    Console.WriteLine("\n=== The Path is Not Yet Clear ===");
                    Console.WriteLine("An invisible force prevents you from approaching the castle.");
                    Console.WriteLine("You must first gather knowledge and strength by exploring both:");
                    Console.WriteLine("- The Forest Path (to learn the nature of the evil)");
                    Console.WriteLine("- The Mountain Pass (to understand the ancient seals)");
                    Console.WriteLine("\nReturn when you have explored both paths.");
                    return;
                }
            }

            _player.Move(newPosition);
            var newLocation = _map.GetLocation(newPosition);
            _player.VisitLocation(newLocation.Name);
        }
        else
        {
            Console.WriteLine("You cannot go that way!");
        }
    }

    private void TalkToNPC(string npcName)
    {
        var location = _map.GetLocation(_player.Position);
        var npc = location.GetNPC(npcName);
        if (npc != null)
        {
            npc.Talk();
        }
        else
        {
            Console.WriteLine($"There is no one named {npcName} here.");
        }
    }

    private void DisplayHelp()
    {
        Console.WriteLine("\nAvailable Commands:");
        Console.WriteLine("- north/n: Move north");
        Console.WriteLine("- south/s: Move south");
        Console.WriteLine("- east/e: Move east");
        Console.WriteLine("- west/w: Move west");
        Console.WriteLine("- look/l: Look around current location");
        Console.WriteLine("- talk [name]: Talk to a person");
        Console.WriteLine("- status: Check your exploration progress");
        Console.WriteLine("- save: Save game");
        Console.WriteLine("- clear/cls: Clear screen");
        Console.WriteLine("- quit/exit: Exit game");
        Console.WriteLine("- help: Show this help message");
    }

    private void ShowPlayerStatus()
    {
        Console.WriteLine("\n=== Adventure Progress ===");
        Console.WriteLine($"Current Location: {_map.GetLocation(_player.Position).Name}");
        Console.WriteLine("\nPath Progress:");
        Console.WriteLine($"- Forest Path: {(_player.HasVisitedLocation("Forest Path") ? "Explored" : "Not Yet Explored")}");
        Console.WriteLine($"- Mountain Pass: {(_player.HasVisitedLocation("Mountain Pass") ? "Explored" : "Not Yet Explored")}");
        Console.WriteLine($"- Dark Castle: {(_player.CanEnterDarkCastle() ? "Ready to Enter" : "Cannot Enter Yet")}");
    }
}
