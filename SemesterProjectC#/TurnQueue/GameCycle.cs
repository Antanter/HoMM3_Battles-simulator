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
        public Queue<Unit> queue;
        private int round;
        private int turn;

        public GameCycle()
        {
            round = 0;
            turn = 0;
            queue = new Queue<Unit>();
        }

        public void Start() => queue.OrderByDescending(unit => unit.speed);

        private void IsConsistent() {
            if (!(queue.Any(x => x.team && !x.IsDead()) && queue.Any(x => !x.team && !x.IsDead()))) {
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

            return next.IsDead() ? Next() : next;
        }

        public void DrawQueue(Cairo.Context cr, double startX, double startY)
        {
            const double boxWidth = 80;
            const double boxHeight = 100;
            const double padding = 10;

            double x = startX;
            double y = startY;

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

            cr.MoveTo(startX, startY + boxHeight + 3 * padding);
            cr.ShowText($"Round: {round}, Turn: {turn}.");
        }
    }
}
