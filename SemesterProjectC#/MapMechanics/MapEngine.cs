﻿using HOMM_Battles.Units;
using HOMM_Battles.TurnQueue;
using Gtk;
using Gdk;
using System.Drawing;

namespace HOMM_Battles.MapMechanics
{
    public class MapEngine
    {
        private BattleGrid battleGrid;
        public GameCycle gameCycle;
        int[,] points = new int[,] {{0, 2}, {0, 4}, {0, 6}, {0, 7}, {0, 8}, {0, 10}, {0, 11},
                                    {17, 2}, {17, 4}, {17, 6}, {17, 7}, {17, 8}, {17, 10}, {17, 11}};
        private bool isMovementInProgress = false;
        public static int width;
        public static int height;

        public bool OnDrawn(Cairo.Context cr, int width, int height)
        {
            MapEngine.width = width;
            MapEngine.height = width;
            var sprite = new Pixbuf("Assets/Battlefield.png").ScaleSimple(width, height, InterpType.Bilinear);
            Gdk.CairoHelper.SetSourcePixbuf(cr, sprite, 0, 0);
            cr.Paint();

            gameCycle.DrawQueue(cr);
            battleGrid.DrawHexGrid(cr, gameCycle.NextCheck());
            return true;
        }

        public async Task OnButtonPress(ButtonPressEventArgs ev)
        {
            if (isMovementInProgress) return;

            int mouseX = (int)ev.Event.X;
            int mouseY = (int)ev.Event.Y;

            Hexacell? clickedHex = battleGrid.GetHexFromCoords(new System.Drawing.Point(mouseX, mouseY));

            if (clickedHex == null) return;

            var unit = gameCycle.NextCheck();
            var posUnit = clickedHex.GetUnit();

            if ((posUnit != null && posUnit.team == unit.team) || posUnit == unit || !battleGrid.CanHandle(clickedHex)) return;

            unit = gameCycle.Next();

            if (posUnit != null && unit.CanAttack(posUnit))
            {
                var path = battleGrid.GetMinimalPathBeforeTarget(unit.currentHex, clickedHex);

                isMovementInProgress = true;
                await unit.GoToPositionAsync(path);
                isMovementInProgress = false;

                clickedHex.isSelected = true;
                unit.Attack(posUnit);
            }

            else if (posUnit == null)
            {
                var path = battleGrid.GetMinimalPath(unit.currentHex, clickedHex);

                isMovementInProgress = true;
                await unit.GoToPositionAsync(path);
                isMovementInProgress = false;

                clickedHex.isSelected = true;
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
        
        public MapEngine(int width, int height, SettingsWindow settings)
        {
            battleGrid = new BattleGrid(width, height);
            gameCycle = new GameCycle();

            for (int i = 0; i < 14; ++i)
            {
                bool res = int.TryParse(settings.amountEntries[i].Text, out int amount);
                if (res) Add(CreateUnit((UnitType)Enum.Parse(typeof(UnitType),
                            settings.unitSelectors[i].ActiveText), amount, battleGrid.map[points[i, 0], points[i, 1]], i < 7));
            }

            gameCycle.Shuffle();
        }
    }
}

