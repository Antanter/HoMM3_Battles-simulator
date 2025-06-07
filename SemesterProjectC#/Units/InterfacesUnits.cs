using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOMM_Battles.Units
{
    public interface IFlyable
    {
        void Fly();
    }

    public interface IShootable
    {
        void Shoot();
    }
}
