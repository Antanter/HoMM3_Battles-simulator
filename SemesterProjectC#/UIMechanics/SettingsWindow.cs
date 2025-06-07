using Gtk;
using System;
using System.Collections.Generic;

public class SettingsWindow : Dialog
{
    public List<Entry> amountEntries { get; private set; }
    public List<ComboBoxText> unitSelectors { get; private set; }

    public SettingsWindow() : base("Unit settings", null, DialogFlags.Modal)
    {
        SetDefaultSize(600, 700);
        SetPosition(WindowPosition.Center);

        Box contentBox = ContentArea;

        amountEntries = new List<Entry>();
        unitSelectors = new List<ComboBoxText>();

        VBox mainBox = new VBox(false, 5);

        for (int i = 0; i < 14; i++)
        {
            HBox row = new HBox(false, 5);

            ComboBoxText unitSelector = new ComboBoxText();
            Entry amountEntry = new Entry { PlaceholderText = "Amount" };

            foreach (string unitName in Enum.GetNames(typeof(HOMM_Battles.Units.UnitType)))
            {
                unitSelector.AppendText(unitName);
            }
            unitSelector.Active = 0;

            unitSelectors.Add(unitSelector);
            amountEntries.Add(amountEntry);

            row.PackStart(amountEntry, false, false, 5);
            row.PackStart(unitSelector, false, false, 5);

            mainBox.PackStart(row, false, false, 5);
        }

        contentBox.PackStart(mainBox, false, false, 10);

        AddButton("Start", ResponseType.Ok);
        AddButton("Cancel", ResponseType.Cancel);

        ShowAll();
    }
}
