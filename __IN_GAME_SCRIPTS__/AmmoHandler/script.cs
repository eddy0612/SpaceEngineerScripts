/*
 * R e a d m e
 * -----------
 * 
 * This script ensures specific cargo containers maintain a specific count of ammo, and queues up more to be
 * produced should the total run low. A status display can also be maintained if needed on an LCD.
 * 
 * Instructions
 * ============
 * Set the custom data for the programable block something like
 * 
 * [Config]
 * ; Update any LCD/cargo with Name [ammo]
 * tag=ammo
 * ; Update every 10 seconds
 * refreshSpeed=10
 * ; Bar length on the display
 * barSize=20
 * 
 * Then (assuming your tag is 'ammo', otherwise use the tag)
 * 
 * - add [ammo] to any LCD you want the status displayed on
 * - add [ammo] to the assembler you want the ammo to be produced on
 * - add [ammo] to any cargo container you regularly want a specific ammount of 
 *     ammo produced and moved into.
 * 
 * Once this is done, wait the refreshSpeed seconds, and the cargo containers custom data will be 
 * filled with a list of ammo types, and a count of how much you want defaulting to 0. Change the one(s)
 * you want to non-zero, and it will queue them up. Once produced they will be moved into the cargo container.
 * Note guns partially fill themselves automatically so the first chunk of produced ammo might be divereted
 * to the gun(s) in which case more will just be queued up.
 */


// ----------------------------- CUT -------------------------------------
String thisScript = "AmmoHandler";

// Development time flags
bool debug = false;
bool stayRunning = true;
bool actuallyQ = true;

// Private variables
private JDBG jdbg = null;
private JINV jinv = null;
private JLCD jlcd = null;

// My configuration - See Instructions.readme

Dictionary<String, String> CompToBlueprint = new Dictionary<String, String>
{
    { /*"Gatling",*/            "MyObjectBuilder_AmmoMagazine/NATO_25x184mm", "MyObjectBuilder_BlueprintDefinition/Position0080_NATO_25x184mmMagazine" },
    { /*"Autocannon",*/         "MyObjectBuilder_AmmoMagazine/AutocannonClip", "MyObjectBuilder_BlueprintDefinition/Position0090_AutocannonClip" },
    { /*"Rocket",*/             "MyObjectBuilder_AmmoMagazine/Missile200mm", "MyObjectBuilder_BlueprintDefinition/Position0100_Missile200mm" },
    { /*"Assault_Cannon",*/     "MyObjectBuilder_AmmoMagazine/MediumCalibreAmmo", "MyObjectBuilder_BlueprintDefinition/Position0110_MediumCalibreAmmo" },
    { /*"Artillery",*/          "MyObjectBuilder_AmmoMagazine/LargeCalibreAmmo", "MyObjectBuilder_BlueprintDefinition/Position0120_LargeCalibreAmmo" },
    { /*"Small_Railgun",*/      "MyObjectBuilder_AmmoMagazine/SmallRailgunAmmo", "MyObjectBuilder_BlueprintDefinition/Position0130_SmallRailgunAmmo" },
    { /*"Large_Railgun",*/      "MyObjectBuilder_AmmoMagazine/LargeRailgunAmmo", "MyObjectBuilder_BlueprintDefinition/Position0140_LargeRailgunAmmo" },
    // Commented out but put back in if needed
    //{ /*"S-10_Pistol",*/        "MyObjectBuilder_AmmoMagazine/SemiAutoPistolMagazine", "MyObjectBuilder_BlueprintDefinition/Position0010_SemiAutoPistolMagazine" },
    //{ /*"S-10E_Pistol",*/       "MyObjectBuilder_AmmoMagazine/ElitePistolMagazine", "MyObjectBuilder_BlueprintDefinition/Position0030_ElitePistolMagazine" },
    //{ /*"S-20A_Pistol",*/       "MyObjectBuilder_AmmoMagazine/FullAutoPistolMagazine" "MyObjectBuilder_BlueprintDefinition/Position0020_FullAutoPistolMagazine" },
    //{ /*"MR-20_Rifle",*/        "MyObjectBuilder_AmmoMagazine/AutomaticRifleGun_Mag_20rd", "MyObjectBuilder_BlueprintDefinition/Position0040_AutomaticRifleGun_Mag_20rd" },
    //{ /*"MR-30E_Rifle",*/       "MyObjectBuilder_AmmoMagazine/UltimateAutomaticRifleGun_Mag_30rd", "MyObjectBuilder_BlueprintDefinition/Position0070_UltimateAutomaticRifleGun_Mag_30rd" },
    //{ /*"MR-50A_Rifle",*/       "MyObjectBuilder_AmmoMagazine/RapidFireAutomaticRifleGun_Mag_50rd", "MyObjectBuilder_BlueprintDefinition/Position0050_RapidFireAutomaticRifleGun_Mag_50rd" },
    //{ /*"MR-8P_Rifle",*/        "MyObjectBuilder_AmmoMagazine/PreciseAutomaticRifleGun_Mag_5rd", "MyObjectBuilder_BlueprintDefinition/Position0060_PreciseAutomaticRifleGun_Mag_5rd" },
};
Dictionary<String, String> BlueprintToComp = new Dictionary<String, String>
{
    { /*"Gatling",*/            "MyObjectBuilder_BlueprintDefinition/Position0080_NATO_25x184mmMagazine", "MyObjectBuilder_AmmoMagazine/NATO_25x184mm" },
    { /*"Autocannon",*/         "MyObjectBuilder_BlueprintDefinition/Position0090_AutocannonClip", "MyObjectBuilder_AmmoMagazine/AutocannonClip" },
    { /*"Rocket",*/             "MyObjectBuilder_BlueprintDefinition/Position0100_Missile200mm", "MyObjectBuilder_AmmoMagazine/Missile200mm" },
    { /*"Assault_Cannon",*/     "MyObjectBuilder_BlueprintDefinition/Position0110_MediumCalibreAmmo", "MyObjectBuilder_AmmoMagazine/MediumCalibreAmmo" },
    { /*"Artillery",*/          "MyObjectBuilder_BlueprintDefinition/Position0120_LargeCalibreAmmo", "MyObjectBuilder_AmmoMagazine/LargeCalibreAmmo" },
    { /*"Small_Railgun",*/      "MyObjectBuilder_BlueprintDefinition/Position0130_SmallRailgunAmmo", "MyObjectBuilder_AmmoMagazine/SmallRailgunAmmo" },
    { /*"Large_Railgun",*/      "MyObjectBuilder_BlueprintDefinition/Position0140_LargeRailgunAmmo", "MyObjectBuilder_AmmoMagazine/LargeRailgunAmmo" },
    // Commented out but put back in if needed
    //{ /*"S-10_Pistol",*/        "MyObjectBuilder_BlueprintDefinition/Position0010_SemiAutoPistolMagazine", "MyObjectBuilder_AmmoMagazine/SemiAutoPistolMagazine" },
    //{ /*"S-10E_Pistol",*/       "MyObjectBuilder_BlueprintDefinition/Position0030_ElitePistolMagazine", "MyObjectBuilder_AmmoMagazine/ElitePistolMagazine" },
    //{ /*"S-20A_Pistol",*/       "MyObjectBuilder_BlueprintDefinition/Position0020_FullAutoPistolMagazine", "MyObjectBuilder_AmmoMagazine/FullAutoPistolMagazine" },
    //{ /*"MR-20_Rifle",*/        "MyObjectBuilder_BlueprintDefinition/Position0040_AutomaticRifleGun_Mag_20rd", "MyObjectBuilder_AmmoMagazine/AutomaticRifleGun_Mag_20rd" },
    //{ /*"MR-30E_Rifle",*/       "MyObjectBuilder_BlueprintDefinition/Position0070_UltimateAutomaticRifleGun_Mag_30rd", "MyObjectBuilder_AmmoMagazine/UltimateAutomaticRifleGun_Mag_30rd" },
    //{ /*"MR-50A_Rifle",*/       "MyObjectBuilder_BlueprintDefinition/Position0050_RapidFireAutomaticRifleGun_Mag_50rd", "MyObjectBuilder_AmmoMagazine/RapidFireAutomaticRifleGun_Mag_50rd" },
    //{ /*"MR-8P_Rifle",*/        "MyObjectBuilder_BlueprintDefinition/Position0060_PreciseAutomaticRifleGun_Mag_5rd", "MyObjectBuilder_AmmoMagazine/PreciseAutomaticRifleGun_Mag_5rd" },
};

