using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOMM_Battles.PersonaficationAndUI;

static class PlayerAccount
{
    static public bool musicEnabled = true;
    static public void ToggleMusic() => musicEnabled = !musicEnabled;
}