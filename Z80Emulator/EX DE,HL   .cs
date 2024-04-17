namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// <summary>
            /// The EX DE,HL instruction
            /// </summary>
            byte EX_DE_HL()
            {
                FetchFinished();

                var temp = RG.DE;
                RG.DE = RG.HL;
                RG.HL = temp;

                return 4;
            }
        }
    }
}