// Mapping of all ammos we add to custom data to internal component name
Dictionary<String, String> friendlyitemname = new Dictionary<String, String>
{
    { "Gatling",            "MyObjectBuilder_AmmoMagazine/NATO_25x184mm" },
    { "Autocannon",         "MyObjectBuilder_AmmoMagazine/AutocannonClip" },
    { "Rocket",             "MyObjectBuilder_AmmoMagazine/Missile200mm" },
    { "Assault_Cannon",     "MyObjectBuilder_AmmoMagazine/MediumCalibreAmmo" },
    { "Artillery",          "MyObjectBuilder_AmmoMagazine/LargeCalibreAmmo" },
    { "Small_Railgun",      "MyObjectBuilder_AmmoMagazine/SmallRailgunAmmo" },
    { "Large_Railgun",      "MyObjectBuilder_AmmoMagazine/LargeRailgunAmmo" },
    // Commented out but put back in if needed
    //{ "S-10_Pistol",        "MyObjectBuilder_AmmoMagazine/SemiAutoPistolMagazine" },
    //{ "S-10E_Pistol",       "MyObjectBuilder_AmmoMagazine/ElitePistolMagazine" },
    //{ "S-20A_Pistol",       "MyObjectBuilder_AmmoMagazine/FullAutoPistolMagazine" },
    //{ "MR-20_Rifle",        "MyObjectBuilder_AmmoMagazine/AutomaticRifleGun_Mag_20rd" },
    //{ "MR-30E_Rifle",       "MyObjectBuilder_AmmoMagazine/UltimateAutomaticRifleGun_Mag_30rd" },
    //{ "MR-50A_Rifle",       "MyObjectBuilder_AmmoMagazine/RapidFireAutomaticRifleGun_Mag_50rd" },
    //{ "MR-8P_Rifle",        "MyObjectBuilder_AmmoMagazine/PreciseAutomaticRifleGun_Mag_5rd" },
};

Dictionary<String, char> solidcolor = new Dictionary<String, char>
{
    { "YELLOW", '' },
    { "RED", '' },
    { "ORANGE", '' },
    { "GREEN", '' },
    { "CYAN", '' },
    { "PURPLE", ''},
    { "BLUE", '' },
    { "WHITE", ''},
    { "BLACK", ''},
};


