using System.Collections.Generic;
using System.Numerics;
using Gdk;
using Gtk;
using Cairo;
using HOMM_Battles.PersonaficationAndUI;
using HOMM_Battles.MapMechanics;
using HOMM_Battles.TurnQueue;
using Pango;
using System.ComponentModel.DataAnnotations;

class SpriteSheetLoader
{
    public static List<Pixbuf> LoadAndScaleSprites(string path, int columns, int rows, int tileWidth, int tileHeight, int targetWidth, int targetHeight)
    {
        Pixbuf sheet = new Pixbuf(path);
        List<Pixbuf> sprites = new List<Pixbuf>();

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                int px = x * tileWidth;
                int py = y * tileHeight;
                Pixbuf bigSprite = new Pixbuf(sheet, px, py, tileWidth, tileHeight);
                Pixbuf scaledSprite = bigSprite.ScaleSimple(targetWidth, targetHeight, InterpType.Bilinear);
                sprites.Add(scaledSprite);
            }
        }

        return sprites;
    }
}


namespace HOMM_Battles.Units
{
    public struct DamageRange
    {
        Random random = new Random();
        public int min;
        public int max;

        public int mult = 1;

        public DamageRange(int min_, int max_)
        {
            min = min_;
            max = max_;
        }

        public DamageRange(int val)
        {
            min = val;
            max = val;
        }

        public bool Contains(int value) => value >= min && value <= max;
        public int ChooseRand() => mult * random.Next(min, max + 1);
    }

    public abstract class Unit
    {
        public string name;
        public int health;
        public int attackDist;
        public int speed;
        public int force;
        public int defence;
        public DamageRange damage;
        public Hexacell currentHex;
        public int amount;
        public bool team;
        public List<Pixbuf> units = SpriteSheetLoader.LoadAndScaleSprites("Assets/Spritesheet_2.png", 4, 4, 256, 256, 64, 64);
        public Pixbuf sprite;
        protected bool isActive = true;
        protected bool isConterattacking;
        protected int healthLeft = 0;

        public Unit(int amount_, Hexacell hex, bool team_, int i)
        {
            amount = amount_;
            currentHex = hex;
            team = team_;
            sprite = team ? units[i] : units[i].Flip(true);
        }

        public async Task NextCellAsync(Hexacell targetHex)
        {
            currentHex.DeleteUnit();
            targetHex.SetUnit(this);
            currentHex = targetHex;

            await Task.Delay(50);
        }

        public async Task GoToPositionAsync(Queue<Hexacell> path)
        {
            foreach (Hexacell next in path)
            {
                await NextCellAsync(next);
            }
        }

        public bool IsDead() => !isActive;

        public virtual void Defend(Unit target)
        {
            int absorbedDamage = target.damage.ChooseRand() * target.amount * (1 + (target.force - defence) / 10);
            healthLeft -= absorbedDamage;

            if (healthLeft <= 0) { Death(); }
            else { amount = (healthLeft + health - 1) / health; }
        }

        public virtual bool CanAttack(Unit target) => target.team != team;

        public virtual void Attack(Unit target)
        {
            target.Defend(this);
            if (target.isConterattacking) Defend(target);
        }

        private void Death()
        {
            isActive = false;
            amount = 0;
            Console.Beep();
        }
    }
}
