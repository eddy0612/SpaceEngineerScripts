﻿// AUTOGENERATED CODE
//
// Do not make changes directly to this (.cs) file.
// Change "INC rr +        .tt" instead.

namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// <summary>
            /// The INC BC instruction.
            /// </summary>
            byte INC_BC()
            {
                FetchFinished();
                RG.BC++;
                return 6;
            }

            /// <summary>
            /// The DEC BC instruction.
            /// </summary>
            byte DEC_BC()
            {
                FetchFinished();
                RG.BC--;
                return 6;
            }

            /// <summary>
            /// The INC DE instruction.
            /// </summary>
            byte INC_DE()
            {
                FetchFinished();
                RG.DE++;
                return 6;
            }

            /// <summary>
            /// The DEC DE instruction.
            /// </summary>
            byte DEC_DE()
            {
                FetchFinished();
                RG.DE--;
                return 6;
            }

            /// <summary>
            /// The INC HL instruction.
            /// </summary>
            byte INC_HL()
            {
                FetchFinished();
                RG.HL++;
                return 6;
            }

            /// <summary>
            /// The DEC HL instruction.
            /// </summary>
            byte DEC_HL()
            {
                FetchFinished();
                RG.HL--;
                return 6;
            }

            /// <summary>
            /// The INC SP instruction.
            /// </summary>
            byte INC_SP()
            {
                FetchFinished();
                RG.SP++;
                return 6;
            }

            /// <summary>
            /// The DEC SP instruction.
            /// </summary>
            byte DEC_SP()
            {
                FetchFinished();
                RG.SP--;
                return 6;
            }

            /// <summary>
            /// The INC IX instruction.
            /// </summary>
            byte INC_IX()
            {
                FetchFinished();
                RG.IX++;
                return 10;
            }

            /// <summary>
            /// The DEC IX instruction.
            /// </summary>
            byte DEC_IX()
            {
                FetchFinished();
                RG.IX--;
                return 10;
            }

            /// <summary>
            /// The INC IY instruction.
            /// </summary>
            byte INC_IY()
            {
                FetchFinished();
                RG.IY++;
                return 10;
            }

            /// <summary>
            /// The DEC IY instruction.
            /// </summary>
            byte DEC_IY()
            {
                FetchFinished();
                RG.IY--;
                return 10;
            }
        }
    }
}