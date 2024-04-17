//-----------------------------------------------------------------------------
// Using code initially from 
//    https://github.com/BluestormDNA/i8080-Space-Invaders
// Then modified/optimized in order to meet the requirements for Space Engineers
//-----------------------------------------------------------------------------
using System;

namespace IngameScript
{
    partial class Program
    {
        public class Bus
        {

            short shift;
            byte offset;
            public Cpu cpu;

            bool testMode = false;
            public bool test_finished = false;

            byte port_shift_result = 0xFF;
            byte port_shift_data = 0xFF;
            byte port_shift_offset = 0xFF;
            byte port_input = 0xFF;


            private bool BIT(int x, int n) { return ((x >> n) & 0x01) > 0; }

            public Bus(byte[] keyBits, byte port_shift_result, byte port_shift_data, 
                byte port_shift_offset, byte port_input)
            {
                // Some games are coin is on active_low, and requires the bits high to start
                // with
                if (keyBits == null) {
                    testMode = true; 
                } else {
                    input = keyBits[(int)Program.GetRomData.KeyIndex.initmask];
                }

                this.port_shift_result = port_shift_result;
                this.port_shift_data = port_shift_data;
                this.port_shift_offset = port_shift_offset;
                this.port_input = port_input;
            }

            public byte input { get; set; }

            byte lower3bitMask = 0x07; //0000 0111 covers amount to shift from 0 to 7

            public static bool rollingc_saved = false;
            public byte Read(byte b)
            { //in
                if (testMode) return 0;
                byte answer = 0x00;

                // Things usually needed:
                if (b == port_input) {
                    answer = input;
                } else if (b == port_shift_result) {
                    answer = (byte)((shift >> offset) & 0xff);
                } else {
                    switch (b) {
                        case 0x00:
                            if (Memory.game == GetRomData.Games.galxwars) {
                                answer = 0x40; // copy protection 
                            } else if (Memory.game == GetRomData.Games.astropal) {
                                answer = 0x1; // 3 lives
                            } else if (Memory.game == GetRomData.Games.rollingc) {
                                // Game select with crouch
                                int newVal = (input & 0x80);
                                input = (byte)(input & 0x7F);
                                if (newVal > 0) rollingc_saved = !rollingc_saved;
                                if (rollingc_saved) return 0x02;
                                else return 0x04;
                            } else if (Memory.game == GetRomData.Games.spacerng) {  //invadpt2
                                answer = 0xf5;
                            } else if (Memory.game == GetRomData.Games.lupin3) {
                                answer = 0x03;
                            } else if (Memory.game == GetRomData.Games.polaris) {
                                answer = (byte)(input & 0xf8);
                            } else if (Memory.game == GetRomData.Games.indianbt) {
                                if (cpu.PC == 0x5fec) {   // copy protection 
                                    answer = 0x10;
                                } else if (cpu.PC == 0x5ffb) {
                                    answer = 0x00;
                                }
                            } else if (Memory.game == GetRomData.Games.vortex) {
                                answer = 0x80; // 1 coin per play
                            }

                            break;
                        case 0x02:
                            if (Memory.game == GetRomData.Games.indianbt) {
                                answer = 0x03;
                            } else if (Memory.game == GetRomData.Games.vortex) {
                                answer = 0xff;
                            } else {
                                answer = 0;
                            }
                            break;
                        case 0x03:
                            if (Memory.game == GetRomData.Games.astropal) {
                                answer = 0x80;
                            }
                            break;
                    }
                }

                //if (Memory.game == GetRomData.Games.vortex) answer = (byte)(answer ^ 0x04);

                //Console.WriteLine("Port - Read : " + b.ToString("X") + " answer:" + answer.ToString("X2"));
                return answer;
            }

            public void Write(byte b, byte A)
            { //out
                /*if (testMode) {
                    if (b == 0) {
                        test_finished = true;
                    } else if (b == 1) {
                        byte op = cpu.C;
                        if (op == 2) {
                            Console.Write((char)cpu.E); 
                        } else if (op == 9) {
                            int addr = cpu.DE; // (cpu.D << 8 | cpu.E); 
                            do {
                                Console.Write((char)cpu.memory[addr]);
                                addr++;
                            } while (cpu.memory[addr] != '$');
                            Console.WriteLine("");
                        }
                    }
                    return;
                }*/
                //Console.WriteLine("Port - Write : " + b.ToString("X") + " , " + A.ToString("X"));
                // Things usually needed:
                if (b == port_shift_offset) {
                    offset = (byte)((~A) & lower3bitMask);
                } else if (b == port_shift_data) {
                    shift = (short)((shift >> 8) | (((short)A) << 7));
                } else {
                    switch (b) {
                        case 0x03:
                            if ((Memory.game == GetRomData.Games.ballbomb) ||
                                (Memory.game == GetRomData.Games.lrescue) ||
                                (Memory.game == GetRomData.Games.spacerng) ||
                                (Memory.game == GetRomData.Games.schaser) ||
                                (Memory.game == GetRomData.Games.rollingc)) {
                                Display.isRed = (A & 0x04) > 0;
                            }
                            break;

                        case 0x04:
                            // lupin3 - m_color_map = data & 0x40
                            if (Memory.game == GetRomData.Games.galxwars) {
                                Display.isRed = (A & 0x04) > 0;
                            }
                            break;

                        case 0x05:
                            if (Memory.game == GetRomData.Games.schaser) {
                                Display.backgroundDisable = ((A >> 3) & 1) != 0;
                                Display.backgroundSelect = ((A >> 4) & 1) != 0;
                            } else if (Memory.game == GetRomData.Games.invrvnge) {
                                Display.isRed = (A & 0x10) > 0;
                            } else if (Memory.game == GetRomData.Games.lupin3) {
                                Display.m_color_map = (A & 0x40) > 0;   // Is this the same as isRed?
                            } else if (Memory.game == GetRomData.Games.galxwars ||
                                       Memory.game == GetRomData.Games.rollingc ||
                                       Memory.game == GetRomData.Games.ballbomb) {
                                Display.m_color_map = BIT(A, 5);   // NOT the same as isRed..
                            }
                            break;
                    }
                }
            }
        }
    }
}