// My configuration
int refreshSpeed = 5;                       // Default to 5 seconds if not provided
int barSize = 15;
String mytag = "IDONTCARE";
String alerttag = "ALERT";

// Internals
DateTime lastCheck = new DateTime(0);

// ---------------------------------------------------------------------------
// Constructor
// ---------------------------------------------------------------------------
public Program()
{
    jdbg = new JDBG(this, debug);
    jlcd = new JLCD(this, jdbg, false);
    jinv = new JINV(jdbg);

    // Run every 100 ticks, but relies on internal check to only actually
    // perform on a defined frequency
    if (stayRunning)
    {
        Runtime.UpdateFrequency = UpdateFrequency.Update100;
    }
}

// ---------------------------------------------------------------------------
// Save
// ---------------------------------------------------------------------------
public void Save()
{
    // Nothing needed
}

// ---------------------------------------------------------------------------
// Mainline code called every update frequency
// ---------------------------------------------------------------------------
public void Main(string argument, UpdateType updateSource)
{
    try
    {
        // ---------------------------------------------------------------------------
        // Decide whether to actually do anything
        // ---------------------------------------------------------------------------
        if (stayRunning)
        {
            TimeSpan timeSinceLastCheck = DateTime.Now - lastCheck;
            if (timeSinceLastCheck.TotalSeconds >= refreshSpeed)
            {
                lastCheck = DateTime.Now;
            }
            else
            {
                return;
            }
        }

        // ---------------------------------------------------------------------------
        // We only get here if we are refreshing
        // ---------------------------------------------------------------------------
        jdbg.ClearDebugLCDs();  // Clear the debug screens

        jdbg.DebugAndEcho("Main Running..." + lastCheck.ToString());


        // ---------------------------------------------------------------------------
        // Get my custom data and parse to get the config
        // ---------------------------------------------------------------------------
        MyIniParseResult result;
        MyIni _ini = new MyIni();
        if (!_ini.TryParse(Me.CustomData, out result))
            throw new Exception(result.ToString());

        // Get the value of the "tag" key under the "config" section.
        mytag = _ini.Get("config", "tag").ToString();
        if (mytag != null)
        {
            mytag = (mytag.Split(';')[0]).Trim();
            Echo("Using tag of " + mytag);
        }
        else
        {
            Echo("No tag configured\nPlease add [config] for tag=<substring>");
            return;
        }

        // Get the value of the "refreshSpeed" key under the "config" section.
        int newrefreshSpeed = _ini.Get("config", "refreshSpeed").ToInt32();
        Echo("New refresh speed will be " + newrefreshSpeed);
        if (newrefreshSpeed < 1)
        {
            Echo("Invalid refresh speed or not defined - defaulting to 5 seconds");
            refreshSpeed = 5;
        }
        else
        {
            refreshSpeed = newrefreshSpeed;
        }

        // Get the value of the "refreshSpeed" key under the "config" section.
        int newBarSize = _ini.Get("config", "barSize").ToInt32();
        Echo("Bar Size will be " + newBarSize);
        if (newBarSize < 1)
        {
            Echo("Invalid bar size or not defined - defaulting to 25 seconds");
            barSize = 25;
        }
        else
        {
            barSize = newBarSize;
        }

        // ---------------------------------------------------------------------------
        // vvv                   Now do the actual work                           vvvv
        // ---------------------------------------------------------------------------
        // Logic:
        // Enumerate through all the cargos and work out whats needed
        // Enumerate through all the assemblers and
        //   - Add up any ammo queued
        //   - Distribute any in its inventory, move to a cargo that needs it
        // Queue up whats missing in batches
        // Build and display status on LCD

        // Work out where to queue things up to
        var assemblers = new List<IMyAssembler>();
        IMyAssembler qAssembler = null;
        GridTerminalSystem.GetBlocksOfType<IMyAssembler>(assemblers, (IMyAssembler x) => (
                                                                                            x.CustomName.IndexOf("[" + mytag + "]") >= 0
                                                                                         ));
        if (assemblers.Count == 0)
        {
            Echo("No assembler found with [" + mytag + "] in the name - guessing");
            GridTerminalSystem.GetBlocksOfType<IMyAssembler>(assemblers, (IMyAssembler x) => (
                                                                                                x.CanUseBlueprint(MyDefinitionId.Parse(CompToBlueprint[friendlyitemname["Gatling"]]))
                                                                                             ));
            if (assemblers.Count == 0)
            {
                Echo("ERROR: Guess failed... aborting");
                return;
            }
        }

        if (assemblers.Count > 1)
        {
            Echo("Multiple assemblers found with [" + mytag + "] in the name - using first");
        }
        qAssembler = assemblers[0];
        Echo("Will queue on " + qAssembler.CustomName);

        Dictionary<String, int> total_expected = new Dictionary<String, int>();
        Dictionary<String, int> total_existing = new Dictionary<String, int>();
        Dictionary<String, int> total_needed = new Dictionary<String, int>();
        foreach (var ammoName in friendlyitemname)
        {
            total_expected[ammoName.Key] = 0;
            total_existing[ammoName.Key] = 0;
            total_needed[ammoName.Key] = 0;
        }

        // Cargo containers to move to / populate
        List<IMyTerminalBlock> cargosToFill = new List<IMyTerminalBlock>();
        GridTerminalSystem.GetBlocksOfType(cargosToFill, (IMyTerminalBlock x) => (
                                                                                  (x.CustomName != null) &&
                                                                                  (x.CustomName.IndexOf("[" + mytag + "]") >= 0) &&
                                                                                  /* We are going to fill locked as well : (x.CustomName.IndexOf("[LOCKED]") < 0) && */
                                                                                  (x.HasInventory) &&
                                                                                  !(x is IMyAssembler)
                                                                                 ));
        Dictionary<String, Dictionary<IMyTerminalBlock, int>> needingAmmo = new Dictionary<String, Dictionary<IMyTerminalBlock, int>>();


        // Calculate current totals & move if necessary
        foreach (var thisblock in cargosToFill)
        {
            jdbg.Debug("Processing block: " + thisblock.CustomName);
            MyIni _blockini = new MyIni();
            if (!_blockini.TryParse(thisblock.CustomData, out result))
            {
                Echo("Block: " + thisblock.CustomName + " has invalid CustomData - ignoring");
                continue;
            }

            if (!_blockini.ContainsSection("ammo"))
            {
                Echo("Block: " + thisblock.CustomName + " has no ammo section... adding it");
                _blockini.AddSection("ammo");
                Echo("Now populating it");
                foreach (var ammoName in friendlyitemname)
                {
                    jdbg.Debug("Now populating it with " + ammoName.Key);
                    _blockini.Set("ammo", ammoName.Key, "0");
                }
                thisblock.CustomData = _blockini.ToString();
                // Nothing to do for this block now - Everything is zero
                continue;
            }
            else
            {
                jdbg.Debug("Block: " + thisblock.CustomName + " has an ammo section... ");
                foreach (var ammoName in friendlyitemname)
                {
                    int ammowanted = _blockini.Get("ammo", ammoName.Key).ToInt32(0);
                    total_expected[ammoName.Key] += ammowanted;

                    if (ammowanted > 0)
                    {
                        jdbg.Debug("We want " + ammowanted + " of " + ammoName.Key);

                        // Now see how many we have across the inventories
                        for (int invCount = 0; invCount < thisblock.InventoryCount; invCount++)
                        {
                            List<MyInventoryItem> allItemsInInventory = new List<MyInventoryItem>();
                            IMyInventory inv = thisblock.GetInventory(invCount);
                            inv.GetItems(allItemsInInventory, (MyInventoryItem x) => (
                                                                                    (x.Type.ToString().Equals(friendlyitemname[ammoName.Key]))
                                                                                     ));
                            int totalFound = 0;
                            foreach (var invitem in allItemsInInventory)
                            {
                                jdbg.Debug("Found: " + invitem.Type.ToString() + " count " + invitem.Amount.ToIntSafe());
                                totalFound += invitem.Amount.ToIntSafe();
                                total_existing[ammoName.Key] += invitem.Amount.ToIntSafe();
                            }
                            if (totalFound < ammowanted)
                            {
                                int needed = ammowanted - totalFound;
                                jdbg.Debug("Block " + thisblock.CustomName + " needs " + needed + " " + ammoName.Key);
                                total_needed[ammoName.Key] += needed;
                                if (!needingAmmo.ContainsKey(ammoName.Key))
                                {
                                    needingAmmo[ammoName.Key] = new Dictionary<IMyTerminalBlock, int>();
                                }
                                needingAmmo[ammoName.Key][thisblock] = needed;
                            }
                            else
                            {
                                jdbg.Debug("Block " + thisblock.CustomName + " has enough " + ammoName.Key + "(" + totalFound + ")");
                            }
                        }
                    }
                }
            }
        }

        // When we get here we know what ammo we need to discover... Now see if there's any in any of
        // the assemblers (Note we might q to one but coop might distribute the production) we can move
        // and queue up the remainder needed
        var allAssemblers = new List<IMyAssembler>();
        GridTerminalSystem.GetBlocksOfType<IMyAssembler>(allAssemblers, (IMyAssembler x) => (
                                                                                            x.CubeGrid.Equals(Me.CubeGrid)
                                                                                         ));
        int totalQueued = 0;
        foreach (var ammoName in friendlyitemname)
        {
            int toBuild = total_needed[ammoName.Key];
            if (toBuild > 0)
            {
                jdbg.Debug("We need to find " + toBuild + " of " + ammoName.Key);

                // Find any built ammo, and move it over
                foreach (var thisAssembler in allAssemblers)
                {
                    jdbg.Debug("Checking assembler: " + thisAssembler.CustomName);
                    List<MyInventoryItem> foundAmmo = new List<MyInventoryItem>();
                    // Cheat as assemblers only have one inventory for output for now
                    thisAssembler.GetInventory(1).GetItems(foundAmmo, (MyInventoryItem x) => (
                                                                                            (x.Type.ToString().Equals(friendlyitemname[ammoName.Key]))
                                                                                             ));
                    if (foundAmmo.Count > 0)
                    {
                        jdbg.Debug("Got " + foundAmmo.Count + " when looking for " + friendlyitemname[ammoName.Key]);
                        foreach (var thisAmmo in foundAmmo)
                        {
                            // Now redistribute them - start with the inventory needing the most
                            int ammoFound = thisAmmo.Amount.ToIntSafe();
                            jdbg.Debug("We have found some produced ammo to distribute count " + ammoFound);

                            // We need to build less
                            toBuild = toBuild - ammoFound;

                            Dictionary<IMyTerminalBlock, int> needingThisAmmo = null;
                            if (needingAmmo.ContainsKey(ammoName.Key)) needingThisAmmo = needingAmmo[ammoName.Key];

                            while ((ammoFound > 0) && (needingThisAmmo != null) && (needingThisAmmo.Count > 0))
                            {
                                foreach (var requirement in needingThisAmmo.OrderByDescending(p => p.Value))
                                {
                                    int moveTo = 0;

                                    // Can we satisty? Update the hash as we go
                                    if (ammoFound >= requirement.Value)
                                    {
                                        jdbg.Debug("We can satisfy " + requirement.Key.CustomName + " which needs " + requirement.Value);
                                        total_existing[ammoName.Key] += requirement.Value;
                                        moveTo = requirement.Value;
                                        needingThisAmmo.Remove(requirement.Key);
                                        if (needingThisAmmo.Count == 0)
                                        {
                                            needingThisAmmo = null;
                                        }
                                    }
                                    else
                                    {
                                        jdbg.Debug("We can only satisfy " + ammoFound + " of " + requirement.Key.CustomName + " which needs " + requirement.Value);
                                        total_existing[ammoName.Key] += ammoFound;
                                        moveTo = ammoFound;
                                        needingThisAmmo[requirement.Key] -= moveTo;
                                    }
                                    ammoFound -= moveTo;

                                    // Now move what we found
                                    if (moveTo > 0)
                                    {
                                        jdbg.Debug("Need to move this block! From " + thisAssembler.CustomName + "\n  to " + requirement.Key.CustomName + " " + moveTo + " of " + ammoName.Key);
                                        if (thisAssembler.GetInventory(1).TransferItemTo(requirement.Key.GetInventory(), thisAmmo, moveTo))
                                        {
                                            jdbg.Debug("Transferred ok");
                                        }
                                        else
                                        {
                                            jdbg.Debug("Failed to transfer?");
                                        }
                                    }

                                    // Stop if we have nothing left
                                    if (ammoFound == 0) break;
                                }
                            }
                        }
                    }

                    // Now handle queued ammo - we dont want to overqueue
                    var prodQ = new List<MyProductionItem>();
                    thisAssembler.GetQueue(prodQ);
                    //Debug("Got " + prodQ.Count + " queued up");
                    foreach (var thisQItem in prodQ)
                    {
                        if (thisQItem.BlueprintId.ToString().Equals(CompToBlueprint[friendlyitemname[ammoName.Key]]))
                        {
                            jdbg.Debug("... found " + thisQItem.Amount + " of " + ammoName.Key + " items");
                            toBuild = toBuild - thisQItem.Amount.ToIntSafe();
                        }
                    }
                }

                if (toBuild > 0)
                {
                    jdbg.Debug("Now we know how many to queue up: " + toBuild);
                    // Now queue up what else is needed
                    MyDefinitionId blueprint = MyDefinitionId.Parse(CompToBlueprint[friendlyitemname[ammoName.Key]]);
                    if (blueprint == null)
                    {
                        Echo("ERROR: Cannot create blueprint " + CompToBlueprint[friendlyitemname[ammoName.Key]]);
                        return;
                    }
                    MyFixedPoint mfp = toBuild;
                    if (actuallyQ) {
                        Echo("Queueing up " + toBuild + " of " + ammoName.Key);
                        qAssembler.AddQueueItem(blueprint, mfp);
                    } else {
                        Echo("DRYRUN: Not Queueing up " + toBuild + " of " + ammoName.Key);
                    }
                    totalQueued += toBuild;
                }
                else if (toBuild < 0)
                {
                    Echo("We have a surplus of " + ammoName.Key+ " - unexpected but ignorable");
                } else
                {
                    jdbg.Debug("We have the right number of " + ammoName.Key);
                }

            }
            else
            {
                jdbg.Debug("We have enough " + ammoName.Key);
            }
        }

        ClearDisplay();
        AddDisplay("Ammo Status at " + DateTime.Now.ToString());
        AddDisplay("===========");
        int totalexisting = 0;
        int totalexpected = 0;
        foreach (var ammoName in friendlyitemname)
        {
            totalexisting += total_existing[ammoName.Key];
            totalexpected += total_expected[ammoName.Key];

            if (total_expected[ammoName.Key] > 0)
            {
                AddDisplay(GetLine(ammoName.Key, total_existing[ammoName.Key], 0, total_expected[ammoName.Key]));
            }
        }
        DrawLCD(mytag, screen);

        int percentComplete = 0;
        percentComplete = (totalexisting * 100) / (totalexpected);

        jdbg.Alert("Completed - " + percentComplete + "% complete, queued: " + totalQueued, "GREEN", alerttag, thisScript);
    } catch (Exception ex)
    {
        jdbg.Alert("Exception - " + ex.ToString() + "\n" + ex.StackTrace, "RED", alerttag, thisScript);
    }
}

