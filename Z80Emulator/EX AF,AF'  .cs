namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// <summary>
            /// The EX AF,AF' instruction
            /// </summary>
            byte EX_AF_AF()
            {
                FetchFinished();

                var temp = RG.AF;
                RG.AF = RG.Alternate.AF;
                RG.Alternate.AF = temp;

                return 4;
            }
        }
    }
}