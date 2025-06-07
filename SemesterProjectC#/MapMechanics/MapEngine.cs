using HOMM_Battles.Units;
using HOMM_Battles.TurnQueue;
using Gtk;
using System.Drawing;
using System.Threading.Tasks;

namespace HOMM_Battles.MapMechanics
{
    public class MapEngine
    {
        private BattleGrid battleGrid;
        public GameCycle gameCycle;
        int[,] points = new int[,] {{0, 2}, {0, 4}, {0, 6}, {0, 7}, {0, 8}, {0, 10}, {0, 11},
                                    {17, 2}, {17, 4}, {17, 6}, {17, 7}, {17, 8}, {17, 10}, {17, 11}};

        public bool OnDrawn(Cairo.Context cr) {
            gameCycle.DrawQueue(cr, 100, 700);
            battleGrid.DrawHexGrid(cr, gameCycle.NextCheck());
            return true;
        }

        public async Task OnButtonPress(ButtonPressEventArgs ev) {
            int mouseX = (int)ev.Event.X;
            int mouseY = (int)ev.Event.Y;

            Hexacell? clickedHex = battleGrid.GetHexFromCoords(new Point(mouseX, mouseY));
            var unit = gameCycle.Next();

            if (clickedHex != null && clickedHex.GetUnit() != unit) {

                if (clickedHex.HoldUnit() && unit.CanAttack(clickedHex.GetUnit())) {
                    var path = battleGrid.GetMinimalPathBeforeTarget(unit.currentHex, clickedHex);
                    
                    await unit.GoToPositionAsync(path);
                    clickedHex.isSelected = true;
                    unit.Attack(clickedHex.GetUnit());
                }

                else if (clickedHex.GetUnit() == null) {
                    var path = battleGrid.GetMinimalPath(unit.currentHex, clickedHex);
                    await unit.GoToPositionAsync(path);
                    clickedHex.isSelected = true;
                }
            }
        }

        public Units.Unit CreateUnit(UnitType type, int amount, Hexacell hex, bool team)
        {
            switch (type)
            {
                case UnitType.Peasant: return new Peasant(amount, hex, team);
                case UnitType.Archer: return new Archer(amount, hex, team);
                case UnitType.Griffin: return new Griffin(amount, hex, team);
                case UnitType.Swordsman: return new Swordsman(amount, hex, team);
                case UnitType.Monk: return new Monk(amount, hex, team);
                case UnitType.Cavalier: return new Cavalier(amount, hex, team);
                case UnitType.Angel: return new Angel(amount, hex, team);

                case UnitType.Skeleton: return new Skeleton(amount, hex, team);
                case UnitType.Zombie: return new Zombie(amount, hex, team);
                case UnitType.Ghost: return new Ghost(amount, hex, team);
                case UnitType.Vampire: return new Vampire(amount, hex, team);
                case UnitType.Lich: return new Lich(amount, hex, team);
                case UnitType.DeathKnight: return new DeathKnight(amount, hex, team);
                case UnitType.BoneDragon: return new BoneDragon(amount, hex, team);

                default: return null;
            }
        }

        private void Add(Units.Unit unit) {
            battleGrid.AppendUnit(unit);
            gameCycle.AppendUnit(unit);
        }

        private void StartGame() {
            gameCycle.Start();
        }

        public MapEngine(int width, int height, SettingsWindow settings) {
            battleGrid = new BattleGrid(width, height);
            gameCycle = new GameCycle();

            for (int i = 0; i < 14; ++i) {
                bool res = int.TryParse(settings.amountEntries[i].Text, out int amount);
                if (res) Add(CreateUnit((UnitType)Enum.Parse(typeof(UnitType), 
                            settings.unitSelectors[i].ActiveText), amount, battleGrid.map[points[i,0], points[i,1]], i < 7));
            }

            StartGame();
        }
    }
}

