﻿// AUTOGENERATED CODE
//
// Do not make changes directly to this (.cs) file.
// Change "AND r +               .tt" instead.

using System;

namespace IngameScript
{
    partial class Program
    {
        public partial class Z80InstructionExecutor
        {
            /// The AND A instruction.
            /// </summary>
            private byte AND_A()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.A;
                var newValue = (byte)(oldValue & argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 1;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 4;
            }

            /// The XOR A instruction.
            /// </summary>
            private byte XOR_A()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.A;
                var newValue = (byte)(oldValue ^ argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 4;
            }

            /// The OR A instruction.
            /// </summary>
            private byte OR_A()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.A;
                var newValue = (byte)(oldValue | argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 4;
            }

            /// The AND B instruction.
            /// </summary>
            private byte AND_B()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.B;
                var newValue = (byte)(oldValue & argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 1;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 4;
            }

            /// The XOR B instruction.
            /// </summary>
            private byte XOR_B()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.B;
                var newValue = (byte)(oldValue ^ argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 4;
            }

            /// The OR B instruction.
            /// </summary>
            private byte OR_B()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.B;
                var newValue = (byte)(oldValue | argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 4;
            }

            /// The AND C instruction.
            /// </summary>
            private byte AND_C()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.C;
                var newValue = (byte)(oldValue & argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 1;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 4;
            }

            /// The XOR C instruction.
            /// </summary>
            private byte XOR_C()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.C;
                var newValue = (byte)(oldValue ^ argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 4;
            }

            /// The OR C instruction.
            /// </summary>
            private byte OR_C()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.C;
                var newValue = (byte)(oldValue | argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 4;
            }

            /// The AND D instruction.
            /// </summary>
            private byte AND_D()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.D;
                var newValue = (byte)(oldValue & argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 1;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 4;
            }

            /// The XOR D instruction.
            /// </summary>
            private byte XOR_D()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.D;
                var newValue = (byte)(oldValue ^ argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 4;
            }

            /// The OR D instruction.
            /// </summary>
            private byte OR_D()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.D;
                var newValue = (byte)(oldValue | argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 4;
            }

            /// The AND E instruction.
            /// </summary>
            private byte AND_E()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.E;
                var newValue = (byte)(oldValue & argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 1;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 4;
            }

            /// The XOR E instruction.
            /// </summary>
            private byte XOR_E()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.E;
                var newValue = (byte)(oldValue ^ argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 4;
            }

            /// The OR E instruction.
            /// </summary>
            private byte OR_E()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.E;
                var newValue = (byte)(oldValue | argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 4;
            }

            /// The AND H instruction.
            /// </summary>
            private byte AND_H()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.H;
                var newValue = (byte)(oldValue & argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 1;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 4;
            }

            /// The XOR H instruction.
            /// </summary>
            private byte XOR_H()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.H;
                var newValue = (byte)(oldValue ^ argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 4;
            }

            /// The OR H instruction.
            /// </summary>
            private byte OR_H()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.H;
                var newValue = (byte)(oldValue | argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 4;
            }

            /// The AND L instruction.
            /// </summary>
            private byte AND_L()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.L;
                var newValue = (byte)(oldValue & argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 1;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 4;
            }

            /// The XOR L instruction.
            /// </summary>
            private byte XOR_L()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.L;
                var newValue = (byte)(oldValue ^ argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 4;
            }

            /// The OR L instruction.
            /// </summary>
            private byte OR_L()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.L;
                var newValue = (byte)(oldValue | argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 4;
            }

