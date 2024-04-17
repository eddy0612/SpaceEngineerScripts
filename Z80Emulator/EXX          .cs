namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// <summary>
            /// The EXX instruction.
            /// </summary>
            byte EXX()
            {
                FetchFinished();

                var tempBC = RG.BC;
                var tempDE = RG.DE;
                var tempHL = RG.HL;

                RG.BC = RG.Alternate.BC;
                RG.DE = RG.Alternate.DE;
                RG.HL = RG.Alternate.HL;

                RG.Alternate.BC = tempBC;
                RG.Alternate.DE = tempDE;
                RG.Alternate.HL = tempHL;

                return 4;
            }
        }
    }
}