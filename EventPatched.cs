using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Network;
using StardewValley.Objects;
using HarmonyLib;
using System.Runtime.CompilerServices;
using System.Numerics;
using System.Security.AccessControl;
using CustomNPCFestivalAdditions.Spring24;
using System.Text.RegularExpressions;
using System.ComponentModel.Design;

namespace CustomNPCFestivalAdditions
{
    internal class EventPatched
    {
        public static IMonitor Monitor;
        public static IModHelper Helper;
        public static ModConfig Config;
        public static Harmony Harmony;

        [HarmonyBefore("setUpFestivalMainEvent_Kelly")]

        public static void Initialize(IMonitor monitor, ModConfig config)
        {
            Monitor = monitor;
            Config = config;
        }
        public static void setUpFestivalMainEvent_CNFA(StardewValley.Event __instance)
        {
            if (Monitor is null || __instance is null || !__instance.isFestival)
            {
                return;
            }

            //scope creep switch for whenever I decide to not be busy
            switch (__instance.id)
            {
                case "festival_spring24":
                    if (Config.EnableCNFAspring24 == true)
                    {
                        Monitor.Log("Initiating custom flower dance patch via Harmony.", LogLevel.Trace);
                        FlowerDancePatch.FlowerDanceSetUp(__instance);
                    }
                    else
                    { 
                        Monitor.Log("Custom flower dance is currently disabled, per mod configurations.", LogLevel.Debug);
                    }
                    break;
                default:
                    Monitor.Log($"Festival ID {__instance.id} is either not yet supported by or not compatable with CNFA. Reverting to vanilla code.", LogLevel.Trace);
                    break; 

            }
        }
       
    }
 }
            
