using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        public class JCTRL
        {
            MyGridProgram mypgm = null;
            JDBG jdbg = null;
            bool singleKey = false;

            public JCTRL(MyGridProgram pgm, JDBG dbg, bool SingleKeyOnly)
            {
                mypgm = pgm;
                jdbg = dbg;
            }

            // ---------------------------------------------------------------------------
            // Get a list of the LCDs with a specific tag
            // ---------------------------------------------------------------------------
            public List<IMyTerminalBlock> GetCTRLsWithTag(String tag)
            {
                List<IMyTerminalBlock> allCTRLs = new List<IMyTerminalBlock>();
                mypgm.GridTerminalSystem.GetBlocksOfType(allCTRLs, (IMyTerminalBlock x) => (
                                                                                       (x.CustomName != null) &&
                                                                                       (x.CustomName.ToUpper().IndexOf("[" + tag.ToUpper() + "]") >= 0) &&
                                                                                       (x is IMyShipController)
                                                                                      ));
                jdbg.Debug("Found " + allCTRLs.Count + " controllers with tag " + tag);
                return allCTRLs;
            }

            public bool IsOccupied(IMyShipController seat)
            {
                return seat.IsUnderControl;
            }

            public bool AnyKey(IMyShipController seat, bool allowJumpOrCrouch)
            {
                bool pressed = false;
                Vector3 dirn = seat.MoveIndicator;
                if (dirn.X != 0 || (allowJumpOrCrouch && dirn.Y != 0) || dirn.Z != 0) {
                    pressed = true;
                }
                return pressed;
            }

            public bool IsLeft(IMyShipController seat)
            {
                Vector3 dirn = seat.MoveIndicator;
                if (singleKey && dirn.X < 0 && dirn.Y == 0 && dirn.Z == 0) return true;
                else if (!singleKey && dirn.X < 0) return true;
                return false;
            }
            public bool IsRight(IMyShipController seat)
            {
                Vector3 dirn = seat.MoveIndicator;
                if (singleKey && dirn.X > 0 && dirn.Y == 0 && dirn.Z == 0) return true;
                else if (!singleKey && dirn.X > 0) return true;
                return false;
            }
            public bool IsUp(IMyShipController seat)
            {
                Vector3 dirn = seat.MoveIndicator;
                if (singleKey && dirn.X == 0 && dirn.Y == 0 && dirn.Z < 0) return true;
                else if (!singleKey && dirn.Z < 0) return true;
                return false;
            }
            public bool IsDown(IMyShipController seat)
            {
                Vector3 dirn = seat.MoveIndicator;
                if (singleKey && dirn.X == 0 && dirn.Y == 0 && dirn.Z > 0) return true;
                else if (!singleKey && dirn.Z > 0) return true;
                return false;
            }
            public bool IsSpace(IMyShipController seat)
            {
                Vector3 dirn = seat.MoveIndicator;
                if (singleKey && dirn.X == 0 && dirn.Y > 0 && dirn.Z == 0) return true;
                else if (!singleKey && dirn.Y > 0) return true;
                return false;
            }
            public bool IsCrouch(IMyShipController seat)
            {
                Vector3 dirn = seat.MoveIndicator;
                if (singleKey && dirn.X == 0 && dirn.Y < 0 && dirn.Z == 0) return true;
                else if (!singleKey && dirn.Y < 0) return true;
                return false;
            }
            public bool IsRollLeft(IMyShipController seat)
            {
                float dirn = seat.RollIndicator;
                if (dirn < 0.0) return true;
                return false;
            }
            public bool IsRollRight(IMyShipController seat)
            {
                float dirn = seat.RollIndicator;
                if (dirn > 0.0) return true;
                return false;
            }
            public bool IsArrowLeft(IMyShipController seat)
            {
                Vector2 dirn = seat.RotationIndicator;
                if (singleKey && dirn.X == 0 && dirn.Y < 0) return true;
                else if (!singleKey && dirn.Y < 0) return true;
                return false;
            }
            public bool IsArrowRight(IMyShipController seat)
            {
                Vector2 dirn = seat.RotationIndicator;
                if (singleKey && dirn.X == 0 && dirn.Y > 0) return true;
                else if (!singleKey && dirn.Y > 0) return true;
                return false;
            }
            public bool IsArrowDown(IMyShipController seat)
            {
                Vector2 dirn = seat.RotationIndicator;
                if (singleKey && dirn.X > 0 && dirn.Y == 0) return true;
                else if (!singleKey && dirn.X > 0) return true;
                return false;
            }
            public bool IsArrowUp(IMyShipController seat)
            {
                Vector2 dirn = seat.RotationIndicator;
                if (singleKey && dirn.X < 0 && dirn.Y == 0) return true;
                else if (!singleKey && dirn.X < 0) return true;
                return false;
            }
        }
    }
}
