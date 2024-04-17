namespace IngameScript
{
    partial class Program
    {
        /// <summary>
        /// Default implementation of <see cref="IMainZ80Registers"/>.
        /// </summary>
        public class MainZ80Registers : IMainZ80Registers
        {
            public short AF { get; set; }

            public short BC { get; set; }

            public short DE { get; set; }

            public short HL { get; set; }

            public byte A
            {
                get { return Z80InstructionExecutor.GetHighByte(AF); }
                set { AF = Z80InstructionExecutor.SetHighByte(AF, value); }
            }

            public byte F
            {
                get { return Z80InstructionExecutor.GetLowByte(AF); }
                set { AF = Z80InstructionExecutor.SetLowByte(AF, value); }
            }

            public byte B
            {
                get { return Z80InstructionExecutor.GetHighByte(BC); }
                set { BC = Z80InstructionExecutor.SetHighByte(BC, value); }
            }

            public byte C
            {
                get { return Z80InstructionExecutor.GetLowByte(BC); }
                set { BC = Z80InstructionExecutor.SetLowByte(BC, value); }
            }

            public byte D
            {
                get { return Z80InstructionExecutor.GetHighByte(DE); }
                set { DE = Z80InstructionExecutor.SetHighByte(DE, value); }
            }

            public byte E
            {
                get { return Z80InstructionExecutor.GetLowByte(DE); }
                set { DE = Z80InstructionExecutor.SetLowByte(DE, value); }
            }

            public byte H
            {
                get { return Z80InstructionExecutor.GetHighByte(HL); }
                set { HL = Z80InstructionExecutor.SetHighByte(HL, value); }
            }

            public byte L
            {
                get { return Z80InstructionExecutor.GetLowByte(HL); }
                set { HL = Z80InstructionExecutor.SetLowByte(HL, value); }
            }

            public Bit CF
            {
                get { return Z80InstructionExecutor.GetBit(F, 0); }
                set { F = Z80InstructionExecutor.WithBit(F, 0, value); }
            }

            public Bit NF
            {
                get { return Z80InstructionExecutor.GetBit(F, 1); }
                set { F = Z80InstructionExecutor.WithBit(F, 1, value); }
            }

            public Bit PF
            {
                get { return Z80InstructionExecutor.GetBit(F, 2); }
                set { F = Z80InstructionExecutor.WithBit(F, 2, value); }
            }

            public Bit Flag3
            {
                get { return Z80InstructionExecutor.GetBit(F, 3); }
                set { F = Z80InstructionExecutor.WithBit(F, 3, value); }
            }

            public Bit HF
            {
                get { return Z80InstructionExecutor.GetBit(F, 4); }
                set { F = Z80InstructionExecutor.WithBit(F, 4, value); }
            }

            public Bit Flag5
            {
                get { return Z80InstructionExecutor.GetBit(F, 5); }
                set { F = Z80InstructionExecutor.WithBit(F, 5, value); }
            }

            public Bit ZF
            {
                get { return Z80InstructionExecutor.GetBit(F, 6); }
                set { F = Z80InstructionExecutor.WithBit(F, 6, value); }
            }

            public Bit SF
            {
                get { return Z80InstructionExecutor.GetBit(F, 7); }
                set { F = Z80InstructionExecutor.WithBit(F, 7, value); }
            }
        }
    }
}