string GetLine(String name, int count, int queued, int wanted)
{
    String result = name.PadRight(20);

    //if (barUnder) result = result + "\n" + "".PadRight(2);
    result = result + ">"; // Could be [ or >

    String bar = "".PadRight(barSize);
    char[] barchars = bar.ToCharArray();

    // Set limits, as count or queued or both may take us over the limit
    if (count > wanted) count = wanted;
    if ((count + queued) > wanted) queued = (wanted - count);

    double squaresize = (double)wanted / (double)barSize;
    // Never round up completed, as 99/100 shouldnt show as a full bar
    int c_count = (int)Math.Floor((double)count / (double)squaresize);

    // We can round up the queued ones, but need to ensure we dont go over the max
    int q_count = (int)Math.Round((double)queued / (double)squaresize);
    if ((c_count + q_count) > barSize) q_count = q_count - 1;

    jdbg.Debug("getline: " + name + "," + count + "," + queued + "," + wanted + " = " + c_count + "/" + q_count);
    var bari = 0;
    for (int i = 0; i < c_count; i++)
    {
        barchars[bari] = solidcolor["CYAN"];
        bari++;
    }
    for (int i = 0; i < q_count; i++)
    {
        barchars[bari] = solidcolor["YELLOW"];
        bari++;
    }
    for (int i = bari; i < barSize; i++)
    {
        barchars[i] = solidcolor["RED"];
    }
    result = result + new string(barchars);
    result = result + "<"; // Could be ] or <
    return result;
}

