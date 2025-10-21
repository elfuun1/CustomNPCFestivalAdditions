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
using System.Diagnostics.Tracing;
using CustomNPCFestivalAdditions.ModData;
using System.Text.Json.Serialization;

namespace CustomNPCFestivalAdditions
{
    internal sealed class ModEntry : Mod
    {
        public static ModConfig Config;
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.GameLaunched += onLaunched;
            helper.Events.GameLoop.SaveLoaded += SaveLoaded;

            Config = helper.ReadConfig<ModConfig>();
            Initialize.InitializeAll(Monitor, Helper, Config);

            bool Kelly2892FlowerDancingLoaded = this.Helper.ModRegistry.IsLoaded("kelly2892.flower.dancing.harmony");
            if (this.Helper.ModRegistry.IsLoaded("kelly2892.flower.dancing.harmony") == true)
            {
                Monitor.Log($"Detected mod \"Flower Dancing\" by kelly2892. Ensuring this mod applies Spring 24 postfix after any CNFA harmony patches to ensure compatibility.");
            }

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

            //Load CP data
            //helper.Events.Content.AssetRequested += OnAssetRequested;

            /*
            //Load custom console commands
            helper.ConsoleCommands.Add
                (
                "CNFA_TestSpring24", "Runs CNFA Harmony postfix and then plays Spring 24 main event.",
                (commands, args) => this.CNFA_TestSpring24(commands, args.AsSpan())
                );
            */

        }

        private void onLaunched(object sender, GameLaunchedEventArgs e)
        {
            //Access Content Patcher API
            var api = this.Helper.ModRegistry.GetApi<IContentPatcherAPI>("Pathoschild.ContentPatcher");

            //Access GMCM API
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
                return;

        }

        /*public void OnAssetRequested(object? sender, AssetRequestedEventArgs e)
        {
            if (Game1.CurrentEvent.isFestival && e.NameWithoutLocale.ToString().EndsWith("_CNFASpring24"))
            {
                string character = e.NameWithoutLocale.ToString().Split("/")[^1];
                e.LoadFrom(() => , AssetLoadPriority.Medium);
            }
            if (e.NameWithoutLocale.IsEquivalentTo("Characters"))
            {
                e.LoadFrom(() => new Dictionary<string,ModData.Spring24SpriteModel>(), AssetLoadPriority.Exclusive);
                
            }
        }*/

        /*private void CNFA_TestSpring24(string command, string[] args)
        {
            if (!Context.IsWorldReady)
            {
                Monitor.Log("World is not ready- please load world.", LogLevel.Warn);
            }
            else 
            {
                
            }
        }*/


        public void SaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            /*
            Spring24PairWhitelist entry1 = new Spring24PairWhitelist("Manual", "Abigail", "Sebastian", true);
            Fields fields1 = new Fields(entry1);
            RawContent content1 = new RawContent(fields1);
            RawCNFAContentPack testPack = new RawCNFAContentPack(content1);

            this.Helper.Data.WriteJsonFile<RawCNFAContentPack>("testdata.json", testPack);
            */
            

            foreach (IContentPack contentPack in this.Helper.ContentPacks.GetOwned())
            {
                Monitor.Log($"Reading CNFA content pack {contentPack.Manifest.Name}, version {contentPack.Manifest.Version} by {contentPack.Manifest.Author}.", LogLevel.Debug);

                if (!contentPack.HasFile("content.json"))
                {
                    Monitor.Log($"CNFA content pack {contentPack.Manifest.Name}, version {contentPack.Manifest.Version} by {contentPack.Manifest.Author} is missing a \"content.json\" file. Content pack will be skipped.", LogLevel.Warn);
                    break;
                }
                else if (contentPack.HasFile("content.json"))
                {
                    RawCNFAContentPack contentPackRawData = contentPack.ReadJsonFile<RawCNFAContentPack>("content.json");
                    if (contentPackRawData == null)
                    { break; }

                    int contentIndex = 1;
                    foreach (ModData.RawContent rawContent in contentPackRawData.Entries)
                    {
                        Monitor.Log($"Attempting to load entry {contentIndex}, content of {rawContent.ContentType} type.", LogLevel.Trace);
                        switch (rawContent.ContentType)
                        {
                            case "Spring24PairWhitelist":

                                try
                                {
                                    Spring24PairWhitelist pairWhiteList = new Spring24PairWhitelist(rawContent);
                                    this.Helper.Data.WriteSaveData(pairWhiteList.ContentID, pairWhiteList);

                                    Spring24PairWhitelist retrieved = this.Helper.Data.ReadSaveData<Spring24PairWhitelist>($"{pairWhiteList.ContentID}");
                                    Monitor.Log($"Whitelisted pair of {retrieved.LowerDancerName} + {retrieved.UpperDancerName} (position strict = {retrieved.IsPositionStrict}) retrieved from {pairWhiteList.SourceContentPack.Manifest.Name}.", LogLevel.Warn);

                                }
                                catch
                                {
                                    Monitor.Log($"Could not load and save contents of content index {contentIndex}. Content will be skipped.");
                                }

                                break;
                        }
                        contentIndex++;
                    }
                }
            }
        }
    }
}

