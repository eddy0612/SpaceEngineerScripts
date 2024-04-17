﻿// AUTOGENERATED CODE
//
// Do not make changes directly to this (.cs) file.
// Change "LD (aa),rr        .tt" instead.

namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// <summary>
            /// The LD (aa),HL instruction.
            /// </summary>
            byte LD_aa_HL()
            {
                var address = (ushort)FetchWord();
                FetchFinished();

                WriteShortToMemory(address, RG.HL);

                return 16;
            }

            /// <summary>
            /// The LD (aa),DE instruction.
            /// </summary>
            byte LD_aa_DE()
            {
                var address = (ushort)FetchWord();
                FetchFinished();

                WriteShortToMemory(address, RG.DE);

                return 20;
            }

            /// <summary>
            /// The LD (aa),BC instruction.
            /// </summary>
            byte LD_aa_BC()
            {
                var address = (ushort)FetchWord();
                FetchFinished();

                WriteShortToMemory(address, RG.BC);

                return 20;
            }

            /// <summary>
            /// The LD (aa),SP instruction.
            /// </summary>
            byte LD_aa_SP()
            {
                var address = (ushort)FetchWord();
                FetchFinished();

                WriteShortToMemory(address, RG.SP);

                return 20;
            }

            /// <summary>
            /// The LD (aa),IX instruction.
            /// </summary>
            byte LD_aa_IX()
            {
                var address = (ushort)FetchWord();
                FetchFinished();

                WriteShortToMemory(address, RG.IX);

                return 20;
            }

            /// <summary>
            /// The LD (aa),IY instruction.
            /// </summary>
            byte LD_aa_IY()
            {
                var address = (ushort)FetchWord();
                FetchFinished();

                WriteShortToMemory(address, RG.IY);

                return 20;
            }
        }
    }
}