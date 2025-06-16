using LibVLCSharp.Shared;

public class MusicPlayer
{
    private LibVLC _libVLC;
    private MediaPlayer _mediaPlayer;
    private Dictionary<string, string> _tracks;

    public MusicPlayer()
    {
        Core.Initialize();
        _libVLC = new LibVLC();
        _mediaPlayer = new MediaPlayer(_libVLC);

        _tracks = new Dictionary<string, string>
        {
            { "main_menu", "Assets/Music/main_menu.mp3" },
            //{ "victory", "Assets/Music/victory_theme.mp3" },
            //{ "menu",    "Assets/Music/menu_theme.mp3" }
        };
    }

    public void Play(string trackName)
    {
        if (!_tracks.ContainsKey(trackName)) return;

        var media = new Media(_libVLC, _tracks[trackName], FromType.FromPath);
        _mediaPlayer.Stop();
        _mediaPlayer.Media = media;
        media.Dispose();
        _mediaPlayer.Play();
    }

    public void Stop()
    {
        _mediaPlayer.Stop();
    }

    public void Dispose()
    {
        _mediaPlayer.Dispose();
        _libVLC.Dispose();
    }
}
