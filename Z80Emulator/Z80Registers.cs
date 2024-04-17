using System;

namespace IngameScript
{
    /// <summary>
    /// Represents a full set of Z80 registers. This is the default implementation of
    /// <see cref="IZ80Registers"/>.
    /// </summary>
    partial class Program
    {
        public class Z80Registers : MainZ80Registers, IZ80Registers
        {
            public Z80Registers()
            {
                Alternate = new MainZ80Registers();
            }

            private IMainZ80Registers _Alternate;

            public IMainZ80Registers Alternate
            {
                get { return _Alternate; }
                set
                {
                    if (value == null)
                        throw new ArgumentNullException("Alternate");

                    _Alternate = value;
                }
            }

            public short IX { get; set; }

            public short IY { get; set; }

            public ushort PC { get; set; }

            public short SP { get; set; }

            public short IR { get; set; }

            public Bit IFF1 { get; set; }

            public Bit IFF2 { get; set; }

            public byte IXH
            {
                get { return Z80InstructionExecutor.GetHighByte(IX); }
                set { IX = Z80InstructionExecutor.SetHighByte(IX, value); }
            }

            public byte IXL
            {
                get { return Z80InstructionExecutor.GetLowByte(IX); }
                set { IX = Z80InstructionExecutor.SetLowByte(IX, value); }
            }

            public byte IYH
            {
                get { return Z80InstructionExecutor.GetHighByte(IY); }
                set { IY = Z80InstructionExecutor.SetHighByte(IY, value); }
            }

            public byte IYL
            {
                get { return Z80InstructionExecutor.GetLowByte(IY); }
                set { IY = Z80InstructionExecutor.SetLowByte(IY, value); }
            }

            public byte I
            {
                get { return Z80InstructionExecutor.GetHighByte(IR); }
                set { IR = Z80InstructionExecutor.SetHighByte(IR, value); }
            }

            public byte R
            {
                get { return Z80InstructionExecutor.GetLowByte(IR); }
                set { IR = Z80InstructionExecutor.SetLowByte(IR, value); }
            }
        }
    }
}