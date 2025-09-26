using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using HarmonyLib;
using CustomNPCFestivalAdditions.InterfaceManagers;

namespace CustomNPCFestivalAdditions
{
    internal sealed class ModEntry:Mod
    {
        public static ModConfig Config;
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.GameLaunched += onLaunched;

            Config = helper.ReadConfig<ModConfig>();
            Initialize.InitializeAll(Monitor, Helper, Config);

            /*
            //Access Content Patcher API
            var api = this.Helper.ModRegistry.GetApi<IContentPatcherAPI>("Pathoschild.ContentPatcher");

            //Access GMCM API
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
                return;

            //Register CNFA in GMCM
            configMenu.Register(
                mod: this.ModManifest,
                reset: () => this.Config = new ModConfig(),
                save: () => this.Helper.WriteConfig(this.Config)
);

            */

            //Initialize Harmony patches
            try
            {
                Harmony harmony = new(ModManifest.UniqueID);
                harmony.Patch
                (
                    original: AccessTools.Method(typeof(StardewValley.Event), nameof(StardewValley.Event.setUpFestivalMainEvent)),
                    postfix: new HarmonyMethod(typeof(EventPatched), nameof(EventPatched.setUpFestivalMainEvent_CNFA))
                );

                Monitor.Log("Custom NPC Festival Additions has finished applying postfix patch using Harmony.", LogLevel.Trace);
            }
            catch (Exception e)
            {
                Monitor.Log("Custom NPC Festival Additions has encountered a Harmony error:" + e, LogLevel.Warn);
            }


        }

        private void onLaunched(object sender, GameLaunchedEventArgs e)
        {
            Monitor.Log("CustomNPCFestivalAdditions has loaded and accessed the log.", LogLevel.Trace);
        }
    }
}
