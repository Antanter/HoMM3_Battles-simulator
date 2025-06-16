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

        VBox mainBox = new VBox(false, 10);
        HBox columns = new HBox(false, 20);
        VBox leftColumnContainer = new VBox(false, 5);
        VBox rightColumnContainer = new VBox(false, 5);

        Label team1Label = new Label("Team 1") { Xalign = 0 };
        Label team2Label = new Label("Team 2") { Xalign = 0 };

        VBox leftColumn = new VBox(false, 5);
        VBox rightColumn = new VBox(false, 5);

        for (int i = 0; i < 14; i++)
        {
            HBox row = new HBox(false, 5);

            ComboBoxText unitSelector = new ComboBoxText();
            Entry amountEntry = new Entry { PlaceholderText = "Amount" };
            amountEntry.Changed += (o, e) =>
            {
                string text = amountEntry.Text;
                string filtered = new string(text.Where(char.IsDigit).ToArray());

                if (text != filtered)
                {
                    int pos = amountEntry.Position;
                    amountEntry.Text = filtered;
                    amountEntry.Position = Math.Min(pos - 1, filtered.Length);
                }
            };

            foreach (string unitName in Enum.GetNames(typeof(HOMM_Battles.Units.UnitType)))
            {
                unitSelector.AppendText(unitName);
            }
            unitSelector.Active = 0;

            unitSelectors.Add(unitSelector);
            amountEntries.Add(amountEntry);

            row.PackStart(amountEntry, false, false, 5);
            row.PackStart(unitSelector, false, false, 5);

            if (i < 7)
                leftColumn.PackStart(row, false, false, 5);
            else
                rightColumn.PackStart(row, false, false, 5);
        }

        leftColumnContainer.PackStart(team1Label, false, false, 5);
        leftColumnContainer.PackStart(leftColumn, false, false, 5);

        rightColumnContainer.PackStart(team2Label, false, false, 5);
        rightColumnContainer.PackStart(rightColumn, false, false, 5);

        columns.PackStart(leftColumnContainer, false, false, 10);
        columns.PackStart(rightColumnContainer, false, false, 10);

        mainBox.PackStart(columns, false, false, 10);
        contentBox.PackStart(mainBox, false, false, 10);

        AddButton("Start", ResponseType.Ok);
        AddButton("Cancel", ResponseType.Cancel);

        Button randomButton = new Button("Random start");

        randomButton.Clicked += (sender, e) =>
        {
            Random rnd = new Random();
            int unitTypeCount = Enum.GetNames(typeof(HOMM_Battles.Units.UnitType)).Length;

            for (int i = 0; i < amountEntries.Count; i++)
            {
                int mid = unitTypeCount / 2;
                int j = i < mid ? rnd.Next(0, mid) : rnd.Next(mid, unitTypeCount);
                amountEntries[i].Text = rnd.Next((mid - (j % mid)) * 10, (mid - (j % mid)) * 20 + 1).ToString();
                unitSelectors[i].Active = j;
            }
        };

        HBox buttonBox = new HBox(false, 5);
        buttonBox.PackStart(randomButton, false, false, 0);
        mainBox.PackStart(buttonBox, false, false, 10);

        ShowAll();
    }
}
