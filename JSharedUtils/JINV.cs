using Sandbox.Game.EntityComponents;
using Sandbox.Game.Screens.Helpers;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        public class JINV
        {
            private JDBG jdbg = null;

            public enum BLUEPRINT_TYPES { BOTTLES, COMPONENTS, AMMO, TOOLS, OTHER, ORES };

            /* Ore to Ingot */
            Dictionary<String, String> oreToIngot = new Dictionary<String, String>
            {
                { "MyObjectBuilder_Ore/Cobalt", "MyObjectBuilder_Ingot/Cobalt" },
                { "MyObjectBuilder_Ore/Gold", "MyObjectBuilder_Ingot/Gold" },
                { "MyObjectBuilder_Ore/Stone", "MyObjectBuilder_Ingot/Stone" },
                { "MyObjectBuilder_Ore/Iron", "MyObjectBuilder_Ingot/Iron" },
                { "MyObjectBuilder_Ore/Magnesium", "MyObjectBuilder_Ingot/Magnesium" },
                { "MyObjectBuilder_Ore/Nickel", "MyObjectBuilder_Ingot/Nickel" },
                { "MyObjectBuilder_Ore/Platinum", "MyObjectBuilder_Ingot/Platinum" },
                { "MyObjectBuilder_Ore/Silicon", "MyObjectBuilder_Ingot/Silicon" },
                { "MyObjectBuilder_Ore/Silver", "MyObjectBuilder_Ingot/Silver" },
                { "MyObjectBuilder_Ore/Uranium", "MyObjectBuilder_Ingot/Uranium" },

                // Not needing these at present:
                { "MyObjectBuilder_Ore/Scrap", "MyObjectBuilder_Ingot/Scrap" },
                //{ "MyObjectBuilder_Ore/Ice", "?" },
                //{ "MyObjectBuilder_Ore/Organic", "?" },
            };

            /* Other stuff */
            Dictionary<String, String> otherCompToBlueprint = new Dictionary<String, String>
            {
                { "MyObjectBuilder_BlueprintDefinition/Position0040_Datapad", "MyObjectBuilder_Datapad/Datapad" },
            };

            /* Tools */
            Dictionary<String, String> toolsCompToBlueprint = new Dictionary<String, String>
            {
                { "MyObjectBuilder_PhysicalGunObject/AngleGrinderItem" , "MyObjectBuilder_BlueprintDefinition/Position0010_AngleGrinder" },
                { "MyObjectBuilder_PhysicalGunObject/AngleGrinder2Item", "MyObjectBuilder_BlueprintDefinition/Position0020_AngleGrinder2" },
                { "MyObjectBuilder_PhysicalGunObject/AngleGrinder3Item" , "MyObjectBuilder_BlueprintDefinition/Position0030_AngleGrinder3" },
                { "MyObjectBuilder_PhysicalGunObject/AngleGrinder4Item" , "MyObjectBuilder_BlueprintDefinition/Position0040_AngleGrinder4" },

                { "MyObjectBuilder_PhysicalGunObject/WelderItem" , "MyObjectBuilder_BlueprintDefinition/Position0090_Welder" },
                { "MyObjectBuilder_PhysicalGunObject/Welder2Item" , "MyObjectBuilder_BlueprintDefinition/Position0100_Welder2" },
                { "MyObjectBuilder_PhysicalGunObject/Welder3Item" , "MyObjectBuilder_BlueprintDefinition/Position0110_Welder3" },
                { "MyObjectBuilder_PhysicalGunObject/Welder4Item" , "MyObjectBuilder_BlueprintDefinition/Position0120_Welder4" },

                { "MyObjectBuilder_PhysicalGunObject/HandDrillItem", "MyObjectBuilder_BlueprintDefinition/Position0050_HandDrill" },
                { "MyObjectBuilder_PhysicalGunObject/HandDrill2Item", "MyObjectBuilder_BlueprintDefinition/Position0060_HandDrill2" },
                { "MyObjectBuilder_PhysicalGunObject/HandDrill3Item" , "MyObjectBuilder_BlueprintDefinition/Position0070_HandDrill3" },
                { "MyObjectBuilder_PhysicalGunObject/HandDrill4Item" , "MyObjectBuilder_BlueprintDefinition/Position0080_HandDrill4" },

            };

            /* Bottles */
            Dictionary<String, String> bottlesCompToBlueprint = new Dictionary<String, String>
            {
                { "MyObjectBuilder_GasContainerObject/HydrogenBottle", "MyObjectBuilder_BlueprintDefinition/Position0020_HydrogenBottle" },
                { "MyObjectBuilder_OxygenContainerObject/OxygenBottle", "MyObjectBuilder_BlueprintDefinition/HydrogenBottlesRefill" },
            };

            /* Components */
            Dictionary<String, String> componentsCompToBlueprint = new Dictionary<String, String>
            {
                { "MyObjectBuilder_Component/BulletproofGlass", "MyObjectBuilder_BlueprintDefinition/BulletproofGlass"},
                { "MyObjectBuilder_Component/Canvas", "MyObjectBuilder_BlueprintDefinition/Position0030_Canvas"},
                { "MyObjectBuilder_Component/Computer", "MyObjectBuilder_BlueprintDefinition/ComputerComponent"},
                { "MyObjectBuilder_Component/Construction", "MyObjectBuilder_BlueprintDefinition/ConstructionComponent"},
                { "MyObjectBuilder_Component/Detector", "MyObjectBuilder_BlueprintDefinition/DetectorComponent"},
                { "MyObjectBuilder_Component/Display", "MyObjectBuilder_BlueprintDefinition/Display"},
                { "MyObjectBuilder_Component/Explosives", "MyObjectBuilder_BlueprintDefinition/ExplosivesComponent"},
                { "MyObjectBuilder_Component/Girder", "MyObjectBuilder_BlueprintDefinition/GirderComponent"},
                { "MyObjectBuilder_Component/GravityGenerator", "MyObjectBuilder_BlueprintDefinition/GravityGeneratorComponent"},
                { "MyObjectBuilder_Component/InteriorPlate", "MyObjectBuilder_BlueprintDefinition/InteriorPlate"},
                { "MyObjectBuilder_Component/LargeTube", "MyObjectBuilder_BlueprintDefinition/LargeTube"},
                { "MyObjectBuilder_Component/Medical", "MyObjectBuilder_BlueprintDefinition/MedicalComponent"},
                { "MyObjectBuilder_Component/MetalGrid", "MyObjectBuilder_BlueprintDefinition/MetalGrid"},
                { "MyObjectBuilder_Component/Motor", "MyObjectBuilder_BlueprintDefinition/MotorComponent"},
                { "MyObjectBuilder_Component/PowerCell", "MyObjectBuilder_BlueprintDefinition/PowerCell"},
                { "MyObjectBuilder_Component/Reactor", "MyObjectBuilder_BlueprintDefinition/ReactorComponent"},
                { "MyObjectBuilder_Component/RadioCommunication", "MyObjectBuilder_BlueprintDefinition/RadioCommunicationComponent"},
                { "MyObjectBuilder_Component/SmallTube", "MyObjectBuilder_BlueprintDefinition/SmallTube"},
                { "MyObjectBuilder_Component/SolarCell", "MyObjectBuilder_BlueprintDefinition/SolarCell"},
                { "MyObjectBuilder_Component/SteelPlate", "MyObjectBuilder_BlueprintDefinition/SteelPlate"},
                { "MyObjectBuilder_Component/Superconductor", "MyObjectBuilder_BlueprintDefinition/Superconductor"},
                { "MyObjectBuilder_Component/Thrust", "MyObjectBuilder_BlueprintDefinition/ThrustComponent"},
            };

            /* Ammo */
            Dictionary<String, String> ammoCompToBlueprint = new Dictionary<String, String>
            {
                /* Ammo */
                { /*"Gatling",*/            "MyObjectBuilder_AmmoMagazine/NATO_25x184mm", "MyObjectBuilder_BlueprintDefinition/Position0080_NATO_25x184mmMagazine" },
                { /*"Autocannon",*/         "MyObjectBuilder_AmmoMagazine/AutocannonClip", "MyObjectBuilder_BlueprintDefinition/Position0090_AutocannonClip" },
                { /*"Rocket",*/             "MyObjectBuilder_AmmoMagazine/Missile200mm", "MyObjectBuilder_BlueprintDefinition/Position0100_Missile200mm" },
                { /*"Assault_Cannon",*/     "MyObjectBuilder_AmmoMagazine/MediumCalibreAmmo", "MyObjectBuilder_BlueprintDefinition/Position0110_MediumCalibreAmmo" },
                { /*"Artillery",*/          "MyObjectBuilder_AmmoMagazine/LargeCalibreAmmo", "MyObjectBuilder_BlueprintDefinition/Position0120_LargeCalibreAmmo" },
                { /*"Small_Railgun",*/      "MyObjectBuilder_AmmoMagazine/SmallRailgunAmmo", "MyObjectBuilder_BlueprintDefinition/Position0130_SmallRailgunAmmo" },
                { /*"Large_Railgun",*/      "MyObjectBuilder_AmmoMagazine/LargeRailgunAmmo", "MyObjectBuilder_BlueprintDefinition/Position0140_LargeRailgunAmmo" },
                { /*"S-10_Pistol",*/        "MyObjectBuilder_AmmoMagazine/SemiAutoPistolMagazine", "MyObjectBuilder_BlueprintDefinition/Position0010_SemiAutoPistolMagazine" },
                { /*"S-10E_Pistol",*/       "MyObjectBuilder_AmmoMagazine/ElitePistolMagazine", "MyObjectBuilder_BlueprintDefinition/Position0030_ElitePistolMagazine" },
                { /*"S-20A_Pistol",*/       "MyObjectBuilder_AmmoMagazine/FullAutoPistolMagazine", "MyObjectBuilder_BlueprintDefinition/Position0020_FullAutoPistolMagazine" },
                { /*"MR-20_Rifle",*/        "MyObjectBuilder_AmmoMagazine/AutomaticRifleGun_Mag_20rd", "MyObjectBuilder_BlueprintDefinition/Position0040_AutomaticRifleGun_Mag_20rd" },
                { /*"MR-30E_Rifle",*/       "MyObjectBuilder_AmmoMagazine/UltimateAutomaticRifleGun_Mag_30rd", "MyObjectBuilder_BlueprintDefinition/Position0070_UltimateAutomaticRifleGun_Mag_30rd" },
                { /*"MR-50A_Rifle",*/       "MyObjectBuilder_AmmoMagazine/RapidFireAutomaticRifleGun_Mag_50rd", "MyObjectBuilder_BlueprintDefinition/Position0050_RapidFireAutomaticRifleGun_Mag_50rd" },
                { /*"MR-8P_Rifle",*/        "MyObjectBuilder_AmmoMagazine/PreciseAutomaticRifleGun_Mag_5rd", "MyObjectBuilder_BlueprintDefinition/Position0060_PreciseAutomaticRifleGun_Mag_5rd" },
                { /*"5.56x45mm NATO magazine",*/ "MyObjectBuilder_AmmoMagazine/NATO_5p56x45mm", null /* Not Craftable */ },
            };
            public JINV(JDBG dbg)
            {
                jdbg = dbg;
            }
            public void addBluePrints(BLUEPRINT_TYPES types, ref Dictionary<String, String> into)
            {
                switch (types)
                {
                    case BLUEPRINT_TYPES.BOTTLES:
                        into = into.Concat(bottlesCompToBlueprint).ToDictionary(x => x.Key, x => x.Value);
                        break;
                    case BLUEPRINT_TYPES.COMPONENTS:
                        into = into.Concat(componentsCompToBlueprint).ToDictionary(x => x.Key, x => x.Value); 
                        break;
                    case BLUEPRINT_TYPES.AMMO:
                        into = into.Concat(ammoCompToBlueprint).ToDictionary(x => x.Key, x => x.Value);
                        break;
                    case BLUEPRINT_TYPES.TOOLS:
                        into = into.Concat(toolsCompToBlueprint).ToDictionary(x => x.Key, x => x.Value);
                        break;
                    case BLUEPRINT_TYPES.OTHER:
                        into = into.Concat(otherCompToBlueprint).ToDictionary(x => x.Key, x => x.Value);
                        break;
                    case BLUEPRINT_TYPES.ORES:
                        into = into.Concat(oreToIngot).ToDictionary(x => x.Key, x => x.Value);
                        break;
                }
            }
        }
    }
}