// ---------------------------------------------------------------------------
// The following functions are for updating the screen
// ---------------------------------------------------------------------------
// Utility variables/function used when building the screen to display
int MAXCOLS = 80;
String screen = "";
void AddDisplay(String textToOutput)
{
    if (textToOutput.Length > MAXCOLS) MAXCOLS = textToOutput.Length;
    screen = screen + "\n" + textToOutput;
}

void ClearDisplay()
{
    screen = "";
    MAXCOLS = barSize + 10;
}

// ---------------------------------------------------------------------------
// Simple wrapper to completely redraw our LCD
// ---------------------------------------------------------------------------
void DrawLCD(String tag, String screenContents)
{
    jdbg.Debug("Drawing the screens");
    List<IMyTerminalBlock> drawLCDs = jlcd.GetLCDsWithTag(tag);
    jlcd.InitializeLCDs(drawLCDs, TextAlignment.LEFT);
    jlcd.SetupFont(drawLCDs, screenContents.Split('\n').Length + 1, MAXCOLS, true);
    jlcd.WriteToAllLCDs(drawLCDs, screenContents, false);
}

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

public class JDBG
{
    public bool debug = false;                          /* Are we logging dbg messages */

    private MyGridProgram mypgm = null;                 /* Main pgm if needed */
    private JLCD jlcd = null;                           /* JLCD class */

