namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// <summary>
            /// The IN A,(n) instruction.
            /// </summary>
            byte IN_A_n()
            {
                var portNumber = ProcessorAgent.FetchNextOpcode();
                FetchFinished();

                RG.A = ProcessorAgent.ReadFromPort(portNumber);

                return 11;
            }
        }
    }
}