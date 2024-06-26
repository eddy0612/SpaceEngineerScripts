//-----------------------------------------------------------------------------
// Using code initially from 
//    https://github.com/BluestormDNA/i8080-Space-Invaders
// Then modified/stripped/optimized in order to meet the requirements for Space
// Engineers and support for various other games added
//-----------------------------------------------------------------------------
using System;

namespace IngameScript
{
    partial class Program
    {
        public class Arcade8080Machine
        {
            Cpu cpu;
            Memory memory;
            Display display;
            public Bus iobus;
            public int[] keyBits;
            public Specifics specifics;
            bool needsProcessing = false;
            int processIndex = 0;
            public int cost = 0;
            public bool showStates = false;
            public GetRomData.Games game;

            // NOTE: Key stuff in specifics for lagunar/zzzap
            public Arcade8080Machine(Specifics specs, GetRomData.Games gameType, String gameData, int cost)
            {
                specifics = specs;
                specs.am = this;

                int rotate270 = 270;
                memory = new Memory();
                byte backCol = 0;
                Display.paletteType palType = Display.paletteType.RBG;

                byte port_shift_result = 0xFF; 
                byte port_shift_data = 0xFF;
                byte port_shift_offset = 0xFF;
                byte[] port_inputs = new byte[4] { 0xff, 0xff, 0xff, 0xff };
                this.cost = cost;
                this.game = gameType;
                memory.LoadRom(ref keyBits, ref rotate270, gameType, gameData, ref backCol, ref needsProcessing, ref palType, ref port_shift_result, ref port_shift_data,
                                ref port_shift_offset, ref port_inputs);

                iobus = new Bus(keyBits, port_shift_result, port_shift_data, port_shift_offset, port_inputs);
                cpu = new Cpu(memory, iobus);

                display = new Display(memory, specifics, this);
                display.palType = palType;
                display.rotate = rotate270;
                Display.backGroundCol = backCol;
                specifics.display = display;
                DirectBitmap.Palette = null;
                State = -3;
            }

            public int State = -3; // ( -3 == loaded, -2 == midfixup, -1 == begin, 0 == cpupart1, 1 == cpupart2, 2 == drawscreen )

            public void part_exe(ref int curFrames, ref int actFrames)
            {
                // Do we need to process the rom?
                if (State == -3) {
                    if (needsProcessing) {
                        processIndex = 0;
                        State = -2;
                    } else {
                        State = -1;
                    }
                }

                if (State == -2) {
                    if (GetRomData.processRom(ref processIndex, ref memory.allProms, specifics, Memory.game)) {
                        cpu.memory = memory.allProms[0];
                        State = -1;
                    } else {
                        return; // More processing to do - out of CPU time
                    }
                }

                // Always process the keys - this is really quick anyway
                specifics.CheckKeys();

                bool seenBegin = false;

                // Now continue the loop until we run out of cycles OR cycle to finish the screen twice
                while (true) {
                    if (State >= 2) {
                        // Drawing the screen can take up to 11000 cycles
                        if (specifics.GetInstructionCount() < Specifics.maxFrames) {
                            bool finished = specifics.drawAndRenderFrame(ref State, ref actFrames);
                            if (finished) {
                                if (game == GetRomData.Games.skylove ||
                                    game == GetRomData.Games.shuttlei) {
                                    iobus.input |= (byte)0x02;
                                }
                                curFrames++;
                                State = -1;
                                if (showStates) specifics.Echo("Moved to state " + State + " - " + specifics.GetInstructionCount());
                            } else {
                                return;
                            }
                        } else {
                            return;
                        }
                    }

                    /* Throttle at 60fps support */
                    if (State == -1) {
                        if (seenBegin) return; // Throttle at 1 frame per tick max
                        seenBegin = true;
                        State = 0;
                        if (showStates) specifics.Echo("Moved to state " + State + " - " + specifics.GetInstructionCount());

                        // Move wheel on those games
                        if ((game == GetRomData.Games.lagunar) ||
                            (game == GetRomData.Games.bowler) ||
                            (game == GetRomData.Games.z280zzzap)) {
                            if (specifics.xaxis_state == -1) {
                                if (specifics.xaxis_value > 7) specifics.xaxis_value -= specifics.xaxis_tick_delta;
                            } else if (specifics.xaxis_state == 1) {
                                if (specifics.xaxis_value < 247) specifics.xaxis_value += specifics.xaxis_tick_delta;
                            }
                            
                            if (specifics.yaxis_state == -1) {
                                if (specifics.yaxis_value > 7) specifics.yaxis_value -= specifics.yaxis_tick_delta;
                            } else if (specifics.yaxis_state == 1) {
                                if (specifics.yaxis_value < 247) specifics.yaxis_value += specifics.yaxis_tick_delta;
                            }
                            if ((game == GetRomData.Games.bowler)) {
                                iobus.input = specifics.yaxis_value;
                                iobus.input3 = specifics.xaxis_value;
                            } else {
                                iobus.input = specifics.xaxis_value;
                            }
                        }
                    }

                    // Get here, its cpu time
                    while ((cpu.cycles < 16666) && (specifics.GetInstructionCount() < Specifics.maxFrames)) {
                        cpu.exe();
                    }

                    // Now either interrupt or return
                    if (specifics.GetInstructionCount() >= Specifics.maxFrames) return;

                    // Its interrupt time
                    if (cpu.cycles >= 16666) {
                        if (game == GetRomData.Games.skylove ||
                            game == GetRomData.Games.shuttlei) {
                            int whichBit = 0x02;
                            iobus.input &= (byte)(~whichBit);
                        }
                        cpu.cycles = 0;
                        if (State == 0) {
                            cpu.handleInterrupt(1);
                            State = 1;
                            if (showStates) specifics.Echo("Moved to state " + State + " - " + specifics.GetInstructionCount());
                        } else {
                            cpu.handleInterrupt(2);
                            State = 2;
                            if (showStates) specifics.Echo("Moved to state " + State + " - " + specifics.GetInstructionCount());
                        }
                    }
                }


            }

