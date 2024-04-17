using System;

namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// <summary>
            /// The DJNZ d instruction.
            /// </summary>
            byte DJNZ_d()
            {
                var offset = ProcessorAgent.FetchNextOpcode();
                FetchFinished();

                var oldValue = RG.B;
                RG.B = (byte)(oldValue - 1);

                if (oldValue == 1)
                    return 8;

                RG.PC = (ushort)(RG.PC + (SByte)offset);
                return 13;
            }
        }
    }
}