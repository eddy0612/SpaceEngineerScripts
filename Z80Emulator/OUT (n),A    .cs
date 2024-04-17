namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// <summary>
            /// The OUT (n),A instruction.
            /// </summary>
            byte OUT_n_A()
            {
                var portNumber = ProcessorAgent.FetchNextOpcode();
                FetchFinished();

                ProcessorAgent.WriteToPort(portNumber, RG.A);

                return 11;
            }
        }
    }
}