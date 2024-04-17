namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// <summary>
            /// The DI instruction.
            /// </summary>
            byte DI()
            {
                FetchFinished(isEiOrDi: true);

                RG.IFF1 = 0;
                RG.IFF2 = 0;

                return 4;
            }
        }
    }
}