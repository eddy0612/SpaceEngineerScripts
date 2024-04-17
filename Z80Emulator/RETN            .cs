﻿// AUTOGENERATED CODE
//
// Do not make changes directly to this (.cs) file.
// Change "RETN            .tt" instead.


namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// <summary>
            /// The RETN instruction.
            /// </summary>
            private byte RETN()
            {
                FetchFinished(isRet: true);

                var sp = (ushort)RG.SP;
                var newPC = CreateShort(
                    ProcessorAgent.ReadFromMemory(sp),
                    ProcessorAgent.ReadFromMemory((ushort)(sp + 1)));
                RG.PC = (ushort)newPC;

                RG.SP += 2;

                RG.IFF1 = RG.IFF2;

                return 14;
            }
        }
    }
}