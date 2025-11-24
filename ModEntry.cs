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
using System.Reflection.Metadata;
using StardewValley.GameData.Characters;

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
                Monitor.Log($"Detected mod \"Flower Dancing\" by kelly2892. Ensuring this mod applies Flower Dancing's Spring 24 postfix after any CNFA harmony patches to ensure compatibility.");
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
            helper.ConsoleCommands.Add
                (
                "CNFA_PrintLoadedContent", "Prints a list of loaded content to the console by content ID, based on inputed content type. If not content type is listed, defaults to printing all loaded content.",
                CNFAConsoleCommands.CNFA_PrintLoadedContent);
            helper.ConsoleCommands.Add
                (
                "CNFA_EnableContent", "Enables content, selected by content ID.",
                CNFAConsoleCommands.CNFA_EnableContent);
            helper.ConsoleCommands.Add
                (
                "CNFA_DisableContent", "Disables content, selected by content ID.",
                CNFAConsoleCommands.CNFA_DisableContent);

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
        public void SaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            
            //Retrieve any saved mod data and/or declare empty arrays for any empty datasets
            //Spring24 data
            var spring24pairwhitelist = this.Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24PairWhitelist>("spring24pairwhitelist") 
                ?? new Models.Spring24.ModData_Spring24PairWhitelist();
            var spring24pairblacklist = this.Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24PairBlacklist>("spring24pairblacklist") 
                ?? new Models.Spring24.ModData_Spring24PairBlacklist();
            var spring24charblacklist = this.Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24CharBlacklist>("spring24charblacklist") 
                ?? new Models.Spring24.ModData_Spring24CharBlacklist();

            //Process configurations from ModConfig
            Monitor.Log($"Reading CNFA configurations from ModConfig.json file.", LogLevel.Trace);

            //ModConfig.json - Spring24 data

            //string[] configSpring24PairWhitelistRaw = Config.
            //ModConfig.json - Spring24PairBlacklist
            string[] configSpring24PairBlacklistRaw = Config.CNFAspring24PairBlacklist.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            foreach (string entry in configSpring24PairBlacklistRaw)
            {
                if (entry.Split("&", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Count() == 1)
                { 
                    Monitor.Log($"Could not load Spring24PairBlacklist entry {Array.IndexOf(configSpring24PairBlacklistRaw, entry)} from ModConfig file as the following input contains only one input: {entry}. Input will be skipped.", LogLevel.Debug); 
                    continue; 
                }
                string upperDancer = entry.Split("&", 2, StringSplitOptions.TrimEntries)[0];
                string lowerDancer = entry.Split("&", 2, StringSplitOptions.TrimEntries)[1];

                bool upperInput = ConfigurationValidator.nameInputValidator(upperDancer);
                bool lowerInput = ConfigurationValidator.nameInputValidator(lowerDancer);

                if (upperInput
                    && lowerInput
                    && !spring24pairblacklist.Data.Any(p => p.ContentID == $"ModConfig_Spring24PairBlacklist_{upperDancer}_{lowerDancer}")) //prevent duplicates from modconfig
                {
                    spring24pairblacklist.Data.Add(new Models.Spring24.Spring24PairBlacklist("ModConfig", entry.Split("&", StringSplitOptions.TrimEntries)[0], entry.Split("&")[1], false));
                }
                if (!upperInput || !lowerInput)
                {
                    string upperInputString = (ConfigurationValidator.nameInputValidator(upperDancer)) ? string.Empty : upperDancer;
                    string lowerInputString = (ConfigurationValidator.nameInputValidator(lowerDancer)) ? string.Empty : lowerDancer;
                    Monitor.Log($"Could not load Spring24PairBlacklist entry {Array.IndexOf(configSpring24PairBlacklistRaw, entry)} from ModConfig file as the following input(s) are invalid: {upperInputString}, {lowerInputString}. Input will be skipped.", LogLevel.Debug);
                }
            }

            //ModConfig.json - Spring24PairWhitelist

            //ModConfig.json - Spring24CharBlacklist
            string[] configSpring24CharBlacklistRaw = Config.CNFAspring24NPCBlacklist.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            foreach (string entry in configSpring24CharBlacklistRaw)
            {
                bool character = ConfigurationValidator.nameInputValidator(entry);
                if (character 
                    && !spring24charblacklist.Data.Any(p => p.ContentID == $"ModConfig_Spring24CharBlacklist_{entry}")) //prevent duplicates from modconfig
                {
                    spring24charblacklist.Data.Add(new Models.Spring24.Spring24CharBlacklist("ModConfig", entry));
                }
                if (!character)
                {
                    Monitor.Log($"Could not load Spring24CharBlacklist entry {Array.IndexOf(configSpring24CharBlacklistRaw, entry)} from ModConfig file as the following input is invalid: {entry}. Input will be skipped.", LogLevel.Debug);
                }
            }


            //Process data from content packs
            foreach (IContentPack contentPack in this.Helper.ContentPacks.GetOwned())
            {
                Monitor.Log($"Reading CNFA content pack {contentPack.Manifest.Name}, version {contentPack.Manifest.Version} by {contentPack.Manifest.Author}.", LogLevel.Trace);

                RawCNFAContentPack contentPackRawData = new RawCNFAContentPack();
                if (!contentPack.HasFile("content.json"))
                {
                    Monitor.Log($"CNFA content pack {contentPack.Manifest.Name}, version {contentPack.Manifest.Version} by {contentPack.Manifest.Author} is missing a \"content.json\" file. Content pack will be skipped.", LogLevel.Debug);
                    break;
                }
                else if (contentPack.HasFile("content.json"))
                {
                    try
                    {
                        contentPackRawData = contentPack.ReadJsonFile<RawCNFAContentPack>("content.json");
                    }
                    catch (Exception invalidContentJson) 
                    {
                        Monitor.Log($"CNFA content pack {contentPack.Manifest.Name}, version {contentPack.Manifest.Version} by {contentPack.Manifest.Author} has an invalid \"content.json\" file. Content pack will be skipped.", LogLevel.Debug);
                        Monitor.Log("" + invalidContentJson, LogLevel.Trace);
                        break;
                    }
                    if (contentPackRawData == null)
                    {
                        Monitor.Log($"CNFA content pack {contentPack.Manifest.Name}, version {contentPack.Manifest.Version} by {contentPack.Manifest.Author} has a \"content.json\" file that lacks any valid content entries. Content pack will be skipped.", LogLevel.Debug);
                        break; 
                    }
                    contentPackRawData.ContentPack = contentPack; //assigns content pack to raw content data

                    int contentIndex = 0;
                    foreach (ModData.RawContent rawContent in contentPackRawData.Entries)
                    {
                        rawContent.ContentPack = contentPackRawData.ContentPack ?? null;
                        switch (rawContent.ContentType)
                        {
                            //ContentPack - Spring24 data
                            case "Spring24PairWhitelist":

                                try
                                {
                                    if (ConfigurationValidator.nameInputValidator(rawContent.ContentFields.UpperDancerName)
                                        && ConfigurationValidator.nameInputValidator(rawContent.ContentFields.LowerDancerName))
                                    {
                                        Models.Spring24.Spring24PairWhitelist pairWhiteList = new Models.Spring24.Spring24PairWhitelist(rawContent);
                                        if (!spring24pairwhitelist.Data.Any(p => p.ContentID == pairWhiteList.ContentID))
                                        {
                                            spring24pairwhitelist.Data.Add(pairWhiteList);
                                        }
                                    }
                                }
                                catch(Exception eRead)
                                {
                                    Monitor.Log($"Could not load contents of content entry index {contentIndex}, content of {rawContent.ContentType} type. Content will be skipped.", LogLevel.Debug);
                                    Monitor.Log($"Content {rawContent.ContentType}, Index {contentIndex} error: " + eRead, LogLevel.Trace);
                                }
                                break;

                            case "Spring24PairBlacklist":
                                try
                                {
                                    if (ConfigurationValidator.nameInputValidator(rawContent.ContentFields.UpperDancerName) 
                                        && ConfigurationValidator.nameInputValidator(rawContent.ContentFields.LowerDancerName))
                                    {
                                        Models.Spring24.Spring24PairBlacklist pairBlackList = new Models.Spring24.Spring24PairBlacklist(rawContent);
                                        if (!spring24charblacklist.Data.Any(p => p.ContentID == pairBlackList.ContentID))
                                        {
                                            spring24pairblacklist.Data.Add(pairBlackList);
                                        }
                                    }
                                }
                                catch(Exception eRead)
                                {
                                    Monitor.Log($"Could not load and save contents of content entry index {contentIndex}, content of {rawContent.ContentType} type. Content will be skipped.", LogLevel.Debug);
                                    Monitor.Log($"Content {rawContent.ContentType}, Index {contentIndex} error: " + eRead, LogLevel.Trace);
                                }
                                break;

                            case "Spring24CharBlacklist":
                                try
                                {
                                    if (ConfigurationValidator.nameInputValidator(rawContent.ContentFields.CharacterName)) 
                                    {
                                        Models.Spring24.Spring24CharBlacklist charBlacklist = new Models.Spring24.Spring24CharBlacklist(rawContent);
                                        if (!spring24charblacklist.Data.Any(c => c.ContentID == charBlacklist.ContentID))
                                        {
                                            spring24charblacklist.Data.Add(charBlacklist);
                                        }
                                    }
                                }
                                catch (Exception eRead)
                                {
                                    Monitor.Log($"Could not load and save contents of content entry index {contentIndex}, content of {rawContent.ContentType} type. Content will be skipped.", LogLevel.Debug);
                                    Monitor.Log($"Content {rawContent.ContentType}, Index {contentIndex} error: " + eRead, LogLevel.Trace);
                                }
                                break;

                            default:
                                Monitor.Log($"{rawContent.ContentType} is not a currently supported content type. Entry {contentIndex} will be skipped.", LogLevel.Debug);
                                break;
                        }
                        contentIndex++;
                    }
                }
            }
            this.Helper.Data.WriteSaveData("spring24pairwhitelist", spring24pairwhitelist);
            this.Helper.Data.WriteSaveData("spring24pairblacklist", spring24pairblacklist);
            this.Helper.Data.WriteSaveData("spring24charblacklist", spring24charblacklist);



            //Testing below, comment out for release :)
            var retrieved = this.Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24PairWhitelist>("spring24pairwhitelist");
            if (retrieved.Data.Any())
            {
                Monitor.Log($"List of retrieved whitelist pair entries by ContentID:", LogLevel.Warn);
                foreach (Models.Spring24.Spring24PairWhitelist entry in retrieved.Data)
                {
                    Monitor.Log($"{entry.ContentID}", LogLevel.Warn);
                }
            }
            var retrieved2 = this.Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24PairBlacklist>("spring24pairblacklist");
            if (retrieved2.Data.Any())
            {
                Monitor.Log($"List of retrieved blacklist pair entries by ContentID:", LogLevel.Warn);
                foreach (Models.Spring24.Spring24PairBlacklist entry in retrieved2.Data)
                {
                    Monitor.Log($"{entry.ContentID}", LogLevel.Warn);
                }
            }

            var retrieved3 = this.Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24CharBlacklist>("spring24charblacklist");
            if (retrieved3.Data.Any())
            {
                Monitor.Log($"List of retrieved blacklist characters by ContentID:", LogLevel.Warn);
                foreach (Models.Spring24.Spring24CharBlacklist entry in retrieved3.Data)
                {
                    Monitor.Log($"{entry.ContentID}", LogLevel.Warn);
                }
            }
        }
    }
}

