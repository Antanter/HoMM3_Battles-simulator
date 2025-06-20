using LibVLCSharp.Shared;
using HOMM_Battles.PersonaficationAndUI;

static public class MusicPlayer
{
    static private LibVLC _libVLC;
    static private Dictionary<string, string> _tracks;

    static MusicPlayer()
    {
        Core.Initialize();
        _libVLC = new LibVLC();

        _tracks = new Dictionary<string, string>
        {
            { "main_menu", "Assets/Music/main_menu.mp3" },
            { "steps", "Assets/Music/steps.mp3" },
            { "attack_1", "Assets/Music/attack_1.mp3" },
            { "attack_2", "Assets/Music/attack_2.mp3" },
            { "attack_3", "Assets/Music/attack_3.mp3" },
            { "attack_4", "Assets/Music/attack_4.mp3" },
            { "death", "Assets/Music/death.mp3" },
            { "endschpiele", "Assets/Music/endschpiele.mp3" }
        };
    }

    static public void PlayMusic(string trackName, bool loop = false, float speed = 1.0f)
    {
        if (!_tracks.ContainsKey(trackName))
            return;

        var mediaPlayer = new MediaPlayer(_libVLC);

        var media = new Media(_libVLC, _tracks[trackName], FromType.FromPath);
        mediaPlayer.Media = media;
        media.Dispose();

        mediaPlayer.SetRate(speed);

        PlayerAccount.MyFlagChanged += (val) =>
        {
            if (val) mediaPlayer.Play();
            else mediaPlayer.Pause();
        };

        if (loop) mediaPlayer.EndReached += (_, _) => PlayMusic(trackName, true);

        mediaPlayer.Play();
    }

    static public void Dispose()
    {
        _libVLC.Dispose();
    }
}
