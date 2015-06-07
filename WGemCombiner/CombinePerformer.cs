using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;

namespace WGemCombiner
{
    class CombinePerformer
    {

        /// <summary>
        /// List of instructions:
        /// X is gem to U/D/C
        /// Y is gem to C with, or -1 = D, or -2 = U
        /// </summary>
        public List<Point> inst = new List<Point>();
        public const int INST_DUPE = -99;
        public const int INST_UPGR = -98;
        const int SLOT_SIZE = 28;
        public const double NATIVE_SCREEN_HEIGHT = 612;//1088 x 612 says spy++, 600 flash version
        public const double NATIVE_SCREEN_WIDTH = 1088;//
        public double resolutionRatio = 1;

        public int Slots_Required;
        public bool limitSlots = true;

        public void SetMethod(string m, bool formula = false)
        {
            // Parses equation/formulas to compressed scheme
            if (formula)
                m = ParseFormula(m);
            m = m.Replace(" ", ""); // Remove spaces, whitespace is for human readers.
            m = m.Replace("\n", ""); // Remove newlines or the parser crashes

            // Parsing to base gems, and standardizing it to my ways.
            m = ParseScheme(m);

            SetGems(m); // Performs the combine pattern internally, recording the useage of each gem.
            CreateInstructions(); // Uses the result gem from SetGems and the useage data to create the instructions.
        }
        // Parses equation/formulas to compressed scheme
        private string ParseFormula(string str)
        {
            str = str.Replace(" ", "");
            string[] strP = str.Split('\t');

            string resStr = strP.Last().Split('=')[0];

            for (int i = strP.Length - 1; i > 2; i -= 1)
            {
                int ind = strP[i].IndexOf('\n');
                string s = strP[i];
                if (ind > 0)
                    s = s.Substring(0, ind);
                string[] strC = s.Split('=');
                if (i == 2)
                    i = 2;
                resStr = resStr.Replace(strC[0], "(" + strC[1] + ")");
            }

            if (resStr.Contains('0'))
            {
                resStr = resStr.Replace('1', '2');
                resStr = resStr.Replace('0', '1');
            }
            resStr = resStr.Substring(1, resStr.Length - 2);

            return resStr;
        }
        private List<Gem> baseGems;
        // All parsing except ParseFormula
        private string ParseScheme(string str)
        {
            baseGems = new List<Gem>();

            if (!schemeIsValid(str)) return "";
            // Is it from gemforce? (Letters)
            if (str.Contains('+') == true)
            {
                char letterOrNumber = str[str.IndexOf('+') - 1];
                if (letterOrNumber != '2' && letterOrNumber != '1')
                    str = ieeePreParser(str); // Replaces 4b with 3b, 2o with o, etc.
                else
                    str = str.Replace("2", "(1+1)");
            }
            else str = ieeePreParser(str); // must be like "8m"
            // Check which letters are used, and replace each one with a number indentifier for the internal combiner
            if (str.Contains('o'))
            {
                baseGems.Add(Gem.Base(Gem.COLOR_ORANGE));
                str = str.Replace("o", baseGems.Count.ToString());
            }
            if (str.Contains('y'))
            {
                baseGems.Add(Gem.Base(Gem.COLOR_YELLOW));
                str = str.Replace("y", baseGems.Count.ToString());
            }
            if (str.Contains('b'))
            {
                baseGems.Add(Gem.Base(Gem.COLOR_BLACK));
                str = str.Replace("b", baseGems.Count.ToString());
            }
            if (str.Contains('r'))
            {
                baseGems.Add(Gem.Base(Gem.COLOR_RED));
                str = str.Replace("r", baseGems.Count.ToString());
            }
            if (str.Contains('k'))
            {
                baseGems.Add(Gem.Base(Gem.COLOR_KILLGEM));
                str = str.Replace("k", baseGems.Count.ToString());
            }
            if (str.Contains('m'))
            {
                baseGems.Add(Gem.Base(Gem.COLOR_MANAGEM));
                str = str.Replace("m", baseGems.Count.ToString());
            }
            if (str.Contains('g')) // generic gem in all over the AG forum
            {
                baseGems.Add(Gem.Base(Gem.COLOR_NULL));
                str = str.Replace("g", baseGems.Count.ToString());
            }
            // If no letters were found, it still needs one base gem.
            if (baseGems.Count == 0)
                baseGems.Add(Gem.Base(Gem.COLOR_NULL));
            return str;
        }
        public string ieeePreParser(string recipe)
        {
            for (int i = 20; i > 1; i--)
            {
                string grd_str = i.ToString();
                int place = recipe.IndexOf(grd_str);
                while (place != -1)
                {
                    string color = recipe[place + grd_str.Length].ToString();
                    string weakerGem = (i - 1).ToString() + color;
                    if (i == 2)
                        weakerGem = color;
                    recipe = recipe.Replace(grd_str + color, "(" + weakerGem + "+" + weakerGem + ")");
                    place = recipe.IndexOf(grd_str);
                }
            }
            return recipe;
        }
        public bool schemeIsValid(string scheme)
        {
            if (scheme.Length < 2) return false;
            if (!evenBrackets(scheme)) return false;// Mismatched brackets
            if (scheme.Count(char.IsLetter) > 0 && !scheme.Contains('+') && scheme.Count(char.IsDigit) == 0) return false;// Only letters
            if (scheme.Count(char.IsDigit) > 0 && !scheme.Contains('+') && scheme.Count(char.IsLetter) == 0) return false;// Only digits
            if (scheme.Count(char.IsLetter) >= 2 && !scheme.Contains('+')) return false;

            return true;
        }

