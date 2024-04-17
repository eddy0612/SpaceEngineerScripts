namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// <summary>
            /// The EI instruction.
            /// </summary>
            byte EI()
            {
                FetchFinished(isEiOrDi: true);

                RG.IFF1 = 1;
                RG.IFF2 = 1;

                return 4;
            }
        }
    }
}