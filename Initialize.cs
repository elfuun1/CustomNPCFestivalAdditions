using CustomNPCFestivalAdditions.InterfaceManagers;
using CustomNPCFestivalAdditions.Spring24;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFestivalAdditions
{
    internal class Initialize
    {
        public static IMonitor Monitor;
        public static IModHelper Helper;
        public static ModConfig Config;

        public static void InitializeAll(IMonitor monitor,IModHelper helper, ModConfig config)
        {
            EventPatched.Initialize(monitor, config);
            Utilities.Initialize(monitor, helper, config);
            CNFAConsoleCommands.Initialize(helper);

            ModData.ModDataSpring24.Initialize(monitor, config, helper);
            
            Spring24.DanceGenerator.Initialize(monitor, helper);
            Spring24.FlowerDancePatch.Initialize(monitor, helper, config);
            
        }
    }
}
