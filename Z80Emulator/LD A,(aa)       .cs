namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// <summary>
            /// The LD A,(nn) instruction.
            /// </summary>
            private byte LD_A_aa()
            {
                var address = (ushort)FetchWord();
                FetchFinished();

                RG.A = ProcessorAgent.ReadFromMemory(address);

                return 13;
            }
        }
    }
}