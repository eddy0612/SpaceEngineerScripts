namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// <summary>
            /// The CPL instruction.
            /// </summary>
            byte CPL()
            {
                FetchFinished();

                RG.A = (byte)(RG.A ^ 0xFF);

                RG.HF = 1;
                RG.NF = 1;
                SetFlags3and5From(RG.A);

                return 4;
            }
        }
    }
}