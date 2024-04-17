namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// <summary>
            /// The NEG instruction.
            /// </summary>
            byte NEG()
            {
                FetchFinished();

                var oldValue = RG.A;
                var newValue = (byte)-oldValue;
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = (oldValue ^ newValue) & 0x10;
                RG.PF = (oldValue == 0x80);
                RG.NF = 1;
                RG.CF = (oldValue != 0);
                SetFlags3and5From(newValue);

                return 8;
            }
        }
    }
}