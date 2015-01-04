using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGemCombiner
{
    class Gem : IComparable
    {
        public int Grade;
        public int Cost;
        // Components
        public double damage; // max damage
        public double critMult;
        public double leech;
        public double blood;
        public double Power
        {
            get
            {
                switch (GetColor())
                {
                    case COLOR_ORANGE:
                        return leech;
                    case COLOR_BLACK:
                        return blood;
                    case COLOR_MANAGEM:
                        return leech * blood;
                    case COLOR_YELLOW:
                        return damage * critMult;
                    case COLOR_KILLGEM:
                        return damage * critMult * blood * blood; // 1+blood makes no sense, g1 bb is set to 1 by convention
                    case COLOR_NULL:
                        return 1; // so that growth does not go to infinity
                }
                // Red have no mana or kill power
                return 0;
            }
        }
        public const int COLOR_ORANGE = 1;
        public const int COLOR_BLACK = 2;
        public const int COLOR_MANAGEM = 3;
        public const int COLOR_YELLOW = 4;
        public const int COLOR_KILLGEM = 5;
        public const int COLOR_RED = 6;
        public const int COLOR_NULL = 7; // the null gem, to be used as placeholder
        public int GetColor()
        {
            if (critMult != 0.0)
            { // Has a yellow component
                if (blood != 0.0)
                    return COLOR_KILLGEM;
                else
                    return COLOR_YELLOW;
            }
            else if (leech != 0.0)
            { // Has an orange component
                if (blood != 0.0)
                    return COLOR_MANAGEM;
                else
                    return COLOR_ORANGE;
            }
            else if (damage > 0) // has damage
                return COLOR_RED;
            else return COLOR_NULL;
        }
        public static int Clr(char c)
        {
            if (c == 'o')
                return COLOR_ORANGE;
            else if (c == 'b')
                return COLOR_BLACK;
            else if (c == 'm')
                return COLOR_MANAGEM;
            else if (c == 'y')
                return COLOR_YELLOW;
            else if (c == 'k')
                return COLOR_KILLGEM;
            else if (c == 'r')
                return COLOR_RED;
            else
                return COLOR_NULL;
        }

        public string strID;

        public Gem Component1;
        public Gem Component2;

        public double Growth = 1; // ???? Math.Log10(1.379) / Math.Log10(2);

        public static Gem Combine(Gem g1, Gem g2)
        {
            Gem ret = new Gem();

            if (g2.Cost > g1.Cost)
            {
                ret.Component1 = g2;
                ret.Component2 = g1;
            }
            else
            {
                ret.Component1 = g1;
                ret.Component2 = g2;
            }

            if (g1.Grade == g2.Grade)
            {
                ret.Grade = g1.Grade + 1;
                ret.damage = g1.damage > g2.damage ? 0.87 * g1.damage + 0.71 * g2.damage : 0.87 * g2.damage + 0.71 * g1.damage;
                ret.leech = g1.leech > g2.leech ? 0.88 * g1.leech + 0.50 * g2.leech : 0.88 * g2.leech + 0.50 * g1.leech;
                ret.blood = g1.blood > g2.blood ? 0.78 * g1.blood + 0.31 * g2.blood : 0.78 * g2.blood + 0.31 * g1.blood;
                ret.critMult = g1.critMult > g2.critMult ? 0.88 * g1.critMult + 0.50 * g2.critMult : 0.88 * g2.critMult + 0.50 * g1.critMult;
            }
            else if (g1.Grade == g2.Grade + 1)
            {
                ret.Grade = g1.Grade;
                ret.damage = g1.damage > g2.damage ? 0.86 * g1.damage + 0.70 * g2.damage : 0.86 * g2.damage + 0.70 * g1.damage;
                ret.leech = g1.leech > g2.leech ? 0.89 * g1.leech + 0.44 * g2.leech : 0.89 * g2.leech + 0.44 * g1.leech;
                ret.blood = g1.blood > g2.blood ? 0.79 * g1.blood + 0.29 * g2.blood : 0.79 * g2.blood + 0.29 * g1.blood;
                ret.critMult = g1.critMult > g2.critMult ? 0.88 * g1.critMult + 0.44 * g2.critMult : 0.88 * g2.critMult + 0.44 * g1.critMult;
            }
            else if (g1.Grade == g2.Grade - 1)
            {
                ret.Grade = g2.Grade;
                ret.damage = g1.damage > g2.damage ? 0.86 * g1.damage + 0.70 * g2.damage : 0.86 * g2.damage + 0.70 * g1.damage;
                ret.leech = g1.leech > g2.leech ? 0.89 * g1.leech + 0.44 * g2.leech : 0.89 * g2.leech + 0.44 * g1.leech;
                ret.blood = g1.blood > g2.blood ? 0.79 * g1.blood + 0.29 * g2.blood : 0.79 * g2.blood + 0.29 * g1.blood;
                ret.critMult = g1.critMult > g2.critMult ? 0.88 * g1.critMult + 0.44 * g2.critMult : 0.88 * g2.critMult + 0.44 * g1.critMult;
            }
            else
            {
                ret.Grade = g1.Grade;
                if (g2.Grade > g1.Grade)
                    ret.Grade = g2.Grade;

                ret.damage = g1.damage > g2.damage ? 0.85 * g1.damage + 0.69 * g2.damage : 0.85 * g2.damage + 0.69 * g1.damage;
                ret.leech = g1.leech > g2.leech ? 0.90 * g1.leech + 0.38 * g2.leech : 0.90 * g2.leech + 0.38 * g1.leech;
                ret.blood = g1.blood > g2.blood ? 0.80 * g1.blood + 0.27 * g2.blood : 0.80 * g2.blood + 0.27 * g1.blood;
                ret.critMult = g1.critMult > g2.critMult ? 0.88 * g1.critMult + 0.44 * g2.critMult : 0.88 * g2.critMult + 0.44 * g1.critMult;
            }

            ret.damage = Math.Max(ret.damage, g1.damage);
            ret.damage = Math.Max(ret.damage, g2.damage);
            ret.Cost = ret.Component1.Cost + ret.Component2.Cost;
            ret.Growth = Math.Log10(ret.Power) / Math.Log10(ret.Cost);

            return ret;
        }


        public string GetCombine()
        {
            string c1 = "";
            if (Component1 == null)
                c1 = "1";
            else
                c1 = Component1.strID;

            string c2 = "";
            if (Component2 == null)
                c2 = "1";
            else
                c2 = Component2.strID;

            return c1 + " + " + c2;
        }
        public string GetFullCombine()
        {
            string c1 = "";
            if (Component1 == null || Component1.strID == "")
                c1 = "1";
            else if (Component1.Cost == 1)
                c1 = Component1.strID;
            else
                c1 = "(" + Component1.GetFullCombine() + ")";

            string c2 = "";
            if (Component2 == null || Component2.strID == "")
                c2 = "1";
            else if (Component2.Cost == 1)
                c2 = Component2.strID;
            else
                c2 = "(" + Component2.GetFullCombine() + ")";

            if (c1 == "(1+1)")
                c1 = "2";
            if (c2 == "(1+1)")
                c2 = "2";
            return c1 + "+" + c2;
        }

        public static Gem Base(int Color)
        {
            Gem b = new Gem();
            b.Grade = 0;
            b.Cost = 1;
            b.damage = 0; // gems that needs damage will have that properly setted (dmg_yellow=1)

            if (Color == COLOR_BLACK)
            {
                b.damage = 1.186168;
                b.blood = 1.0;
            }
            else if (Color == COLOR_KILLGEM)
            {
                b.damage = 1.0;
                b.critMult = 1.0;
                b.blood = 1.0;
            }
            else if (Color == COLOR_MANAGEM)
            {
                b.leech = 1.0;
                b.blood = 1.0;
            }
            else if (Color == COLOR_ORANGE)
                b.leech = 1.0;
            else if (Color == COLOR_YELLOW)
            {
                b.damage = 1.0;
                b.critMult = 1.0;
            }
            else if (Color == COLOR_RED)
                b.damage = 0.909091;

            return b;
        }

        public int CompareTo(object g1)
        {
            return Cost.CompareTo((g1 as Gem).Cost);
        }
    }
}
