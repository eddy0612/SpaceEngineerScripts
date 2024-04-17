namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// <summary>
            /// The HALT instruction.
            /// </summary>
            byte HALT()
            {
                FetchFinished(isHalt: true);

                return 4;
            }
        }
    }
}