//
// Space Engineer based version specifics - abstract the SE stuff into a single class
//    to enable easy porting from windows app to SE.
//
using System;
using VRageMath;
using Sandbox.ModAPI.Ingame;
using System.Collections.Generic;
using VRage.GameServices;
using static IngameScript.Program;

namespace IngameScript
{
    partial class Program
    {
        public class Specifics
        {
            public Arcade8080Machine am;
            public Display display;

            public JDBG jdbg;
            public JCTRL jctrl;
            public IMyShipController controller;
            public Program mypgm;

            public static int maxFrames = 45000;
            public static int skipFrames = 0;
            public static int skipFramesLeft = 0;

            bool space = false; // fire
            bool left = false;
            bool right = false;
            bool crouch = false;  // spare
            bool up = false;
            bool down = false;
            bool q = false;    // insert coin
            bool e = false;    // start button

            public void CheckKeys()
            {
                if (left != jctrl.IsLeft(controller)) {
                    left = !left;
                    am.handleInput(am.keyBits[(int)GetRomData.KeyIndex.keyleft], left);
                }
                if (right != jctrl.IsRight(controller)) {
                    right = !right;
                    am.handleInput(am.keyBits[(int)GetRomData.KeyIndex.keyright], right);
                }
                if (up != jctrl.IsUp(controller)) {
                    up = !up;
                    am.handleInput(am.keyBits[(int)GetRomData.KeyIndex.keyup], up);
                }
                if (down != jctrl.IsDown(controller)) {
                    down = !down;
                    am.handleInput(am.keyBits[(int)GetRomData.KeyIndex.keydown], down);
                }
                if (space != jctrl.IsSpace(controller)) {
                    space = !space;
                    am.handleInput(am.keyBits[(int)GetRomData.KeyIndex.space], space);
                }
                if (crouch != jctrl.IsCrouch(controller)) {
                    crouch = !crouch;
                    am.handleInput(am.keyBits[(int)GetRomData.KeyIndex.crouch], crouch);
                }
                if (am.cost == 0 && (q != jctrl.IsRollLeft(controller))) {
                    q = !q;
                    am.handleInput(am.keyBits[(int)GetRomData.KeyIndex.q], q);
                }
                if (e != jctrl.IsRollRight(controller)) {
                    e = !e;
                    am.handleInput(am.keyBits[(int)GetRomData.KeyIndex.e], e);
                }
            }

            public void DebugAndEcho(String s)
            {
                jdbg.DebugAndEcho(s);
            }
            public void Echo(String s)
            {
                jdbg.Echo(s);
            }

            public int GetInstructionCount()
            {
                return mypgm.Runtime.CurrentInstructionCount;
            }

            public bool drawAndRenderFrame(ref int State, ref int actualDraws)
            {
                bool didComplete = true;
                if (skipFramesLeft <= 1) {
                    didComplete = display.generateFrameToDisplay(ref State);

                    if (!didComplete) return false;

                    /* We built it ok! */
                    actualDraws++;
                    mypgm.jlcd.WriteToAllLCDs(mypgm.displays, new String(DirectBitmap.Pixels), false);
                    skipFramesLeft = skipFrames;

                } else {
                    skipFramesLeft--;
                }

                return true;
            }

            public void setRotation(int rotate)
            {
                mypgm.jlcd.SetLCDRotation(mypgm.displays, (float)rotate);
            }
        }
    }
}