        private bool evenBrackets(string scheme)
        {
            int openingBrackets = 0;
            int closingBrackets = 0;
            foreach (char character in scheme)
            {
                if (character == '(') openingBrackets++;
                if (character == ')') closingBrackets++;
            }
            if (openingBrackets != closingBrackets) return false;
            return true;
        }

        public Gem resultGem;
        List<Gem> combined = new List<Gem>();
        List<Gem> orderedCombined;
        List<int> useCount = new List<int>();
        private void SetGems(string str)
        {
            combined.Clear();
            useCount.Clear();

            // Add baseGems to combined, and add values for their useage count.
            combined.Add(null);
            useCount.Add(-1);
            for (int i = 0; i < baseGems.Count; i++)
            {
                combined.Add(baseGems[i]);
                combined[i + 1].strID = (i + 1).ToString();
                useCount.Add(0);
            }

            do
            { // Get the first set of parenthesis and replace with incrementing id.
                int close = str.IndexOf(')');
                if (close == -1)
                    break;
                int open = str.LastIndexOf('(', close);
                string thisCombine = str.Substring(open + 1, close - open - 1);
                string[] cGems = thisCombine.Split('+');

                int gem1 = Convert.ToInt32(cGems[0]);
                int gem2 = Convert.ToInt32(cGems[1]);
                str = str.Replace("(" + gem1 + "+" + gem2 + ")", combined.Count.ToString());

                // Internal combines
                resultGem = Gem.Combine(combined[gem1], combined[gem2]);
                resultGem.strID = combined.Count.ToString();
                combined.Add(resultGem);

                useCount.Add(0); // Have to track times each gem is used.
                useCount[gem1]++; useCount[gem2]++;

            } while (true);
            // final combine
            if (str.Contains('+'))
            {  // if scheme was surrounded by parenthesis (e.g. ((1+1)+1)) this step is not required, was already done in loop.
                string[] lastGems = str.Split('+');

                int fgem1 = Convert.ToInt32(lastGems[0]);
                int fgem2 = Convert.ToInt32(lastGems[1]);

                resultGem = Gem.Combine(combined[fgem1], combined[fgem2]);
                resultGem.strID = combined.Count.ToString();
                combined.Add(resultGem);
                useCount.Add(0);
                useCount[fgem1]++; useCount[fgem2]++;
            }

            orderedCombined = new List<Gem>();
            orderedCombined.AddRange(combined);
        }
        private void CreateInstructions()
        {
            inst.Clear(); // Instructions list

            Gem[] inventory = new Gem[136]; // Array of what is in each of your inventory slots (Over 36 because this is before slot compression.)

            int[] slots = new int[combined.Count]; // Which slot each gem is in
            for (int i = 0; i < slots.Length; i++)
                slots[i] = -2;

            // Place baseGems in inventory slots
            for (int i = 0; i < baseGems.Count; i++)
            {
                inventory[i] = combined[i + 1];
                slots[i + 1] = i;
            }

            Slots_Required = 0;

            // All oneUse stuff is for slot compression. (This part of slot compression works.)
            List<int> oneUse = new List<int>();

            int startAt = baseGems.Count + 1; // Don't try to give instructinos for placing the base gems.
            for (int i = startAt; i < combined.Count; i++)
            {
                Gem g = combined[i];
                int c1 = Convert.ToInt32(g.Component1.strID);
                int c2 = Convert.ToInt32(g.Component2.strID);
                int slot1 = slots[c1];
                int slot2 = slots[c2];

                if (c1 == c2)
                { // Upgrade gem
                    useCount[c2] -= 2;
                    if (useCount[c2] > 0)
                    {
                        // DUPE
                        inst.Add(new Point(slot1, INST_DUPE));
                        slots[c1] = GetEmpty(inventory);
                        inventory[slots[c1]] = g.Component1;
                        CheckSlotReq(slots[c1]);
                    }
                    inst.Add(new Point(slot1, INST_UPGR));
                    inventory[slot1] = g;
                    slots[Convert.ToInt32(g.strID)] = slot1;
                }
                else
                {
                    if (useCount[c1] > 1)
                    { // DUPE
                        inst.Add(new Point(slot1, INST_DUPE));
                        slots[c1] = GetEmpty(inventory);
                        inventory[slots[c1]] = g.Component1;
                        CheckSlotReq(slots[c1]);
                    }
                    if (useCount[c2] > 1)
                    { // DUPE (repetitive code :/)
                        inst.Add(new Point(slot2, INST_DUPE));
                        slots[c2] = GetEmpty(inventory);
                        inventory[slots[c2]] = g.Component1;
                        CheckSlotReq(slots[c2]);
                    }
                    useCount[c1] -= 1;
                    useCount[c2] -= 1;
                    if (useCount[c1] == 1)
                        oneUse.Add(c1);
                    if (useCount[c2] == 1)
                        oneUse.Add(c2);

                    inst.Add(new Point(slot1, slot2));
                    inventory[slot1] = null;
                    inventory[slot2] = g;
                    slots[Convert.ToInt32(g.strID)] = slot2;
                    // None remaining? (Did not dupe)
                    // useCount[c1] == 0 should work here?
                    if (slots[c1] == slot1)
                        slots[c1] = -1;
                    if (slots[c2] == slot2)
                        slots[c2] = -1;

                    // Check oneUse combines
                    for (int iU = i + 2; iU < combined.Count; iU++)
                    {
                        Gem gU = combined[iU];
                        int c1U = Convert.ToInt32(gU.Component1.strID);
                        int c2U = Convert.ToInt32(gU.Component2.strID);
                        int slot1U = slots[c1U];
                        int slot2U = slots[c2U];

                        int oneID1 = oneUse.IndexOf(c1U);
                        int oneID2 = oneUse.IndexOf(c2U);
                        if (oneID1 != -1 && oneID2 != -1)
                        { // Combine, by shifting position in combined list.
                            combined.Insert(i + 1, combined[iU]);
                            combined.RemoveAt(iU + 1);
                            oneUse.RemoveAt(oneID1);
                            oneUse.Remove(c2U);
                            iU -= 2;
                        }
                        else if (GetEmpty(inventory) < Slots_Required && oneID1 != -1 && slot2U > 0)
                        { // If only one of the two is a oneUse, combine it anyway, if doing so will not put it over the current Slots_Req
                            combined.Insert(i + 1, combined[iU]);
                            combined.RemoveAt(iU + 1);
                            oneUse.RemoveAt(oneID1); iU--;
                        }
                        else if (GetEmpty(inventory) < Slots_Required && oneID2 != -1 && slot1U > 0)
                        {
                            combined.Insert(i + 1, combined[iU]);
                            combined.RemoveAt(iU + 1);
                            oneUse.RemoveAt(oneID2); iU--;
                        }
                    }
                }
            }

            // REDUCE SLOT REQUIREMENT
            if (limitSlots && Slots_Required > 36)
            {
                inst = CondenseSlots(resultGem, inst, false, 36);
                inst.Insert(0, new Point(0, -36));
            }
        }
        // CondenseSlots seems to be messed up, I can't get the 262144-combine to work.
        private List<Point> CondenseSlots(Gem g, List<Point> bigInst, bool keepBase, int slotLimit)
        {
            // Get the combine INST for both components. (Will not include duplicating base gem.)
            // If the combine for a component exceeds the slotLimit, CondenseSlots to get new INST.
            // 
            // Each component's combine INST must include placing the base gem in slot 0.
            // If I add the duplicate step to p1.inst, then the new condensed one will already have it.
            // To solve this, try: If the INST does not begin with duplicate step, add it.


            Gem c1 = g.Component1;
            Gem c2 = g.Component2;
            CombinePerformer p1 = new CombinePerformer(); p1.limitSlots = false;
            p1.SetMethod(c1.GetFullCombine()); p1.resultGem.strID = c1.strID;
            CombinePerformer p2 = new CombinePerformer(); p2.limitSlots = false;
            p2.SetMethod(c2.GetFullCombine()); p2.resultGem.strID = c2.strID;

            if (p1.Slots_Required > slotLimit - 1)
                p1.inst = CondenseSlots(c1, p1.inst, true, slotLimit);
            // Move result gem to highest open slot
            p1.inst.Add(new Point(p1.inst.Last().Y, -(slotLimit - 1))); // Move to 1st open space.
            if (p2.Slots_Required > slotLimit - 1)
                p2.inst = CondenseSlots(c2, p2.inst, false, slotLimit - 1);
            if (keepBase) // Is slot 36 used by baseGem?
                p2.inst.Add(new Point(p2.inst.Last().Y, -(slotLimit - 2))); // 2nd open (now 1st) space.
            else
                p2.inst.Add(new Point(p2.inst.Last().Y, -36));

            List<Point> newInst = new List<Point>();
            // Both combines require placing a base gem in slot 0 first.
            if (p1.inst[0].X != 35)
                newInst.Add(new Point(35, INST_DUPE));
            newInst.AddRange(p1.inst);

            if (keepBase || p2.Slots_Required > slotLimit - 1) // Duplicate base_gem
                newInst.Add(new Point(35, INST_DUPE));
            else // Move base_gem
                newInst.Add(new Point(35, -1));
            newInst.AddRange(p2.inst);

            // Combine the two resulting gems
            if (keepBase)
                newInst.Add(new Point((slotLimit - 3), (slotLimit - 2))); // Combine to the higher slot (shouldn't matter, last gem is moved later anyway)
            else
                newInst.Add(new Point((slotLimit - 2), 35));

            // Finally
            return newInst;
        }

