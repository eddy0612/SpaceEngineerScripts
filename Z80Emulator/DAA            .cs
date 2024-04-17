namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// <summary>
            /// The DAA instruction.
            /// </summary>
            byte DAA()
            {

                FetchFinished();

                //Algorithm borrowed from MAME:
                //https://github.com/mamedev/mame/blob/master/src/emu/cpu/z80/z80.c

                var oldValue = RG.A;
                var newValue = oldValue;

                if (RG.HF || (oldValue & 0x0F) > 9) newValue = (byte)(newValue + (RG.NF ? -0x06 : 0x06)); //FA
                if (RG.CF || oldValue > 0x99) newValue = (byte)(newValue + (RG.NF ? -0x60 : 0x60)); //A0

                RG.CF |= (oldValue > 0x99);
                RG.HF = ((oldValue ^ newValue) & 0x10);
                RG.SF = (newValue & 0x80);
                RG.ZF = (newValue == 0);
                RG.PF = Parity[newValue];
                SetFlags3and5From(newValue);

                RG.A = newValue;

                return 4;
            }
        }
    }
}