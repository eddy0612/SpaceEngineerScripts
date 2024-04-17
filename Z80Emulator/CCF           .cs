namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// <summary>
            /// The CCF instruction.
            /// </summary>
            byte CCF()
            {
                FetchFinished();

                var oldCF = RG.CF;
                RG.NF = 0;
                RG.HF = oldCF;
                RG.CF = !oldCF;
                SetFlags3and5From(RG.A);

                return 4;
            }
        }
    }
}