            public void insertCoin(bool pushed)
            {
                int whichBit = keyBits[(int)Program.GetRomData.KeyIndex.q];
                // Assumed never use input3 for coins
                bool isInput1 = true;
                if (whichBit > 0xFF) {
                    isInput1 = false;
                    whichBit = whichBit >> 8;
                }
                specifics.DebugAndEcho("Coin inserted: " + whichBit + "/" + pushed + " at " + DateTime.Now);

                if ((keyBits[(int)Program.GetRomData.KeyIndex.initmask] & keyBits[(int)Program.GetRomData.KeyIndex.q]) > 0) {

                    if (!pushed) {
                        if (isInput1) {
                            iobus.input |= (byte)whichBit;
                        } else {
                            iobus.input2 |= (byte)whichBit;
                        }
                    } else {
                        if (isInput1) {
                            iobus.input &= (byte)~whichBit;
                        } else {
                            iobus.input2 &= (byte)~whichBit;
                        }
                    }

                    // ACTIVE_HIGH
                } else {
                    if (pushed) {
                        if (isInput1) {
                            iobus.input |= (byte)whichBit;
                        } else {
                            iobus.input2 |= (byte)whichBit;
                        }
                    } else {
                        if (isInput1) {
                            iobus.input &= (byte)~whichBit;
                        } else {
                            iobus.input2 &= (byte)~whichBit;
                        }
                    }
                }

            }

            public void handleInput(int b, Boolean pushed)
            {
                byte whichBit;
                bool isInput1 = true;
                bool isInput2 = false;
                bool isInput3 = false;

                if (b > 0xff00) {
                    isInput1 = false;
                    isInput3 = true;
                    whichBit = (byte)(b >> 16);
                } else if (b > 0xff) {
                    isInput1 = false;
                    isInput2 = true;
                    whichBit = (byte)(b>>8);
                } else {
                    whichBit = (byte)b;
                }

                // ACTIVE_LOW:
                if ((keyBits[(int)Program.GetRomData.KeyIndex.initmask] & b) > 0)
                {

                    if (!pushed) {
                        if (isInput1) {
                            iobus.input |= whichBit;
                        } else if (isInput2) {
                            iobus.input2 |= whichBit;
                        } else if (isInput3) {
                            iobus.input3 |= whichBit;
                        }
                    } else {
                        if (isInput1) {
                            iobus.input &= (byte)~whichBit;
                        } else if (isInput2) {
                            iobus.input2 &= (byte)~whichBit;
                        } else if (isInput3) {
                            iobus.input3 &= (byte)~whichBit;
                        }
                    }

                    // ACTIVE_HIGH
                } else {
                    if (pushed) {
                        if (isInput1) {
                            iobus.input |= whichBit;
                        } else if (isInput2) {
                            iobus.input2 |= whichBit;
                        } else if (isInput3) {
                            iobus.input3 |= whichBit;
                        }
                    } else {
                        if (isInput1) {
                            iobus.input &= (byte)~whichBit;
                        } else if (isInput2) {
                            iobus.input2 &= (byte)~whichBit;
                        } else if (isInput3) {
                            iobus.input3 &= (byte)~whichBit;
                        }
                    }
                }
            }

        }
    }
}
