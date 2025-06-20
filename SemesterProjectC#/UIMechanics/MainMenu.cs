using System.Security.Cryptography.X509Certificates;
using Gtk;
using HOMM_Battles.MapMechanics;
using HOMM_Battles.PersonaficationAndUI;

public class GameMenu : Window
{
    VBox mainBox = new VBox();
    VBox mainMenu;
    VBox settingsMenu;
    VBox modeMenu;

    public GameMenu() : base("Heroes of Might and Magic III: Battle edition")
    {
        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Add(mainBox);

        BuildMainMenu();
        BuildSettingsMenu();
        BuildModeMenu();

        ShowMainMenu();

        ShowAll();
        MusicPlayer.PlayMusic("main_menu", true);
    }

    void ClearMainBox()
    {
        foreach (var child in mainBox.Children) mainBox.Remove(child);
    }

    void ShowMainMenu()
    {
        ClearMainBox();
        mainBox.PackStart(mainMenu, true, true, 0);
        ShowAll();
    }

    void ShowSettingsMenu()
    {
        ClearMainBox();
        mainBox.PackStart(settingsMenu, true, true, 0);
        ShowAll();
    }

    void ShowModeMenu()
    {
        ClearMainBox();
        mainBox.PackStart(modeMenu, true, true, 0);
        ShowAll();
    }

    void ShowGame() {
        Hide();
        BuildGame();
    }
    
    void BuildMainMenu()
    {
        mainMenu = new VBox(true, 10);

        Button playBtn = new Button("Play");
        Button settingsBtn = new Button("Settings");
        Button exitBtn = new Button("Exit");

        playBtn.Clicked += (s, e) => ShowModeMenu();
        settingsBtn.Clicked += (s, e) => ShowSettingsMenu();
        exitBtn.Clicked += (s, e) => Application.Quit();

        mainMenu.PackStart(playBtn, false, false, 0);
        mainMenu.PackStart(settingsBtn, false, false, 0);
        mainMenu.PackStart(exitBtn, false, false, 0);
        mainMenu.BorderWidth = 20;
    }

    void BuildSettingsMenu()
    {
        settingsMenu = new VBox(true, 10);
        HBox topLeft = new HBox();

        Button backBtn = new Button("←");
        backBtn.Clicked += (s, e) => ShowMainMenu();
        topLeft.PackStart(backBtn, false, false, 0);

        CheckButton check = new CheckButton("Play music") { Active = true };
        check.Toggled += (s, e) => PlayerAccount.musicEnabled = check.Active;

        settingsMenu.PackStart(topLeft, false, false, 0);
        settingsMenu.PackStart(check, false, false, 0);

        settingsMenu.BorderWidth = 20;
    }

    void BuildModeMenu()
    {
        modeMenu = new VBox(true, 10);
        HBox topLeft = new HBox();
        Button backBtn = new Button("←");
        backBtn.Clicked += (s, e) => ShowMainMenu();
        topLeft.PackStart(backBtn, false, false, 0);

        Button singleBtn = new Button("Singleplayer");

        singleBtn.Clicked += (s, e) => ShowGame();

        modeMenu.PackStart(topLeft, false, false, 0);
        modeMenu.PackStart(singleBtn, false, false, 0);
        modeMenu.BorderWidth = 20;
    }

    void BuildGame() {
        SettingsWindow settings = new SettingsWindow();
        settings.Response += (o, args) => {
            if (args.ResponseId == ResponseType.Ok) {
                MapEngine map = new MapEngine(18, 13, settings);
                settings.Destroy();

                GameWindow window = new GameWindow(map);
                window.mapEngine.gameCycle.RestartRequested += (sender, e) =>
                {
                    MusicPlayer.PlayMusic("endschpiele", true, 0.6f);

                    bool transitionDone = false;

                    GLib.Timeout.Add(2000, () =>
                    {
                        if (!transitionDone)
                        {
                            transitionDone = true;
                            window.Destroy();
                            ShowMainMenu();
                        }
                        return false;
                    });

                    window.ButtonPressEvent += (s, ev) =>
                    {
                        if (!transitionDone)
                        {
                            transitionDone = true;
                            window.Destroy();
                            ShowMainMenu();
                        }
                    };
                };
                window.ShowAll();
            }
            else {
                settings.Destroy();
                ShowMainMenu();
            }
        };
        settings.ShowAll();
    }
}