    private bool inDebug = false;                       /* Avoid recursion: */
    private static List<IMyTerminalBlock> debugLCDs = null;    /* LCDs to write to */

    // ---------------------------------------------------------
    // Constructor
    // ---------------------------------------------------------
    public JDBG(MyGridProgram pgm, bool debugState)
    {
        mypgm = pgm;
        debug = debugState;
        jlcd = new JLCD(pgm, this, false);
    }

    // ---------------------------------------------------------
    // Echo - write a message to the console
    // ---------------------------------------------------------
    public void Echo(string str)
    {
        mypgm.Echo("JDBG: " + str);
    }

    // ---------------------------------------------------------
    // Debug - write a message to the debug LCD
    // ---------------------------------------------------------
    public void Debug(String str)
    {
        Debug(str, true);
    }
    public void Debug(String str, bool consoledbg)
    {
        if (debug && !inDebug)
        {
            inDebug = true;

            if (debugLCDs == null)
            {
                Echo("First run - working out debug panels");
                initializeDBGLCDs();
                ClearDebugLCDs();
            }

            Echo("D:" + str);
            jlcd.WriteToAllLCDs(debugLCDs, str + "\n", true);
            inDebug = false;
        }
    }

    // ---------------------------------------------------------
    // Debug - write a message to the debug LCD
    // ---------------------------------------------------------
    public void DebugAndEcho(String str)
    {
        Echo(str);          // Will always come out
        Debug(str, false);  // Will only come out if debug is on
    }

    // ---------------------------------------------------------
    // InitializeLCDs - cache the debug LCD
    // ---------------------------------------------------------
    private void initializeDBGLCDs()
    {
        inDebug = true;
        debugLCDs = jlcd.GetLCDsWithTag("DEBUG");
        jlcd.InitializeLCDs(debugLCDs, TextAlignment.LEFT);
        inDebug = false;
    }

    // ---------------------------------------------------------
    // ClearDebugLCDs - clear the debug LCD
    // ---------------------------------------------------------
    public void ClearDebugLCDs()
    {
        if (debug) {
            if (debugLCDs == null) {
                Echo("First runC - working out debug panels");
                initializeDBGLCDs();
            }
            jlcd.WriteToAllLCDs(debugLCDs, "", false);  // Clear the debug screens
        }
    }

    // ---------------------------------------------------------------------------
    // Simple wrapper to write to a central alert - Needs work to support wrapping
    // ---------------------------------------------------------------------------
    public void Alert(String alertMsg, String colour, String alertTag, String thisScript)
    {
        List<IMyTerminalBlock> allBlocksWithLCDs = new List<IMyTerminalBlock>();
        mypgm.GridTerminalSystem.GetBlocksOfType(allBlocksWithLCDs, (IMyTerminalBlock x) => (
                                                                                  (x.CustomName != null) &&
                                                                                  (x.CustomName.IndexOf("[" + alertTag + "]") >= 0) &&
                                                                                  (x is IMyTextSurfaceProvider)
                                                                                 ));
        DebugAndEcho("Found " + allBlocksWithLCDs.Count + " lcds with '" + alertTag + "' to alert to");

        String alertOutput = JLCD.solidcolor[colour] + " " +
                             DateTime.Now.ToShortTimeString() + ":" +
                             thisScript + " " +
                             alertMsg + "\n";
        DebugAndEcho("ALERT: " + alertMsg);
        if (allBlocksWithLCDs.Count > 0)
        {
            jlcd.WriteToAllLCDs(allBlocksWithLCDs, alertOutput, true);
        }
    }

