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

        private Hexacell? GetNearestHex(Point point)
        {
            Hexacell? selectedCell = null;
            double minDistance = double.MaxValue;

            for (int x = 0; x < width; ++x) {
                for (int y = 0; y < height; ++y) {
                    var cell = map[x, y];

                    cell.isSelected = false;

                    if (cell.IsPointInHexagon(point.X, point.Y)) {
                        selectedCell = cell;
                        continue;
                    }

                    if (!cell.isSelected) {
                        double distance = cell.Dist(point);

                        if (distance < minDistance) {
                            minDistance = distance;
                            selectedCell = cell;
                        }
                    }
                }
            }

            return selectedCell;
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

        private List<Hexacell> GetNeighbors(Hexacell hex, Hexacell start, Hexacell target)
        {
            List<Hexacell> neighbors = new List<Hexacell>();

            int x = hex.GetPosition().X;
            int y = hex.GetPosition().Y;

            int[,] directions = new int[,] { {1, 0}, {-1, 0}, {1, 1}, {-1, 1}, {1, -1}, {-1, -1} };

            for (int i = 0; i < 6; i++)
            {
                int nx = x + directions[i, 0];
                int ny = y + directions[i, 1];

                if (nx >= 0 && ny >= 0 && nx < width && ny < height && (start.GetUnit() != target.GetUnit())) {
                    neighbors.Add(map[nx, ny]);
                }
            }

            return neighbors;
        }

        private Queue<Hexacell> BuildPath(Hexacell startHex, Hexacell targetHex, bool includeTarget)
        {
            Queue<Hexacell> path = new Queue<Hexacell>();
            Dictionary<Hexacell, Hexacell> cameFrom = new Dictionary<Hexacell, Hexacell>();
            Queue<Hexacell> frontier = new Queue<Hexacell>();

            frontier.Enqueue(startHex);
            cameFrom[startHex] = null;

            while (frontier.Count > 0)
            {
                Hexacell current = frontier.Dequeue();

                if (current == targetHex)
                    break;

                foreach (var neighbor in GetNeighbors(current, startHex, targetHex))
                {
                    if (neighbor == null || cameFrom.ContainsKey(neighbor))
                        continue;

                    frontier.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }

            if (!cameFrom.ContainsKey(targetHex))
                return path;

            Hexacell step = includeTarget ? targetHex : cameFrom[targetHex];
            Stack<Hexacell> reversedPath = new Stack<Hexacell>();

            while (step != null)
            {
                reversedPath.Push(step);
                cameFrom.TryGetValue(step, out step);
            }

            while (reversedPath.Count > 0)
                path.Enqueue(reversedPath.Pop());

            return path;
        }

        public Queue<Hexacell> GetMinimalPath(Hexacell startHex, Hexacell targetHex)
        {
            return BuildPath(startHex, targetHex, includeTarget: true);
        }

        public Queue<Hexacell> GetMinimalPathBeforeTarget(Hexacell startHex, Hexacell targetHex)
        {
            return BuildPath(startHex, targetHex, includeTarget: false);
        }
    }
}