namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// <summary>
            /// The NOP instruction.
            /// </summary>
            byte NOP()
            {
                FetchFinished();
                return 4;
            }
        }
    }
}