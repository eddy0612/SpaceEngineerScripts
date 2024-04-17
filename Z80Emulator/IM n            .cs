﻿// AUTOGENERATED CODE
//
// Do not make changes directly to this (.cs) file.
// Change "IM n            .tt" instead.


namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// <summary>
            /// The IM 0 instruction.
            /// </summary>
            private byte IM_0()
            {
                FetchFinished();

                ProcessorAgent.SetInterruptMode(0);

                return 8;
            }

            /// <summary>
            /// The IM 1 instruction.
            /// </summary>
            private byte IM_1()
            {
                FetchFinished();

                ProcessorAgent.SetInterruptMode(1);

                return 8;
            }

            /// <summary>
            /// The IM 2 instruction.
            /// </summary>
            private byte IM_2()
            {
                FetchFinished();

                ProcessorAgent.SetInterruptMode(2);

                return 8;
            }
        }
    }
}