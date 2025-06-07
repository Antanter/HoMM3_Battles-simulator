using Gtk;
using HOMM_Battles.TurnQueue;
using HOMM_Battles.MapMechanics;
using System.Drawing;
using System.ComponentModel;

public class Hexacell : DrawingArea
{
    private Point position;
    private HOMM_Battles.Units.Unit? unit;

    private const double hexSize = 30;
    public bool isSelected = false;
    public bool isColoured = false;

    public Hexacell(int x, int y)
    {
        position.X = x;
        position.Y = y;
    }

    public Hexacell(Point point)
    {
        position.X = point.X;
        position.Y = point.Y;
    }

    public void SetUnit(HOMM_Battles.Units.Unit unit_) => unit = unit_;

    public HOMM_Battles.Units.Unit? GetUnit() => unit;

    public void DeleteUnit() => unit = null;

    public bool HoldUnit() => unit != null;
    
    public Point GetPosition() => position;

    public void IsColoured(HOMM_Battles.Units.Unit unit_)
    {
        (int, int) OffsetToAxial(int col, int row) => (col - (row % 2 == 0 ? row / 2 : (row + 1) / 2), row);

        (int q1, int r1) = OffsetToAxial(position.X, position.Y);
        (int q2, int r2) = OffsetToAxial(unit_.currentHex.position.X, unit_.currentHex.position.Y);

        int dq = q1 - q2;
        int dr = r1 - r2;
        int ds = -(dq + dr);

        isColoured = (Math.Abs(dq) + Math.Abs(dr) + Math.Abs(ds)) / 2 <= unit_.speed;
    }

    public bool IsPointInHexagon(double px, double py)
    {
        if (position.Y % 2 == 0) { px -= 0.5 * Math.Sqrt(3) * hexSize; }

        double dx = Math.Abs(px - ((position.X + BattleGrid.Padding()) * Math.Sqrt(3) * hexSize));
        double dy = Math.Abs(py - ((position.Y + BattleGrid.Padding()) * 1.5 * hexSize));

        if (dx > hexSize || dy > hexSize || !isColoured) return false;
        return Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2)) <= hexSize;
    }

    public double Dist(Point point) => Math.Pow(point.X - position.X, 2) + Math.Pow(point.Y - position.Y, 2);

    public void DrawHexagon(Cairo.Context cr, double x, double y, HOMM_Battles.Units.Unit unit_)
    {
        IsColoured(unit_);
        double xOffset = (x + BattleGrid.Padding()) * Math.Sqrt(3) * hexSize;
        double yOffset = (y + BattleGrid.Padding()) * 1.5 * hexSize;

        if (y % 2 == 0) { xOffset += 0.5 * Math.Sqrt(3) * hexSize; }

        double angleStep = Math.PI / 3;
        double startAngle = Math.PI / 6;

        Cairo.Color sourceColor = (isSelected, isColoured, unit != null && !unit.IsDead() && unit_ == unit) switch
        {
            (true, _, _) => new Cairo.Color(0.1, 0.1, 0.1, 0.5),
            (_, true, false) => new Cairo.Color(0.5, 0.5, 0.5, 0.9),
            (_, _, true) => new Cairo.Color(1, 1, 0, 0.5),
            _ => new Cairo.Color(0.5, 0.5, 0.5, 0.5)
        };

        cr.SetSourceColor(sourceColor);
        isSelected = false;

        cr.MoveTo(xOffset + hexSize * Math.Cos(startAngle), yOffset + hexSize * Math.Sin(startAngle));

        for (int i = 1; i <= 6; i++)
        {
            double angle = startAngle + i * angleStep;
            cr.LineTo(xOffset + hexSize * Math.Cos(angle), yOffset + hexSize * Math.Sin(angle));
        }

        cr.ClosePath();
        cr.FillPreserve();
        cr.SetSourceRGB(0, 0, 0);
        cr.Stroke();

        if (unit != null && !unit.IsDead())
        {
            double spriteX = xOffset - unit.sprite.Width / 2.0;
            double spriteY = yOffset - unit.sprite.Height / 2.0;

            Gdk.CairoHelper.SetSourcePixbuf(cr, unit.sprite, spriteX, spriteY);
            cr.Paint();

            cr.SelectFontFace("Sans", Cairo.FontSlant.Normal, Cairo.FontWeight.Normal);
            cr.SetFontSize(14);

            string amountText = unit.amount.ToString();
            var extents = cr.TextExtents(amountText);

            double textX = spriteX + unit.sprite.Width / 4;
            double textY = spriteY + unit.sprite.Height;
            double padding = 4;

            cr.SetSourceRGBA(0, 0, 0, 0.6);
            cr.Rectangle(
                textX + extents.XBearing - padding,
                textY + extents.YBearing - padding,
                extents.Width + padding * 2,
                extents.Height + padding * 2
            );

            cr.Fill();

            cr.SetSourceRGB(1, 1, 1);
            cr.MoveTo(textX, textY);
            cr.ShowText(amountText);
        }
    }
}