        private int GetEmpty(Gem[] a)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == null)
                    return i;
            }
            return -1;
        }
        private void CheckSlotReq(int s)
        {
            if (s + 1 > Slots_Required)
                Slots_Required = s + 1;
        }

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);
        private void MoveCursor(int X, int Y)
        { Cursor.Position = new Point(X, Y); }
        private void PressMouse()
        { mouse_event(2, 0, 0, 0, UIntPtr.Zero); }
        private void ReleaseMouse()
        { mouse_event(4, 0, 0, 0, UIntPtr.Zero); }

        const uint KEYEVENTF_KEYUP = 0x2;
        [DllImport("user32.dll")]
        static extern bool keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        void PressKey(byte keyCode)
        {
            keybd_event(keyCode, 0, 0, UIntPtr.Zero);
            Thread.Sleep(3);
            keybd_event(keyCode, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        public delegate void DEL_STEPC(int stepID);
        public event DEL_STEPC StepComplete;
        public bool cancel_Combine = false;
        public int sleep_time = 33;
        public void PerformCombine(int mSteps)
        {
            const byte KEY_D = 0x44;
            const byte KEY_U = 0x55;
            Form1.logger.WriteLine("Performing a combine:");
            cancel_Combine = false;
            Point sA1 = Cursor.Position;
            for (int i = mSteps; i < inst.Count; i++)
            {
                Point sPos = GetSlotPos(inst[i].X);
                MoveCursor(sA1.X - (int)(sPos.X * SLOT_SIZE * resolutionRatio), sA1.Y - (int)(sPos.Y * SLOT_SIZE * resolutionRatio));
                Form1.logger.WriteLine("Moved cursor to\t\t" + inst[i].X + "\t\t" + Cursor.Position.ToString());
                //Thread.Sleep(sleep_time);
                if (inst[i].Y == INST_DUPE)
                {
                    PressKey(KEY_D);
                    //SendKeys.Send("d");
                    //SendKeys.SendWait("d");
                    //Thread.Sleep(sleep_time);
                    Form1.logger.WriteLine("Duped gem in slot\t\t" + inst[i].X + "\t\t" + Cursor.Position.ToString());
                }
                else if (inst[i].Y == INST_UPGR)
                {
                    PressKey(KEY_U);
                    //SendKeys.Send("u");
                    //SendKeys.SendWait("u");
                    //Thread.Sleep(sleep_time);
                    Form1.logger.WriteLine("Upgrd gem in slot\t\t" + inst[i].X + "\t\t" + Cursor.Position.ToString());
                }
                else if (inst[i].Y < 0) // Move gem (only used when slots are compressed)
                {
                    PressMouse();
                    Thread.Sleep(sleep_time);
                    sPos = GetSlotPos(-(inst[i].Y + 1));
                    MoveCursor(sA1.X - (int)(sPos.X * SLOT_SIZE * resolutionRatio), sA1.Y - (int)(sPos.Y * SLOT_SIZE * resolutionRatio));
                    ReleaseMouse();
                }
                else
                { // Try the button. Works wonders!
                    Form1.logger.WriteLine("Combining gems \t\t" + inst[i].X + " and " + inst[i].Y);
                    MoveCursor(sA1.X - (int)(-0.5 * SLOT_SIZE * resolutionRatio), sA1.Y - (int)(12.8 * SLOT_SIZE * resolutionRatio));
                    PressMouse(); ReleaseMouse();
                    MoveCursor(sA1.X - (int)(sPos.X * SLOT_SIZE * resolutionRatio), sA1.Y - (int)(sPos.Y * SLOT_SIZE * resolutionRatio));
                    PressMouse();
                    sPos = GetSlotPos(inst[i].Y);
                    MoveCursor(sA1.X - (int)(sPos.X * SLOT_SIZE * resolutionRatio), sA1.Y - (int)(sPos.Y * SLOT_SIZE * resolutionRatio));
                    ReleaseMouse();
                }

                if (StepComplete != null)
                    StepComplete.Invoke(i);
                if (cancel_Combine)
                    break;
                Thread.Sleep(sleep_time);
            }
        }
        private Point GetSlotPos(int s)
        {
            int row = s / 3;
            int column = s % 3;
            return new Point(column, row);
        }

        // Save a combine
        public byte[] GetSave()
        {
            // First, gem all gems. (combined already has them yay!!) Then sort by grade and cost.
            // Then save using GemCombiner's save.

            // Actually, I shouldn't need to sort combined since they are in an order with each combine only requiring previous ones.
            List<Point> gemCombines = new List<Point>();
            // Base gems
            for (int i = 1; i < orderedCombined.Count; i++)
            { // i = 0 is null, no ID 0 gem
                if (orderedCombined[i].Component1 == null)
                    continue;
                gemCombines.Add(new Point(i - 1, 0)); // Number of base gems
                break;
            }
            for (int i = gemCombines[0].X + 1; i < orderedCombined.Count; i++)
            {
                gemCombines.Add(new Point(Convert.ToInt32(orderedCombined[i].Component1.strID), Convert.ToInt32(orderedCombined[i].Component2.strID)));
            }

            byte[] saveBytes = new byte[gemCombines.Count * 8];
            for (int i = 0; i < gemCombines.Count; i++)
            {
                BitConverter.GetBytes(gemCombines[i].X).CopyTo(saveBytes, i * 8);
                BitConverter.GetBytes(gemCombines[i].Y).CopyTo(saveBytes, i * 8 + 4);
            }

            return saveBytes;
        }
        public static Gem LoadGem(byte[] lBytes, char[] colors = null)
        {
            // turn the array back in to a list of points
            List<Point> gemCombines = new List<Point>();
            int baseGems = BitConverter.ToInt32(lBytes, 0);
            gemCombines.Add(new Point(baseGems, 0));

            for (int i = 8; i < lBytes.Length; i += 8)
            {
                gemCombines.Add(new Point(BitConverter.ToInt32(lBytes, i),
                    BitConverter.ToInt32(lBytes, i + 4)));
            }

            // Get the gem by performing these combines.
            List<Gem> Gems = new List<Gem>();
            Gems.Add(null);
            for (int i = 0; i < gemCombines[0].X; i++)
            {
                if (colors != null)
                {
                    Gems.Add(Gem.Base(Gem.Clr(colors[i])));
                    Gems[Gems.Count - 1].strID = colors[i].ToString();
                }
                else
                    Gems.Add(Gem.Base(Gem.COLOR_NULL)); // TEMPORARY FIX
            }
            for (int i = 1; i < gemCombines.Count; i++)
            {
                Gem c1 = Gems[gemCombines[i].X];
                Gem c2 = Gems[gemCombines[i].Y];
                Gems.Add(Gem.Combine(c1, c2));
            }

            return Gems.Last();
        }

    }
}
