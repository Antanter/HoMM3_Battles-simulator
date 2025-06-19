using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOMM_Battles.PersonaficationAndUI;

static class PlayerAccount
{
    static private bool _musicEnabled;

    static public bool musicEnabled
    {
        get => _musicEnabled;
        set
        {
            _musicEnabled = value;
            OnMyFlagChanged(value);
        }
    }

    static public event Action<bool> MyFlagChanged;

    static private void OnMyFlagChanged(bool newValue)
    {
        MyFlagChanged?.Invoke(newValue);
    }
}