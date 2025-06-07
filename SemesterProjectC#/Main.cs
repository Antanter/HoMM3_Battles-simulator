using System;
using Gtk;
using Cairo;
using HOMM_Battles.MapMechanics;

class Program
{
    static void Main(string[] args)
    {
        Application.Init();
        new GameMenu();
        Application.Run();
    }
}