            /// The AND (HL) instruction.
            /// </summary>
            private byte AND_aHL()
            {
                FetchFinished();

                var oldValue = RG.A;
                var address = (ushort)RG.HL;
                var argument = ProcessorAgent.ReadFromMemory(address);
                var newValue = (byte)(oldValue & argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 1;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 7;
            }

            /// The XOR (HL) instruction.
            /// </summary>
            private byte XOR_aHL()
            {
                FetchFinished();

                var oldValue = RG.A;
                var address = (ushort)RG.HL;
                var argument = ProcessorAgent.ReadFromMemory(address);
                var newValue = (byte)(oldValue ^ argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 7;
            }

            /// The OR (HL) instruction.
            /// </summary>
            private byte OR_aHL()
            {
                FetchFinished();

                var oldValue = RG.A;
                var address = (ushort)RG.HL;
                var argument = ProcessorAgent.ReadFromMemory(address);
                var newValue = (byte)(oldValue | argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 7;
            }

            /// The AND n instruction.
            /// </summary>
            private byte AND_n()
            {
                var argument = ProcessorAgent.FetchNextOpcode();
                FetchFinished();

                var oldValue = RG.A;
                var newValue = (byte)(oldValue & argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 1;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 7;
            }

            /// The XOR n instruction.
            /// </summary>
            private byte XOR_n()
            {
                var argument = ProcessorAgent.FetchNextOpcode();
                FetchFinished();

                var oldValue = RG.A;
                var newValue = (byte)(oldValue ^ argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 7;
            }

            /// The OR n instruction.
            /// </summary>
            private byte OR_n()
            {
                var argument = ProcessorAgent.FetchNextOpcode();
                FetchFinished();

                var oldValue = RG.A;
                var newValue = (byte)(oldValue | argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 7;
            }

            /// The AND IXH instruction.
            /// </summary>
            private byte AND_IXH()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.IXH;
                var newValue = (byte)(oldValue & argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 1;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 8;
            }

            /// The XOR IXH instruction.
            /// </summary>
            private byte XOR_IXH()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.IXH;
                var newValue = (byte)(oldValue ^ argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 8;
            }

            /// The OR IXH instruction.
            /// </summary>
            private byte OR_IXH()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.IXH;
                var newValue = (byte)(oldValue | argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 8;
            }

            /// The AND IXL instruction.
            /// </summary>
            private byte AND_IXL()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.IXL;
                var newValue = (byte)(oldValue & argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 1;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 8;
            }

            /// The XOR IXL instruction.
            /// </summary>
            private byte XOR_IXL()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.IXL;
                var newValue = (byte)(oldValue ^ argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 8;
            }

            /// The OR IXL instruction.
            /// </summary>
            private byte OR_IXL()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.IXL;
                var newValue = (byte)(oldValue | argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 8;
            }

            /// The AND IYH instruction.
            /// </summary>
            private byte AND_IYH()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.IYH;
                var newValue = (byte)(oldValue & argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 1;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 8;
            }

            /// The XOR IYH instruction.
            /// </summary>
            private byte XOR_IYH()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.IYH;
                var newValue = (byte)(oldValue ^ argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 8;
            }

            /// The OR IYH instruction.
            /// </summary>
            private byte OR_IYH()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.IYH;
                var newValue = (byte)(oldValue | argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 8;
            }

            /// The AND IYL instruction.
            /// </summary>
            private byte AND_IYL()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.IYL;
                var newValue = (byte)(oldValue & argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 1;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 8;
            }

            /// The XOR IYL instruction.
            /// </summary>
            private byte XOR_IYL()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.IYL;
                var newValue = (byte)(oldValue ^ argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 8;
            }

            /// The OR IYL instruction.
            /// </summary>
            private byte OR_IYL()
            {
                FetchFinished();

                var oldValue = RG.A;
                var argument = RG.IYL;
                var newValue = (byte)(oldValue | argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 8;
            }

            /// The AND (IX+n) instruction.
            /// </summary>
            private byte AND_aIX_plus_n()
            {
                var offset = ProcessorAgent.FetchNextOpcode();
                FetchFinished();

                var oldValue = RG.A;
                var address = (ushort)(RG.IX + (SByte)offset);
                var argument = ProcessorAgent.ReadFromMemory(address);
                var newValue = (byte)(oldValue & argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 1;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 19;
            }

            /// The XOR (IX+n) instruction.
            /// </summary>
            private byte XOR_aIX_plus_n()
            {
                var offset = ProcessorAgent.FetchNextOpcode();
                FetchFinished();

                var oldValue = RG.A;
                var address = (ushort)(RG.IX + (SByte)offset);
                var argument = ProcessorAgent.ReadFromMemory(address);
                var newValue = (byte)(oldValue ^ argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 19;
            }

            /// The OR (IX+n) instruction.
            /// </summary>
            private byte OR_aIX_plus_n()
            {
                var offset = ProcessorAgent.FetchNextOpcode();
                FetchFinished();

                var oldValue = RG.A;
                var address = (ushort)(RG.IX + (SByte)offset);
                var argument = ProcessorAgent.ReadFromMemory(address);
                var newValue = (byte)(oldValue | argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 19;
            }

            /// The AND (IY+n) instruction.
            /// </summary>
            private byte AND_aIY_plus_n()
            {
                var offset = ProcessorAgent.FetchNextOpcode();
                FetchFinished();

                var oldValue = RG.A;
                var address = (ushort)(RG.IY + (SByte)offset);
                var argument = ProcessorAgent.ReadFromMemory(address);
                var newValue = (byte)(oldValue & argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 1;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 19;
            }

            /// The XOR (IY+n) instruction.
            /// </summary>
            private byte XOR_aIY_plus_n()
            {
                var offset = ProcessorAgent.FetchNextOpcode();
                FetchFinished();

                var oldValue = RG.A;
                var address = (ushort)(RG.IY + (SByte)offset);
                var argument = ProcessorAgent.ReadFromMemory(address);
                var newValue = (byte)(oldValue ^ argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 19;
            }

            /// The OR (IY+n) instruction.
            /// </summary>
            private byte OR_aIY_plus_n()
            {
                var offset = ProcessorAgent.FetchNextOpcode();
                FetchFinished();

                var oldValue = RG.A;
                var address = (ushort)(RG.IY + (SByte)offset);
                var argument = ProcessorAgent.ReadFromMemory(address);
                var newValue = (byte)(oldValue | argument);
                RG.A = newValue;

                RG.SF = GetBit(newValue, 7);
                RG.ZF = (newValue == 0);
                RG.HF = 0;
                RG.PF = Parity[newValue];
                RG.NF = 0;
                RG.CF = 0;
                SetFlags3and5From(newValue);

                return 19;
            }
        }
    }
}