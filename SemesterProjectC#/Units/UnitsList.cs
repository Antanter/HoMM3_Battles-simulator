using Gdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace HOMM_Battles.Units
{
    public enum UnitType
    {
        Peasant,
        Archer,
        Griffin,
        Swordsman,
        Monk,
        Cavalier,
        Angel,



        Skeleton,
        Zombie,
        Ghost,
        Vampire,
        Lich,
        DeathKnight,
        BoneDragon
    }
    
    // Castle

    public class Peasant : Unit
    {
        public Peasant(int amount_, Hexacell hex, bool team_) : base(amount_, hex, team_)
        {
            name = "Peasant";
            health = 7;
            speed = 8;
            force = 1;
            defence = 1;
            damage = new DamageRange(1, 2);
            isConterattacking = true;
            sprite = new Pixbuf("Assets/peasant.png").ScaleSimple(35, 35, InterpType.Bilinear);
            healthLeft = amount * health;
        }
    }

    public class Archer : Unit
    {
        public Archer(int amount_, Hexacell hex, bool team_) : base(amount_, hex, team_)
        {
            name = "Archer";
            health = 10;
            speed = 5;
            force = 4;
            defence = 2;
            damage = new DamageRange(2, 5);
            isConterattacking = true;
            sprite = new Pixbuf("Assets/archer.png").ScaleSimple(35, 35, InterpType.Bilinear);
            healthLeft = amount * health;
        }
    }

    public class Griffin : Unit
    {
        public Griffin(int amount_, Hexacell hex, bool team_) : base(amount_, hex, team_)
        {
            name = "Griffin";
            health = 18;
            speed = 7;
            force = 6;
            defence = 5;
            damage = new DamageRange(4, 6);
            isConterattacking = true;
            sprite = new Pixbuf("Assets/griffin.png").ScaleSimple(35, 35, InterpType.Bilinear);
            healthLeft = amount * health;
        }
    }

    public class Swordsman : Unit
    {
        public Swordsman(int amount_, Hexacell hex, bool team_) : base(amount_, hex, team_)
        {
            name = "Swordsman";
            health = 40;
            speed = 4;
            force = 6;
            defence = 4;
            damage = new DamageRange(3, 6);
            isConterattacking = true;
            sprite = new Pixbuf("Assets/swordsman.png").ScaleSimple(35, 35, InterpType.Bilinear);
            healthLeft = amount * health;
        }
    }

    public class Monk : Unit
    {
        public Monk(int amount_, Hexacell hex, bool team_) : base(amount_, hex, team_)
        {
            name = "Monk";
            health = 35;
            speed = 6;
            force = 8;
            defence = 6;
            damage = new DamageRange(5, 8);
            isConterattacking = true;
            sprite = new Pixbuf("Assets/monk.png").ScaleSimple(35, 35, InterpType.Bilinear);
            healthLeft = amount * health;
        }
    }

    public class Cavalier : Unit
    {
        public Cavalier(int amount_, Hexacell hex, bool team_) : base(amount_, hex, team_)
        {
            name = "Cavalier";
            health = 24;
            speed = 8;
            force = 12;
            defence = 10;
            damage = new DamageRange(8, 12);
            isConterattacking = true;
            sprite = new Pixbuf("Assets/cavalier.png").ScaleSimple(35, 35, InterpType.Bilinear);
            healthLeft = amount * health;
        }
    }

    public class Angel : Unit
    {
        public Angel(int amount_, Hexacell hex, bool team_) : base(amount_, hex, team_)
        {
            name = "Angel";
            health = 90;
            speed = 9;
            force = 20;
            defence = 18;
            damage = new DamageRange(50);
            isConterattacking = true;
            sprite = new Pixbuf("Assets/angel.png").ScaleSimple(35, 35, InterpType.Bilinear);
            healthLeft = amount * health;
        }
    }

    //Necropolis

    public class Skeleton : Unit
    {
        public Skeleton(int amount_, Hexacell hex, bool team_) : base(amount_, hex, team_)
        {
            name = "Skeleton";
            health = 6;
            speed = 4;
            force = 5;
            defence = 5;
            damage = new DamageRange(1, 3);
            isConterattacking = true;
            sprite = new Pixbuf("Assets/skeleton.png").ScaleSimple(35, 35, InterpType.Bilinear);
            healthLeft = amount * health;
        }
    }

    public class Zombie : Unit
    {
        public Zombie(int amount_, Hexacell hex, bool team_) : base(amount_, hex, team_)
        {
            name = "Zombie";
            health = 12;
            speed = 2;
            force = 6;
            defence = 5;
            damage = new DamageRange(3, 5);
            isConterattacking = true;
            sprite = new Pixbuf("Assets/zombie.png").ScaleSimple(35, 35, InterpType.Bilinear);
            healthLeft = amount * health;
        }
    }

    public class Ghost : Unit
    {
        public Ghost(int amount_, Hexacell hex, bool team_) : base(amount_, hex, team_)
        {
            name = "Ghost";
            health = 8;
            speed = 6;
            force = 4;
            defence = 3;
            damage = new DamageRange(2, 4);
            isConterattacking = false;
            sprite = new Pixbuf("Assets/ghost.png").ScaleSimple(35, 35, InterpType.Bilinear);
            healthLeft = amount * health;
        }
    }

    public class Vampire : Unit
    {
        public Vampire(int amount_, Hexacell hex, bool team_) : base(amount_, hex, team_)
        {
            name = "Vampire";
            health = 18;
            speed = 5;
            force = 8;
            defence = 6;
            damage = new DamageRange(4, 6);
            isConterattacking = true;
            sprite = new Pixbuf("Assets/vampire.png").ScaleSimple(35, 35, InterpType.Bilinear);
            healthLeft = amount * health;
        }
    }

    public class Lich : Unit
    {
        public Lich(int amount_, Hexacell hex, bool team_) : base(amount_, hex, team_)
        {
            name = "Lich";
            health = 20;
            speed = 4;
            force = 10;
            defence = 7;
            damage = new DamageRange(5, 8);
            isConterattacking = true;
            sprite = new Pixbuf("Assets/lich.png").ScaleSimple(35, 35, InterpType.Bilinear);
            healthLeft = amount * health;
        }
    }

    public class DeathKnight : Unit
    {
        public DeathKnight(int amount_, Hexacell hex, bool team_) : base(amount_, hex, team_)
        {
            name = "Death Knight";
            health = 16;
            speed = 4;
            force = 7;
            defence = 6;
            damage = new DamageRange(4, 7);
            isConterattacking = true;
            sprite = new Pixbuf("Assets/wight.png").ScaleSimple(35, 35, InterpType.Bilinear);
            healthLeft = amount * health;
        }
    }

    public class BoneDragon : Unit
    {
        public BoneDragon(int amount_, Hexacell hex, bool team_) : base(amount_, hex, team_)
        {
            name = "Bone Dragon";
            health = 40;
            speed = 6;
            force = 15;
            defence = 12;
            damage = new DamageRange(8, 12);
            isConterattacking = true;
            sprite = new Pixbuf("Assets/bone_dragon.png").ScaleSimple(35, 35, InterpType.Bilinear);
            healthLeft = amount * health;
        }
    }
}
