using System;

namespace IngameScript
{
    partial class Program
    {
        class Core_CPU
        {
        }

        public partial class Core : IMemory, IZ80InterruptSource
        {
            private static readonly int cpuClockFreq = 3500000;
            private readonly Z80Processor cpu;
            //private float cpuLastTCount;
            private readonly float cpuSpeed;
            private readonly byte[] cpuRam;
            private bool cpuIrq;

            public int Size => cpuRam.Length;

            public byte this[int address]
            {
                get { return cpuRam[address]; }

                set { cpuRam[address] = value; }
            }

            //#pragma warning disable CS0067
            //        public event EventHandler NmiInterruptPulse;
            //#pragma warning restore CS0067

            public bool IntLineIsActive
            {
                get
                {
                    if (cpuIrq)
                    {
                        cpuIrq = false;
                        return true;
                    }

                    return false;
                }
            }

            public byte? ValueOnDataBus => 255;

            private Core(Core_CPU _)
            {
                cpu = new Z80Processor();
                cpuIrq = false;
                //cpuLastTCount = 0;
                cpuSpeed = (float)cpuClockFreq / (float)baseFreq;
                cpuRam = new byte[65536];
                Array.Copy(ResourceLoader.Spectrum48KROM(), cpuRam, 16384);
                cpu.SetMemoryAccessMode(0, 16384, MemoryAccessMode.ReadOnly);
                cpu.Memory = this;
                cpu.RegisterInterruptSource(this);
            }

            public void CPU_Execute_Block()
            {
                mypgm.Echo("Running code");
                /*                float tCount = cpuLastTCount;
                                while (tCount < cpuSpeed)
                                    tCount += cpu.ExecuteNextInstruction();
                                cpuLastTCount = tCount - cpuSpeed;*/
                mypgm.Echo("Drawing screen");
                BuildVideoScreen();
            }

            /*private void CPU_Interrupt()
            {
                cpuIrq = true;
                Video_Display();
            }*/

            public void SetContents(int startAddress, byte[] contents, int startIndex = 0, int? length = null)
            {
                Array.Copy(contents, 0, cpuRam, startIndex, length ?? contents.Length);
            }

            public byte[] GetContents(int startAddress, int length)
            {
                byte[] bytes = new byte[length];
                Array.Copy(cpuRam, startAddress, bytes, 0, length);
                return bytes;
            }
        }
    }
}