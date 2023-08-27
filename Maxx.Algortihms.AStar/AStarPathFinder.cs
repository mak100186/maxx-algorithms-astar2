namespace Maxx.Algortihms.AStar;
public class AStarPathFinder
{
    public class Coord
    {
        public int Row { get; }
        public int Col { get; }

        public Coord(int col, int row)
        {
            Row = row;
            Col = col;
        }

        public override bool Equals(object? obj)
        {
            if ((obj == null) || GetType() != obj.GetType())
            {
                return false;
            }

            var p = (Coord)obj;
            return (Col == p.Col) && (Row == p.Row);
        }

        public override int GetHashCode()
        {
            return (Col << 2) ^ Row;
        }

        public override string ToString()
        {
            return $"({Col},{Row})";
        }

        public static string ToLogString(IEnumerable<Coord> path)
        {
            return string.Join(',', path);
        }
    }
    public class SearchParameters
    {
        public bool[,] Map { get; init; } = null!;
        public Coord StartLocation { get; init; } = null!;
        public Coord EndLocation { get; init; } = null!;

        public static void Print(SearchParameters parameters, IEnumerable<Coord>? path = null)
        {
            var grid = parameters.Map;
            var startCoord = parameters.StartLocation;
            var endCoord = parameters.EndLocation;

            for (var x = 0; x < grid.GetLength(0); x++)
            {
                var line = "";
                for (var y = 0; y < grid.GetLength(1); y++)
                {
                    var node = grid[x, y];
                    var location = new Coord(x, y);

                    if (location.Equals(startCoord))
                    {
                        line += "A";
                    }
                    else if (location.Equals(endCoord))
                    {
                        line += "B";
                    }
                    else if (node)
                    {
                        if (path != null && path.Contains(location))
                        {
                            line += "x";
                        }
                        else
                        {
                            line += ".";
                        }
                    }
                    else
                    {
                        line += "█";
                    }
                }

                Console.WriteLine(line);
            }
        }
    }

    private readonly Dictionary<Coord, bool> _closedSet = new();
    private readonly Dictionary<Coord, bool> _openSet = new();

    //cost of start to this key node
    private readonly Dictionary<Coord, int> _gScore = new();
    //cost of start to goal, passing through key node
    private readonly Dictionary<Coord, int> _fScore = new();

    private readonly Dictionary<Coord, Coord> _nodeLinks = new();

    public IEnumerable<Coord> FindPath(SearchParameters parameters)
    {
        _openSet[parameters.StartLocation] = true;
        _gScore[parameters.StartLocation] = 0;
        _fScore[parameters.StartLocation] = Heuristic(parameters.StartLocation, parameters.EndLocation);

        while (_openSet.Count > 0)
        {
            var current = NextBest();
            if (current != null && current.Equals(parameters.EndLocation))
            {
                return Reconstruct(current);
            }

            _openSet.Remove(current);
            _closedSet[current] = true;

            foreach (var neighbor in Neighbors(parameters.Map, current))
            {
                if (_closedSet.ContainsKey(neighbor))
                {
                    continue;
                }

                var projectedG = GetGScore(current) + 1;

                if (!_openSet.ContainsKey(neighbor))
                {
                    _openSet[neighbor] = true;
                }
                else if (projectedG >= GetGScore(neighbor))
                {
                    continue;
                }

                //record it
                _nodeLinks[neighbor] = current;
                _gScore[neighbor] = projectedG;
                _fScore[neighbor] = projectedG + Heuristic(neighbor, parameters.EndLocation);

            }
        }


        return new List<Coord>();
    }

    private static int Heuristic(Coord start, Coord goal)
    {
        var dx = goal.Col - start.Col;
        var dy = goal.Row - start.Row;
        return Math.Abs(dx) + Math.Abs(dy);
    }

    private int GetGScore(Coord pt)
    {
        _gScore.TryGetValue(pt, out var score);
        return score;
    }


    private int GetFScore(Coord pt)
    {
        _fScore.TryGetValue(pt, out var score);
        return score;
    }

    private static IEnumerable<Coord> Neighbors(bool[,] graph, Coord center)
    {
        //top row
        var pt = new Coord(center.Col - 1, center.Row - 1);
        if (IsValidNeighbor(graph, pt))
        {
            yield return pt;
        }

        pt = new Coord(center.Col, center.Row - 1);
        if (IsValidNeighbor(graph, pt))
        {
            yield return pt;
        }

        pt = new Coord(center.Col + 1, center.Row - 1);
        if (IsValidNeighbor(graph, pt))
        {
            yield return pt;
        }

        //middle row
        pt = new Coord(center.Col - 1 , center.Row);
        if (IsValidNeighbor(graph, pt))
        {
            yield return pt;
        }

        pt = new Coord(center.Col + 1, center.Row);
        if (IsValidNeighbor(graph, pt))
        {
            yield return pt;
        }


        //bottom row
        pt = new Coord(center.Col - 1, center.Row + 1);
        if (IsValidNeighbor(graph, pt))
        {
            yield return pt;
        }

        pt = new Coord(center.Col, center.Row + 1);
        if (IsValidNeighbor(graph, pt))
        {
            yield return pt;
        }

        pt = new Coord(center.Col + 1, center.Row + 1);
        if (IsValidNeighbor(graph, pt))
        {
            yield return pt;
        }
    }

    private static bool IsValidNeighbor(bool[,] matrix, Coord pt)
    {
        var x = pt.Col;
        var y = pt.Row;
        if (x < 0 || x >= matrix.GetLength(0))
        {
            return false;
        }

        if (y < 0 || y >= matrix.GetLength(1))
        {
            return false;
        }

        return matrix[x,y];
    }

    private IEnumerable<Coord> Reconstruct(Coord current)
    {
        var path = new List<Coord>();
        while (_nodeLinks.ContainsKey(current))
        {
            path.Add(current);
            current = _nodeLinks[current];
        }

        path.Reverse();
        return path;
    }

    private Coord? NextBest()
    {
        var best = int.MaxValue;
        Coord? bestPt = null;
        foreach (var node in _openSet.Keys)
        {
            var score = GetFScore(node);
            if (score < best)
            {
                bestPt = node;
                best = score;
            }
        }

        return bestPt;
    }

}
