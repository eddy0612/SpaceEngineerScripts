using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    partial class Program
    {
        public partial class Core
        {
            private static readonly int baseFreq = 60; // 60 ticks/sec?
            MyGridProgram mypgm = null;                 /* Main pgm if needed */

            public Core(MyGridProgram pgm) : this(new Core_Video())
            {
                mypgm = pgm;
            }
        }
    }
}