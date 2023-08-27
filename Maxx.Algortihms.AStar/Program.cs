namespace Maxx.Algortihms.AStar;

internal static class Program
{
    private static void Main()
    {
        var searchParams1 = new AStarPathFinder.SearchParameters
        {
            Map = new[,]
            {
                { true, true, true, true, true, true, true, true},
                { true, false, true, true, true, true, true, true},
                { true, false, false, false, false, false, true, true},
                { true, false, true, false, true, false, true, true},
                { true, false, true, false, true, true, true, true},
                { true, false, true, true, true, true, true, true},
                { true, false, false, false, false, false, true, true},
                { true, true, true, true, true, true, true, true},
                { true, true, true, true, true, true, true, true},
                { true, true, true, true, true, true, true, true},
                { true, true, true, true, true, true, true, true},
                { true, true, true, true, true, true, true, true},
            },
            StartLocation = new AStarPathFinder.Coord(0, 4),
            EndLocation = new AStarPathFinder.Coord(3, 2)
        };

        var path1 = new AStarPathFinder().FindPath(searchParams1);

        Console.WriteLine($"Path = [{AStarPathFinder.Coord.ToLogString(path1)}]");

        AStarPathFinder.SearchParameters.Print(searchParams1, path1);


        var searchParams2 = new AStarPathFinder.SearchParameters
        {
            Map = new[,]
            {
                { true, true, true, true, true, true, true, true},
                { true, false, true, true, true, true, true, true},
                { true, false, false, false, false, false, true, true},
                { true, false, true, false, true, false, true, true},
                { true, false, true, false, true, true, true, true},
                { true, false, true, true, true, true, true, true},
                { true, false, false, false, false, false, true, true},
                { true, true, true, true, true, true, true, true},
                { true, true, true, true, true, true, true, true},
                { true, true, true, true, true, true, true, true},
                { true, true, true, true, true, true, true, true},
                { true, true, true, true, true, true, true, true},
            },
            StartLocation = new AStarPathFinder.Coord(0, 4),
            EndLocation = new AStarPathFinder.Coord(7, 2)
        };
        
        var path2 = new AStarPathFinder().FindPath(searchParams2);
            
        Console.WriteLine($"Path = [{AStarPathFinder.Coord.ToLogString(path2)}]");

        AStarPathFinder.SearchParameters.Print(searchParams2, path2);
    }
}
