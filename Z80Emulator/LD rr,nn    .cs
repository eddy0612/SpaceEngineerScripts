﻿// AUTOGENERATED CODE
//
// Do not make changes directly to this (.cs) file.
// Change "LD rr,nn    .tt" instead.


namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// <summary>
            /// The LD BC,nn instruction.
            /// </summary>
            byte LD_BC_nn()
            {
                var value = FetchWord();
                FetchFinished();
                RG.BC = value;
                return 10;
            }

            /// <summary>
            /// The LD DE,nn instruction.
            /// </summary>
            byte LD_DE_nn()
            {
                var value = FetchWord();
                FetchFinished();
                RG.DE = value;
                return 10;
            }

            /// <summary>
            /// The LD HL,nn instruction.
            /// </summary>
            byte LD_HL_nn()
            {
                var value = FetchWord();
                FetchFinished();
                RG.HL = value;
                return 10;
            }

            /// <summary>
            /// The LD SP,nn instruction.
            /// </summary>
            byte LD_SP_nn()
            {
                var value = FetchWord();
                FetchFinished(isLdSp: true);
                RG.SP = value;
                return 10;
            }

            /// <summary>
            /// The LD IX,nn instruction.
            /// </summary>
            byte LD_IX_nn()
            {
                var value = FetchWord();
                FetchFinished();
                RG.IX = value;
                return 14;
            }

            /// <summary>
            /// The LD IY,nn instruction.
            /// </summary>
            byte LD_IY_nn()
            {
                var value = FetchWord();
                FetchFinished();
                RG.IY = value;
                return 14;
            }
        }
    }
}