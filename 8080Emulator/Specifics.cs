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

            /* Values for lagunar & 280zzzap - Since these are the only games, not putting them */
            /* all in per game config but some set in processRom                                */
            /* Also reused for bowler, which is supposed to be a trackball                      */
            public byte xaxis_value = 0x7f;
            public int xaxis_state = 0; // 0 == not l or r
            public byte xaxis_tick_delta = 1;   // 20 - lagunat/280zzzap, 3 bowler - see processRom

            // Only used for bowler:
            public byte yaxis_value = 7;
            public int yaxis_state = 0; // 0 == not u or d
            public byte yaxis_tick_delta = 5;

            /* Used as gear stick for driving games as a toggled value */
            bool oldCrouchState = false;

            /* Although there's lots of duplication below, I've tried to keep the odd games behind */
            /* a single initial conditional, meaning most games performance is unaffected          */
            public void CheckKeys()
            {
                /* Steering wheel / trackball support (wheel self centers on release */
                if ((am.game == GetRomData.Games.lagunar) ||
                    (am.game == GetRomData.Games.bowler) ||
                    (am.game == GetRomData.Games.z280zzzap)) {

                    // left 
                    if (left != jctrl.IsLeft(controller)) {
                        left = !left;
                        if (left) {
                            xaxis_state = 1;
                            if (xaxis_value > 7) xaxis_value -= xaxis_tick_delta;
                        } else {
                            // For steering wheel games, if you let go it recenters
                            xaxis_state = 0;
                            if ((am.game == GetRomData.Games.lagunar) ||
                                (am.game == GetRomData.Games.z280zzzap)) {
                                xaxis_value = 0x7f;
                            }
                        }
                    }
                    if (right != jctrl.IsRight(controller)) {
                        right = !right;
                        if (right) {
                            xaxis_state = -1;
                            if (xaxis_value < 247) xaxis_value += xaxis_tick_delta;
                        } else {
                            // For steering wheel games, if you let go it recenters
                            xaxis_state = 0;
                            if ((am.game == GetRomData.Games.lagunar) ||
                                (am.game == GetRomData.Games.z280zzzap)) {
                                xaxis_value = 0x7f;
                            }
                        }
                    }

                    if ((am.game == GetRomData.Games.bowler)) {
                        // up down
                        if (up != jctrl.IsUp(controller)) {
                            up = !up;
                            if (up) {
                                yaxis_state = 1;
                                if (yaxis_value > 7) yaxis_value -= yaxis_tick_delta;
                            } else {
                                yaxis_state = 0;
                                yaxis_value = 0x00; // Bowler resets to 0
                            }
                        }
                        if (down != jctrl.IsDown(controller)) {
                            down = !down;
                            if (down) {
                                yaxis_state = -1;
                                if (yaxis_value < 247) yaxis_value += yaxis_tick_delta;
                            } else {
                                yaxis_state = 0;
                                yaxis_value = 0x00; // Bowler resets to 0
                            }
                        }
                        /* Default crouch */
                        if (crouch != jctrl.IsCrouch(controller)) {
                            crouch = !crouch;
                            am.handleInput(am.keyBits[(int)GetRomData.KeyIndex.crouch], crouch);
                        }

                    } else {
                        /* Default up/down for lagunar and 280zzzap */
                        if (up != jctrl.IsUp(controller)) {
                            up = !up;
                            am.handleInput(am.keyBits[(int)GetRomData.KeyIndex.keyup], up);
                        }
                        if (down != jctrl.IsDown(controller)) {
                            down = !down;
                            am.handleInput(am.keyBits[(int)GetRomData.KeyIndex.keydown], down);
                        }
                        /* Crouch acts as a toggle each time its pressed */
                        if (crouch != jctrl.IsCrouch(controller)) {
                            crouch = !crouch;
                            if (crouch) oldCrouchState = !oldCrouchState;
                            jdbg.Debug("Crounch pressed? " + crouch + ", state: " + oldCrouchState);
                            am.handleInput(am.keyBits[(int)GetRomData.KeyIndex.crouch], oldCrouchState);
                        }
                    }

                } else {
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
                    if (crouch != jctrl.IsCrouch(controller)) {
                        crouch = !crouch;
                        am.handleInput(am.keyBits[(int)GetRomData.KeyIndex.crouch], crouch);
                    }
                }
                if (space != jctrl.IsSpace(controller)) {
                    space = !space;
                    am.handleInput(am.keyBits[(int)GetRomData.KeyIndex.space], space);
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
