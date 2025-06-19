using LibVLCSharp.Shared;
using HOMM_Battles.PersonaficationAndUI;

static public class MusicPlayer
{
    static private LibVLC _libVLC;
    static private Dictionary<string, string> _tracks;
    static private Dictionary<string, MediaPlayer> _activePlayers = new();

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

    static public void PlayMusic(string id, string trackName, bool loop = false, float speed = 1.0f)
    {
        if (!_tracks.ContainsKey(trackName) || !PlayerAccount.musicEnabled)
            return;

        StopMusic(id);

        var mediaPlayer = new MediaPlayer(_libVLC);
        var media = new Media(_libVLC, _tracks[trackName], FromType.FromPath);
        mediaPlayer.Media = media;
        media.Dispose();

        mediaPlayer.SetRate(speed);

        if (loop)
        {
            mediaPlayer.EndReached += (_, _) =>
            {
                mediaPlayer.Stop();
                mediaPlayer.Play();
            };
        }
        else
        {
            mediaPlayer.EndReached += (_, _) =>
            {
                //StopMusic(id);
                //mediaPlayer.Dispose();
                _activePlayers.Remove(id);
            };
        }

        _activePlayers[id] = mediaPlayer;
        mediaPlayer.Play();
    }

    static public void StopMusic(string id)
    {
        if (_activePlayers.TryGetValue(id, out var player))
        {
            player.Stop();
            player.Dispose();
            _activePlayers.Remove(id);
        }
    }

    static public void StopAll()
    {
        foreach (var kvp in _activePlayers)
        {
            kvp.Value.Stop();
            kvp.Value.Dispose();
        }
        _activePlayers.Clear();
    }

    static public void Dispose()
    {
        StopAll();
        _libVLC.Dispose();
    }
}
