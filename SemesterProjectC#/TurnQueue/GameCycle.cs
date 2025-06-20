using System;
using HOMM_Battles.Units;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HOMM_Battles.MapMechanics;

namespace HOMM_Battles.TurnQueue
{
    public class GameCycle
    {
        public event EventHandler RestartRequested;
        private bool restartAlreadyRequested = false;
        public Queue<Unit> queue;
        private int turnReal;
        private int turnImag;

        public GameCycle()
        {
            turnReal = 0;
            turnImag = 0;
            queue = new Queue<Unit>();
        }

        public void Shuffle() => queue = new Queue<Unit>(queue.OrderByDescending(unit => unit.speed));

        private void IsConsistent()
        {
            if (restartAlreadyRequested) return;

            bool hasTeam1Alive = queue.Any(x => x.team && !x.IsDead());
            bool hasTeam2Alive = queue.Any(x => !x.team && !x.IsDead());

            if (!(hasTeam1Alive && hasTeam2Alive))
            {
                restartAlreadyRequested = true;
                RestartRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        public void AppendUnit(Unit unit) {
            queue.Enqueue(unit);
        }

        public Unit NextCheck() {
            IsConsistent();

            return queue.FirstOrDefault(x => !x.IsDead());
        }

        public Unit Next() {
            IsConsistent();
            
            Unit next = queue.Dequeue();
            queue.Enqueue(next);
            ++turnImag;

            if (next.IsDead()) return Next();
            else { ++turnReal; return next; }
        }

        public void DrawQueue(Cairo.Context cr)
        {
            const double boxWidth = 80;
            const double boxHeight = 100;
            const double padding = 10;

            double x = MapEngine.width / 5.0;
            double y = MapEngine.height / 2.0;

            foreach (Unit unit in queue)
            {
                if (unit.IsDead()) continue;

                cr.SetSourceRGBA(0.2, 0.2, 0.2, 0.8);
                cr.Rectangle(x, y, boxWidth, boxHeight);
                cr.Fill();

                double spriteX = x + 10;
                double spriteY = y + 10;

                Gdk.CairoHelper.SetSourcePixbuf(cr, unit.sprite, spriteX, spriteY);
                cr.Paint();

                cr.SetSourceRGB(1, 1, 1);
                cr.SelectFontFace("Sans", Cairo.FontSlant.Normal, Cairo.FontWeight.Bold);
                cr.SetFontSize(12);

                cr.MoveTo(x + 5, y + boxHeight - 10);
                cr.ShowText($"{unit.name} ({unit.amount})");

                x += boxWidth + padding;
            }

            cr.SetSourceRGB(0, 0, 0);
            cr.SelectFontFace("Sans", Cairo.FontSlant.Normal, Cairo.FontWeight.Bold);
            cr.SetFontSize(14);

            cr.MoveTo(MapEngine.width / 5.0, MapEngine.height / 2.0 + boxHeight + 1.5 * padding);
            cr.ShowText($"Round: {turnImag / queue.Count}, Turn: №{turnReal}.");
        }
    }
}
