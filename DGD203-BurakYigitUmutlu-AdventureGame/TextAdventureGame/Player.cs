public class Player
{
    private string _name;
    private Vector2Int _position;
    private HashSet<string> _visitedLocations;

    public string Name => _name;
    public Vector2Int Position => _position;

    public Player()
    {
        _visitedLocations = new HashSet<string>();
        _position = new Vector2Int(0, 0); // Start at Village Square
    }

    public void SetUp(string name)
    {
        _name = name;
    }

    public void Move(Vector2Int newPosition)
    {
        _position = newPosition;
    }

    public void VisitLocation(string locationName)
    {
        _visitedLocations.Add(locationName);
    }

    public bool HasVisitedLocation(string locationName)
    {
        return _visitedLocations.Contains(locationName);
    }

    public IEnumerable<string> GetVisitedLocations()
    {
        return _visitedLocations;
    }

    public bool CanEnterDarkCastle()
    {
        return HasVisitedLocation("Forest Path") && HasVisitedLocation("Mountain Pass");
    }
}
