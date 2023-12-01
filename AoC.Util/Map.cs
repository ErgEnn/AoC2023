using System.Collections;

namespace AoC.Util;

public class Map<TTile> where TTile : Map<TTile>.Tile
{
    public static Map<TTile> From<TTile>(string[] lines, Func<(int x, int y, Map<TTile> map, char data), TTile> generator,int startIndex = 0, int step = 1) where TTile : Map<TTile>.Tile
    {
        var map = new Map<TTile>();
        for (int y = 0; y < lines.Length; y++)
        {
            map._tiles.Add(new List<TTile>());
            for (int x = 0; x < lines[y].Length; x++)
            {
                map._tiles[y].Add(generator((x,y,map, lines[y][x])));
            }
        }
        map.Height = lines.Length;
        map.Width = lines[0].Length;
        return map;
    }

    public int Height { get; private set; }
    public int Width { get; private set; }

    private IList<IList<TTile>> _tiles = new List<IList<TTile>>();

    public TTile? At(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
        {
            return null;
        }
        return _tiles[y][x];
    }

    public LinkedList<TTile>? Pathfind(TTile start, TTile end)
    {
        var queue = new PriorityQueue<TTile, int>();
        queue.Enqueue(start, 0);
        var cameFrom = new Dictionary<TTile, TTile?>();
        var cost = new Dictionary<TTile, int>();
        cameFrom.Add(start, null);
        cost.Add(start, 0);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (current == end)
                break;

            foreach (var nextTile in current.GetNextTiles())
            {
                var newCost = cost[current] + 1;
                if (!cost.ContainsKey(nextTile) || newCost < cost[nextTile])
                {
                    cost.CreateOrUpdate(nextTile, newCost);
                    queue.Enqueue(nextTile, newCost + nextTile.DistanceTo(end));
                    cameFrom.CreateOrUpdate(nextTile, current);
                }
            }
        }

        if (!cameFrom.ContainsKey(end))
            return null;

        return cameFrom.ToLinkedList(end);
    }

    public abstract class Tile
    {
        public Map<TTile> Map { get; init; }
        public int X { get; init; }
        public int Y { get; init; }
        public char Data { get; init; }

        public TTile? Up()
        {
            return Map.At(X, Y-1);
        }

        public TTile? Down()
        {
            return Map.At(X, Y+1);
        }

        public TTile? Left()
        {
            return Map.At(X-1, Y);
        }

        public TTile? Right()
        {
            return Map.At(X+1, Y);
        }

        public int DistanceTo(TTile tile)
        {
            return Math.Abs(tile.X - X) + Math.Abs(tile.Y - Y);
        }

        public IEnumerable<(char direction,TTile? tile)> Iter4Surrounding()
        {
            yield return (direction: 'U', tile:Up());
            yield return (direction: 'R', tile: Right());
            yield return (direction: 'D', tile: Down());
            yield return (direction: 'L', tile: Left());
        }

        public virtual IEnumerable<TTile> GetNextTiles()
        {
            foreach ((char _, TTile tile)  in Iter4Surrounding())
            {
                yield return tile;
            }
        }

        public IEnumerable<(char direction,TTile? tile)> Iter8Surrounding()
        {
            yield return (direction: 'U', tile:Up());
            yield return (direction: 'R', tile: Right());
            yield return (direction: 'D', tile: Down());
            yield return (direction: 'L', tile: Left());
        }
    }

    public IEnumerable<TTile> GetTiles()
    {
        foreach (var tileRow in _tiles)
        {
            foreach (var tile in tileRow)
            {
                yield return tile;
            }
        }
    }

    public void PrintMap(Action<TTile> converter)
    {
        foreach (var tileRow in _tiles)
        {
            foreach (var tile in tileRow)
            {
                converter(tile);
            }
            Console.WriteLine();
        }
    }
}