    // ---------------------------------------------------------------------------
    // Useful for complex scripts to see how far through the cpu slice you are
    // ---------------------------------------------------------------------------
    public void EchoCurrentInstructionCount(String tag)
    {
        Echo(tag + " instruction count: " + mypgm.Runtime.CurrentInstructionCount + "," + mypgm.Runtime.CurrentCallChainDepth);
    }
    public void EchoMaxInstructionCount()
    {
        Echo("Max instruction count: " + mypgm.Runtime.MaxInstructionCount + "," + mypgm.Runtime.MaxCallChainDepth);
    }
}

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
        //{ "MyObjectBuilder_Ore/Scrap", "MyObjectBuilder_Ingot/Scrap" },
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
        { "myobjectbuilder_component/bulletproofglass", "myobjectbuilder_blueprintdefinition/bulletproofglass"},
        { "myobjectbuilder_component/canvas", "myobjectbuilder_blueprintdefinition/position0030_canvas"},
        { "myobjectbuilder_component/computer", "myobjectbuilder_blueprintdefinition/computercomponent"},
        { "myobjectbuilder_component/construction", "myobjectbuilder_blueprintdefinition/constructioncomponent"},
        { "myobjectbuilder_component/detector", "myobjectbuilder_blueprintdefinition/detectorcomponent"},
        { "myobjectbuilder_component/display", "myobjectbuilder_blueprintdefinition/display"},
        { "myobjectbuilder_component/explosives", "myobjectbuilder_blueprintdefinition/explosivescomponent"},
        { "myobjectbuilder_component/girder", "myobjectbuilder_blueprintdefinition/girdercomponent"},
        { "myobjectbuilder_component/gravitygenerator", "myobjectbuilder_blueprintdefinition/gravitygeneratorcomponent"},
        { "myobjectbuilder_component/interiorplate", "myobjectbuilder_blueprintdefinition/interiorplate"},
        { "myobjectbuilder_component/largetube", "myobjectbuilder_blueprintdefinition/largetube"},
        { "myobjectbuilder_component/medical", "myobjectbuilder_blueprintdefinition/medicalcomponent"},
        { "myobjectbuilder_component/metalgrid", "myobjectbuilder_blueprintdefinition/metalgrid"},
        { "myobjectbuilder_component/motor", "myobjectbuilder_blueprintdefinition/motorcomponent"},
        { "myobjectbuilder_component/powercell", "myobjectbuilder_blueprintdefinition/powercell"},
        { "myobjectbuilder_component/reactor", "myobjectbuilder_blueprintdefinition/reactorcomponent"},
        { "myobjectbuilder_component/radiocommunication", "myobjectbuilder_blueprintdefinition/radiocommunicationcomponent"},
        { "myobjectbuilder_component/smalltube", "myobjectbuilder_blueprintdefinition/smalltube"},
        { "myobjectbuilder_component/solarcell", "myobjectbuilder_blueprintdefinition/solarcell"},
        { "myobjectbuilder_component/steelplate", "myobjectbuilder_blueprintdefinition/steelplate"},
        { "myobjectbuilder_component/superconductor", "myobjectbuilder_blueprintdefinition/superconductor"},
        { "myobjectbuilder_component/thrust", "myobjectbuilder_blueprintdefinition/thrustcomponent"},
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

public class JLCD
{
    public MyGridProgram mypgm = null;
    public JDBG jdbg = null;
    bool suppressDebug = false;

    // Useful for lookup via colour
    public static Dictionary<String, char> solidcolor = new Dictionary<String, char>
    {
        { "YELLOW", '' },
        { "RED", '' },
        { "ORANGE", '' },
        { "GREEN", '' },
        { "CYAN", '' },
        { "PURPLE", ''},
        { "BLUE", '' },
        { "WHITE", ''},
        { "BLACK", ''},
        { "GREY", '' }
    };

    // Useful for direct code
    public static char COLOUR_YELLOW = '';
    public static char COLOUR_RED = '';
    public static char COLOUR_ORANGE = '';
    public static char COLOUR_GREEN = '';
    public static char COLOUR_CYAN = '';
    public static char COLOUR_PURPLE = '';
    public static char COLOUR_BLUE = '';
    public static char COLOUR_WHITE = '';
    public static char COLOUR_BLACK = '';
    public static char COLOUR_GREY = '';

    public JLCD(MyGridProgram pgm, JDBG dbg, bool suppressDebug)
    {
        this.mypgm = pgm;
        this.jdbg = dbg;
        this.suppressDebug = suppressDebug; /* Suppress the 'Writing to...' msg */
    }

    // ---------------------------------------------------------------------------
    // Get a list of the LCDs with a specific tag
    // ---------------------------------------------------------------------------
    public List<IMyTerminalBlock> GetLCDsWithTag(String tag)
    {
        List<IMyTerminalBlock> allLCDs = new List<IMyTerminalBlock>();
        mypgm.GridTerminalSystem.GetBlocksOfType(allLCDs, (IMyTerminalBlock x) => (
                                                                               (x.CustomName != null) &&
                                                                               (x.CustomName.ToUpper().IndexOf("[" + tag.ToUpper() + "]") >= 0) &&
                                                                               (x is IMyTextSurfaceProvider)
                                                                              ));
        jdbg.Debug("Found " + allLCDs.Count + " lcds to update with tag " + tag);
        return allLCDs;
    }

