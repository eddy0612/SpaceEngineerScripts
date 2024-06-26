﻿// AUTOGENERATED CODE
//
// Do not make changes directly to this (.cs) file.
// Change "RRD +           .tt" instead.

namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// <summary>
            /// The RRD instruction.
            /// </summary>
            byte RRD()
            {
                FetchFinished();

                var memoryAddress = (ushort)RG.HL;

                var Avalue = RG.A;
                var HLcontents = ProcessorAgent.ReadFromMemory(memoryAddress);

                var newAvalue = (byte)((Avalue & 0xF0) | (HLcontents & 0x0F));
                var newHLcontents = (byte)(((HLcontents >> 4) & 0x0F) | ((Avalue << 4) & 0xF0));
                RG.A = newAvalue;
                ProcessorAgent.WriteToMemory(memoryAddress, newHLcontents);

                RG.SF = GetBit(newAvalue, 7);
                RG.ZF = (newAvalue == 0);
                RG.HF = 0;
                RG.PF = Parity[newAvalue];
                RG.NF = 0;
                SetFlags3and5From(newAvalue);

                return 18;
            }

            /// <summary>
            /// The RLD instruction.
            /// </summary>
            byte RLD()
            {
                FetchFinished();

                var memoryAddress = (ushort)RG.HL;

                var Avalue = RG.A;
                var HLcontents = ProcessorAgent.ReadFromMemory(memoryAddress);

                var newAvalue = (byte)((Avalue & 0xF0) | ((HLcontents >> 4) & 0x0F));
                var newHLcontents = (byte)(((HLcontents << 4) & 0xF0) | (Avalue & 0x0F));
                RG.A = newAvalue;
                ProcessorAgent.WriteToMemory(memoryAddress, newHLcontents);

                RG.SF = GetBit(newAvalue, 7);
                RG.ZF = (newAvalue == 0);
                RG.HF = 0;
                RG.PF = Parity[newAvalue];
                RG.NF = 0;
                SetFlags3and5From(newAvalue);

                return 18;
            }
        }
    }
}