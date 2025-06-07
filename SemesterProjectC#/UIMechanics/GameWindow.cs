using System;
using System.Numerics;
using Gtk;

namespace HOMM_Battles.MapMechanics
{
    public class GameWindow : Window
    {
        public MapEngine mapEngine;
        private DrawingArea drawingArea;

        public GameWindow(MapEngine map) : base("Battle Map")
        {
            mapEngine = map;
            drawingArea = new DrawingArea();

            drawingArea.AddEvents((int)Gdk.EventMask.ButtonPressMask);

            SetDefaultSize(1200, 850);
            SetPosition(WindowPosition.Center);

            DeleteEvent += (o, args) => Application.Quit();
            drawingArea.Drawn += (o, args) => mapEngine.OnDrawn(args.Cr);
            drawingArea.ButtonPressEvent += (o, args) => mapEngine.OnButtonPress(args);

            drawingArea.QueueDraw();

            Add(drawingArea);
            ShowAll();

            GLib.Timeout.Add(20, () => {
                drawingArea.QueueDraw();
                return true;
            });
        }
    }
}
