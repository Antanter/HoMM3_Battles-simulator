using LibVLCSharp.Shared;
using HOMM_Battles.PersonaficationAndUI;

static public class MusicPlayer
{
    static private LibVLC _libVLC;
    static private MediaPlayer _mediaPlayer;
    static private Dictionary<string, string> _tracks;

    static MusicPlayer()
    {
        Core.Initialize();
        _libVLC = new LibVLC();
        _mediaPlayer = new MediaPlayer(_libVLC);

        _tracks = new Dictionary<string, string>
        {
            { "main_menu", "Assets/Music/main_menu.mp3" },
            { "steps", "Assets/Music/steps.mp3" },
            { "attack_1",    "Assets/Music/attack_1.mp3" },
            { "attack_2",    "Assets/Music/attack_2.mp3" },
            { "attack_3",    "Assets/Music/attack_3.mp3" },
            { "attack_4",    "Assets/Music/attack_4.mp3" },
            { "death",    "Assets/Music/death.mp3" },
            { "endschpiele",    "Assets/Music/endschpiele.mp3" }
        };
    }

    static public void Play()
    {
        if (!PlayerAccount.musicEnabled) { Stop(); return; };
        if (_mediaPlayer.IsPlaying) return;

        _mediaPlayer.Play();
    }


    static public void SetTrack(string trackName, float speed = 1.0f)
    {
        if (!_tracks.ContainsKey(trackName)) return;

        var media = new Media(_libVLC, _tracks[trackName], FromType.FromPath);
        Stop();
        _mediaPlayer.Media = media;
        media.Dispose();

        _mediaPlayer.SetRate(speed);
    }

    static public void Stop()
    {
        _mediaPlayer.Stop();
    }

    static public void Dispose() {
        _mediaPlayer.Dispose();
        _libVLC.Dispose();
    }
}
