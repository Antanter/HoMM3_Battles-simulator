using Gtk;
using HOMM_Battles.TurnQueue;
using System.Drawing;

namespace HOMM_Battles.MapMechanics
{
    public class BattleGrid
    {
        private int width;
        private int height;
        public Hexacell[,] map;
        private const double deltaPadding = 1.5;

        public BattleGrid(int width_, int height_)
        {
            width = width_;
            height = height_;
            map = new Hexacell[width, height];

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    map[x, y] = new Hexacell(x, y);
                }
            }
        }

        public static double Padding() => deltaPadding;

        public Hexacell GetCell(Point point) => map[point.X, point.Y];

        public List<Units.Unit> ListOfUnits()
        {
            List<Units.Unit> units = new List<Units.Unit>();

            foreach (Hexacell cell in map)
            {
                var unit = cell.GetUnit();
                if (unit != null) units.Add(unit);
            }

            return units;
        }

        public Hexacell? GetHexFromCoords(Point point)
        {
            Hexacell? cellOutput = null;

            foreach (var cell in map)
            {
                if (cell.IsPointInHexagon(point.X, point.Y))
                {
                    cellOutput = cell;
                }
            }

            return cellOutput;
        }

        public void DrawHexGrid(Cairo.Context cr, Units.Unit unit) {
            for (int x = 0; x < width; ++x) {
                for (int y = 0; y < height; ++y) {
                    var hex = GetCell(new Point(x, y));
                    hex.DrawHexagon(cr, x, y, unit);
                }
            }
        }

        public void AppendUnit(Units.Unit unit) {
            var point = unit.currentHex.GetPosition();
            map[point.X, point.Y].SetUnit(unit);
        }

        private List<Hexacell> GetNeighbors(Hexacell hex)
        {
            List<Hexacell> neighbors = new List<Hexacell>();

            int x = hex.GetPosition().X;
            int y = hex.GetPosition().Y;

            int[,] directions = new int[,] { {1, 0}, {-1, 0}, {0, -1}, {0, 1}, {1, 1}, {1, -1} };

            for (int i = 0; i < 6; i++)
            {
                int nx = x + (x % 2 == 0 ? -directions[i, 0] : directions[i, 0]);
                int ny = y + directions[i, 1];

                if (nx >= 0 && ny >= 0 && nx < width && ny < height) neighbors.Add(map[nx, ny]);
            }

            return neighbors;
        }

        private Queue<Hexacell> BuildPath(Hexacell startHex, Hexacell targetHex, bool includeTarget)
        {
            var rootTable = new Dictionary<Hexacell, Hexacell>();
            var distanceTable = new Dictionary<Hexacell, int>();
            var queue = new Queue<Hexacell>();

            queue.Enqueue(startHex);
            rootTable[startHex] = null;
            distanceTable[startHex] = 0;

            if (startHex.GetUnit() != targetHex.GetUnit())
            {
                while (queue.Count > 0)
                {
                    Hexacell current = queue.Dequeue();
                    int currentDistance = distanceTable[current];

                    foreach (var neighbor in GetNeighbors(current))
                    {
                        if (neighbor == null) continue;

                        var unitInNeighbor = neighbor.GetUnit();
                        if (unitInNeighbor != null && unitInNeighbor != targetHex.GetUnit())
                            continue;

                        if (!distanceTable.ContainsKey(neighbor) || currentDistance + 1 < distanceTable[neighbor])
                        {
                            queue.Enqueue(neighbor);
                            distanceTable[neighbor] = currentDistance + 1;
                            rootTable[neighbor] = current;
                        }
                    }
                }
            }

            Hexacell step = includeTarget ? targetHex : rootTable[targetHex];
            var path = new Stack<Hexacell>();

            while (step != null)
            {
                path.Push(step);
                step = rootTable[step];
            }

            return new Queue<Hexacell>(path);
        }

        public bool CanHandle(Hexacell hex)
        {
            bool output = false;
            foreach (var neighbour in GetNeighbors(hex)) { if (neighbour.GetUnit() == null) output = true; }
            return output;
        }

        public Queue<Hexacell> GetMinimalPath(Hexacell startHex, Hexacell targetHex)
        {
            return BuildPath(startHex, targetHex, true);
        }

        public Queue<Hexacell> GetMinimalPathBeforeTarget(Hexacell startHex, Hexacell targetHex)
        {
            return BuildPath(startHex, targetHex, false);
        }
    }
}