    // ---------------------------------------------------------------------------
    // Initialize LCDs
    // ---------------------------------------------------------------------------
    public void InitializeLCDs(List<IMyTerminalBlock> allLCDs, TextAlignment align)
    {
        foreach (var thisLCD in allLCDs)
        {
            jdbg.Debug("Setting up the font for " + thisLCD.CustomName);

            // Set it to text / monospace, black background
            // Also set the padding
            IMyTextSurface thisSurface = ((IMyTextSurfaceProvider)thisLCD).GetSurface(0);
            if (thisSurface == null) continue;
            thisSurface.Font = "Monospace";
            thisSurface.ContentType = ContentType.TEXT_AND_IMAGE;
            thisSurface.BackgroundColor = Color.Black;
            thisSurface.Alignment = align;
        }
    }

    public void SetLCDFontColour(List<IMyTerminalBlock> allLCDs, Color colour)
    {
        foreach (var thisLCD in allLCDs) {
            if (thisLCD is IMyTextPanel) {
                jdbg.Debug("Setting up the color for " + thisLCD.CustomName);
                ((IMyTextPanel)thisLCD).FontColor = colour;
            }
        }
    }

    public void SetLCDRotation(List<IMyTerminalBlock> allLCDs, float Rotation)
    {
        foreach (var thisLCD in allLCDs) {
            if (thisLCD is IMyTextPanel) {
                jdbg.Debug("Setting up the rotation for " + thisLCD.CustomName);
                thisLCD.SetValueFloat("Rotate", Rotation);
            }
        }
    }

    // ---------------------------------------------------------------------------
    // Set the font and display properties of each display UNLESS [LOCKED]
    // ---------------------------------------------------------------------------
    public void SetupFont(List<IMyTerminalBlock> allLCDs, int rows, int cols, bool mostlySpecial)
    {
        SetupFontCalc(allLCDs, rows, cols, mostlySpecial, 0.05F, 0.05F);
    }
    public void SetupFontCustom(List<IMyTerminalBlock> allLCDs, int rows, int cols, bool mostlySpecial, float size, float incr)
    {
        SetupFontCalc(allLCDs, rows, cols, mostlySpecial, size,incr);
    }

    public void SetupFontCalc(List<IMyTerminalBlock> allLCDs, int rows, int cols, bool mostlySpecial, float startSize, float startIncr)
    {
        foreach (var thisLCD in allLCDs)
        {
            jdbg.Debug("Setting up font on screen: " + thisLCD.CustomName + " (" + rows + " x " + cols + ")");
            IMyTextSurface thisSurface = ((IMyTextSurfaceProvider)thisLCD).GetSurface(0);
            if (thisSurface == null) continue;

            // Now work out the font size
            float size = startSize;
            float incr = startIncr;

            // Get a list of cols strings
            StringBuilder teststr = new StringBuilder("".PadRight(cols, (mostlySpecial? solidcolor["BLACK"] : 'W')));
            Vector2 actualScreenSize = thisSurface.TextureSize;
            while (true)
            {
                thisSurface.FontSize = size;
                Vector2 actualSize = thisSurface.TextureSize;

                Vector2 thisSize = thisSurface.MeasureStringInPixels(teststr, thisSurface.Font, size);
                //jdbg.Debug("..with size " + size + " width is " + thisSize.X + " max " + actualSize.X);
                //jdbg.Debug("..with size " + size + " height is " + thisSize.Y + " max " + actualSize.Y);

                int displayrows = (int)Math.Floor(actualScreenSize.Y / thisSize.Y);

                if ((thisSize.X < actualSize.X) && (displayrows > rows))
                {
                    size += incr;
                }
                else
                {
                    break;
                }
            }
            thisSurface.FontSize = size - incr;
            jdbg.Debug("Calc size of " + thisSurface.FontSize);

            // BUG? Corner LCDs are a factor of 4 out - no idea why but try *4
            if (thisLCD.DefinitionDisplayNameText.Contains("Corner LCD")) {
                jdbg.Debug("INFO: Avoiding bug, multiplying by 4: " + thisLCD.DefinitionDisplayNameText);
                thisSurface.FontSize *= 4;
            }
        }
    }

    // ---------------------------------------------------------------------------
    // Write a message to all related LCDs
    // ---------------------------------------------------------------------------
    public void WriteToAllLCDs(List<IMyTerminalBlock> allLCDs, String msg, bool append)
    {
        //if (allLCDs == null) {
            //jdbg.Debug("WARNING: No screen to write to");
            //return;
        //}
        foreach (var thisLCD in allLCDs)
        {
            if (!this.suppressDebug) jdbg.Debug("Writing to display " + thisLCD.CustomName);
            IMyTextSurface thisSurface = ((IMyTextSurfaceProvider)thisLCD).GetSurface(0);
            if (thisSurface == null) continue;
            thisSurface.WriteText(msg, append);
        }
    }

    // ---------------------------------------------------------------------------
    // Convert r g b to a character - Credit
    //    to https://github.com/Whiplash141/Whips-Image-Converter
    // ---------------------------------------------------------------------------
    public char ColorToChar(byte r, byte g, byte b)
    {
        const double bitSpacing = 255.0 / 7.0;
        return (char)(0xe100 + ((int)Math.Round(r / bitSpacing) << 6) + ((int)Math.Round(g / bitSpacing) << 3) + (int)Math.Round(b / bitSpacing));
    }

}