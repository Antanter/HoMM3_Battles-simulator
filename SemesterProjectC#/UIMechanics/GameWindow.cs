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

            Fullscreen();

            DeleteEvent += (o, args) => Application.Quit();
            drawingArea.Drawn += (o, args) => {
                int width = drawingArea.AllocatedWidth;
                int height = drawingArea.AllocatedHeight;

                mapEngine.OnDrawn(args.Cr, width, height);
            };
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
