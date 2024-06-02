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
        public class Memory
        {

            public byte[][] allProms = new byte[3][];  // Max 3 proms so far - memory, colour, extra
            public bool isColour = false;
            public static GetRomData.Games game;

            public Memory()
            {
                game = GetRomData.Games.None;
                allProms[0] = new byte[0x10000];
            }
            public void LoadRom(ref int[] keyBits, ref int rotate, GetRomData.Games newgame, String gameData, 
                                ref byte backCol, ref bool needsProcessing, ref Display.paletteType palType,
                                ref byte port_shift_result, ref byte port_shift_data,
                                ref byte port_shift_offset, ref byte[] port_inputs)
            {
                game = newgame;
                allProms = GetRomData.getRomData(game, gameData.Equals("") ? GetRomData.getRomData(game) : gameData, ref keyBits, ref rotate,
                                                        ref backCol, ref needsProcessing, ref palType, 
                                                        ref port_shift_result, ref port_shift_data,
                                                        ref port_shift_offset, ref port_inputs);
                if (allProms[1] != null) {
                    isColour = true;
                } else {
                    isColour = false;
                }
            }
        }
    }
}
