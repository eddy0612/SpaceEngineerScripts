using System;

namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// <summary>
            /// The JR d instruction.
            /// </summary>
            byte JR_d()
            {
                var offset = ProcessorAgent.FetchNextOpcode();
                FetchFinished();

                RG.PC = (ushort)(RG.PC + (SByte)offset);

                return 12;
            }
        }
    }
}