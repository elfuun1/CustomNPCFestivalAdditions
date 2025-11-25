using CustomNPCFestivalAdditions.Models;
using CustomNPCFestivalAdditions.Models.Spring24;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFestivalAdditions
{
    internal class CNFAConsoleCommands
    {
        public static IMonitor Monitor;
        public static IModHelper Helper;

        public static void Initialize(IMonitor monitor, IModHelper helper)
        {
            Helper = helper;
            Monitor = monitor;
        }

        public void CNFA_ClearContent(string command, string[] args)
        {
            switch (args[0])
            {
                //Spring24 Content
                case "Spring24CharacterBlacklist":
                    var spring24charblacklistcase = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24CharBlacklist>("spring24charblacklist")
                    ?? new Models.Spring24.ModData_Spring24CharBlacklist();
                    foreach (Models.Spring24.Spring24CharBlacklist entry in spring24charblacklistcase.Data)
                    {
                        switch (entry.Source)
                        {
                            case "ContentPack":
                                entry.Enabled = false;
                                Monitor.Log($"Disabled {entry.ContentID}", LogLevel.Trace);
                                break;
                            case "ModConfig":
                                entry.Enabled = false;
                                Monitor.Log($"Disabled {entry.ContentID}", LogLevel.Trace);
                                break;
                            case "Terminal":
                                spring24charblacklistcase.Data.Remove(entry);
                                Monitor.Log($"Removed {entry.ContentID}", LogLevel.Trace);
                                break;
                            case "GUI":
                                spring24charblacklistcase.Data.Remove(entry);
                                Monitor.Log($"Removed {entry.ContentID}", LogLevel.Trace);
                                break;
                            default:
                                entry.Enabled = false;
                                Monitor.Log($"Disabled {entry.ContentID}", LogLevel.Trace);
                                break;
                        }
                    }
                    Helper.Data.WriteSaveData("spring24pairblacklist", spring24charblacklistcase);
                    Monitor.Log("Disabled or removed all content in the Spring 24 Character Blacklist.", LogLevel.Info);
                    break;
                case "Spring24PairBlacklist":
                    var spring24pairblacklistcase = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24PairBlacklist>("spring24pairblacklist")
                    ?? new Models.Spring24.ModData_Spring24PairBlacklist();
                    foreach (Models.Spring24.Spring24PairBlacklist entry in spring24pairblacklistcase.Data)
                    {
                        switch (entry.Source)
                        {
                            case "ContentPack":
                                entry.Enabled = false;
                                Monitor.Log($"Disabled {entry.ContentID}", LogLevel.Trace);
                                break;
                            case "ModConfig":
                                entry.Enabled = false;
                                Monitor.Log($"Disabled {entry.ContentID}", LogLevel.Trace);
                                break;
                            case "Terminal":
                                spring24pairblacklistcase.Data.Remove(entry);
                                Monitor.Log($"Removed {entry.ContentID}", LogLevel.Trace);
                                break;
                            case "GUI":
                                spring24pairblacklistcase.Data.Remove(entry);
                                Monitor.Log($"Removed {entry.ContentID}", LogLevel.Trace);
                                break;
                            default:
                                entry.Enabled = false;
                                Monitor.Log($"Disabled {entry.ContentID}", LogLevel.Trace);
                                break;
                        }
                    }
                    Helper.Data.WriteSaveData("spring24pairblacklist", spring24pairblacklistcase);
                    Monitor.Log("Disabled or removed all content in the Spring 24 Pair Blacklist.", LogLevel.Info);
                    break;
                default:
                    break;
            }
        }
        public static void CNFA_PrintLoadedContent(string command, string[] args)
        {
            if (args == Array.Empty<string>())
            {
                args = new string[] { "all" };
            }
            switch (args[0])
            {
                case "Spring24CharacterBlacklist":
                    var spring24charblacklistcase = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24CharBlacklist>("spring24charblacklist")
                    ?? new Models.Spring24.ModData_Spring24CharBlacklist();
                    if (!spring24charblacklistcase.Data.Any())
                    {
                        Monitor.Log("Spring 24 Character Blacklist does not contain any valid content.", LogLevel.Info);
                        break;
                    }
                    Monitor.Log("Spring 24 Character Blacklist contains the following content:", LogLevel.Info);
                    foreach (Models.Spring24.Spring24CharBlacklist entry in spring24charblacklistcase.Data)
                    {
                        PrintListContent(entry);
                    }
                    break;
                case "Spring24PairBlacklist":
                    var spring24pairblacklistcase = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24PairBlacklist>("spring24pairblacklist")
                    ?? new Models.Spring24.ModData_Spring24PairBlacklist();
                    if (!spring24pairblacklistcase.Data.Any())
                    {
                        Monitor.Log("Spring 24 Pair Blacklist does not contain any valid content.", LogLevel.Info);
                        break;
                    }
                    Monitor.Log("Spring 24 Pair Blacklist contains the following content:", LogLevel.Info);
                    foreach (Models.Spring24.Spring24PairBlacklist entry in spring24pairblacklistcase.Data)
                    {
                        PrintListContent(entry);
                    }
                    break;

                case "Spring24PairWhiteslist":
                    var spring24pairwhitelistcase = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24PairWhitelist>("spring24pairwhitelist")
                    ?? new Models.Spring24.ModData_Spring24PairWhitelist();
                    if (!spring24pairwhitelistcase.Data.Any())
                    {
                        Monitor.Log("Spring 24 Pair Whitelist does not contain any valid content.", LogLevel.Info);
                        break;
                    }
                    Monitor.Log("Spring 24 Pair Whitelist contains the following content:", LogLevel.Info);
                    foreach (Models.Spring24.Spring24PairWhitelist entry in spring24pairwhitelistcase.Data)
                    {
                        PrintListContent(entry);
                    }
                    break;

                case "all":
                    var spring24charblacklistall = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24CharBlacklist>("spring24charblacklist")
                    ?? new Models.Spring24.ModData_Spring24CharBlacklist();
                    if (!spring24charblacklistall.Data.Any())
                    {
                        Monitor.Log("Spring 24 Character Blacklist does not contain any valid content.", LogLevel.Info);
                    }
                    else
                    {
                        Monitor.Log("Spring 24 Character Blacklist contains the following content:", LogLevel.Info);
                        foreach (Models.Spring24.Spring24CharBlacklist entry in spring24charblacklistall.Data)
                        {
                            PrintListContent(entry);
                        }
                    }

                    Monitor.Log("", LogLevel.Info); //Terminal line spacer

                    var spring24pairblacklistall = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24PairBlacklist>("spring24pairblacklist")
                    ?? new Models.Spring24.ModData_Spring24PairBlacklist();
                    if (!spring24pairblacklistall.Data.Any())
                    {
                        Monitor.Log("Spring 24 Pair Blacklist does not contain any valid content.", LogLevel.Info);
                    }
                    else
                    {
                        Monitor.Log("Spring 24 Pair Blacklist contains the following content:", LogLevel.Info);
                        foreach (Models.Spring24.Spring24PairBlacklist entry in spring24pairblacklistall.Data)
                        {
                            PrintListContent(entry);
                        }
                    }

                    Monitor.Log("", LogLevel.Info); //Terminal line spacer

                    var spring24pairwhitelistall = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24PairWhitelist>("spring24pairwhitelist")
                    ?? new Models.Spring24.ModData_Spring24PairWhitelist();
                    if (!spring24pairwhitelistall.Data.Any())
                    {
                        Monitor.Log("Spring 24 Pair Whitelist does not contain any valid content.", LogLevel.Info);
                        break;
                    }
                    else
                    {
                        Monitor.Log("Spring 24 Pair Whitelist contains the following content:", LogLevel.Info);
                        foreach (Models.Spring24.Spring24PairWhitelist entry in spring24pairwhitelistall.Data)
                        {
                            PrintListContent(entry);
                        }
                    }
                    break;

                default:
                    Monitor.Log($"{args[0]} is not a valid Content Type. Valid Content Types are \"Spring24CharBlacklist\", \"Spring24PairBlacklist\", and \"Spring24Whitelist\".", LogLevel.Info);
                    break;
            }
        }
        public static void CNFA_PrintEnabledContent(string command, string[] args)
        {
            if (args == Array.Empty<string>())
            {
                args = new string[] { "all" };
            }
            switch (args[0])
            {
                case "Spring24CharBlacklist":
                    var spring24charblacklistcase = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24CharBlacklist>("spring24charblacklist")
                    ?? new Models.Spring24.ModData_Spring24CharBlacklist();
                    if (!spring24charblacklistcase.Data.Any(p => p.Enabled == true))
                    {
                        Monitor.Log("Spring 24 Character Blacklist does not contain any valid enabled content.", LogLevel.Info);
                        break;
                    }
                    Monitor.Log("Spring 24 Character Blacklist contains the following enabled content:", LogLevel.Info);
                    foreach (Models.Spring24.Spring24CharBlacklist entry in spring24charblacklistcase.Data)
                    {
                        if(entry.Enabled == true)
                        {
                            PrintListContent(entry);
                        }
                    }
                    break;

                case "Spring24PairBlacklist":
                    var spring24pairblacklistcase = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24PairBlacklist>("spring24pairblacklist")
                    ?? new Models.Spring24.ModData_Spring24PairBlacklist();
                    if (!spring24pairblacklistcase.Data.Any(p => p.Enabled == true))
                    {
                        Monitor.Log("Spring 24 Pair Blacklist does not contain any valid enabled content.", LogLevel.Info);
                        break;
                    }
                    Monitor.Log("Spring 24 Pair Blacklist contains the following enabled content:", LogLevel.Info);
                    foreach (Models.Spring24.Spring24PairBlacklist entry in spring24pairblacklistcase.Data)
                    {
                        if(entry.Enabled == true)
                        {
                            PrintListContent(entry);
                        }
                    }
                    break;

                case "Spring24PairWhitelist":
                    var spring24pairwhitelistcase = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24PairWhitelist>("spring24pairwhitelist")
                    ?? new Models.Spring24.ModData_Spring24PairWhitelist();
                    if (!spring24pairwhitelistcase.Data.Any(p => p.Enabled == true))
                    {
                        Monitor.Log("Spring 24 Pair Whitelist does not contain any valid enabled content.", LogLevel.Info);
                        break;
                    }
                    Monitor.Log("Spring 24 Pair Whitelist contains the following enabled content:", LogLevel.Info);
                    foreach (Models.Spring24.Spring24PairWhitelist entry in spring24pairwhitelistcase.Data)
                    {
                        if (entry.Enabled == true)
                        {
                            PrintListContent(entry);
                        }
                    }
                    break;

                case "all":
                    var spring24charblacklistall = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24CharBlacklist>("spring24charblacklist")
                    ?? new Models.Spring24.ModData_Spring24CharBlacklist();
                    if (!spring24charblacklistall.Data.Any(p => p.Enabled == true))
                    {
                        Monitor.Log("Spring 24 Character Blacklist does not contain any valid enabled content.", LogLevel.Info);
                    }
                    else 
                    {
                        Monitor.Log("Spring 24 Character Blacklist contains the following enabled content:", LogLevel.Info);
                        foreach (Models.Spring24.Spring24CharBlacklist entry in spring24charblacklistall.Data)
                        {
                            if (entry.Enabled == true)
                            {
                                PrintListContent(entry);
                            }
                        }
                    }

                    Monitor.Log("", LogLevel.Info); //Terminal line spacer

                    var spring24pairblacklistall = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24PairBlacklist>("spring24pairblacklist")
                    ?? new Models.Spring24.ModData_Spring24PairBlacklist();
                    if (!spring24pairblacklistall.Data.Any(p => p.Enabled == true))
                    {
                        Monitor.Log("Spring 24 Pair Blacklist does not contain any valid enabled content.", LogLevel.Info);
                    }
                    else
                    {
                        Monitor.Log("Spring 24 Pair Blacklist contains the following enabled content:", LogLevel.Info);
                        foreach (Models.Spring24.Spring24PairBlacklist entry in spring24pairblacklistall.Data)
                        {
                            if (entry.Enabled == true)
                            {
                                PrintListContent(entry);
                            }
                        }
                    }
                    var spring24pairwhitelistall = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24PairWhitelist>("spring24pairwhitelist")
                    ?? new Models.Spring24.ModData_Spring24PairWhitelist();
                    if (!spring24pairwhitelistall.Data.Any(p => p.Enabled == true))
                    {
                        Monitor.Log("Spring 24 Pair Whitelist does not contain any valid enabled content.", LogLevel.Info);
                        break;
                    }
                    else 
                    {
                        Monitor.Log("Spring 24 Pair Whitelist contains the following enabled content:", LogLevel.Info);
                        foreach (Models.Spring24.Spring24PairWhitelist entry in spring24pairwhitelistall.Data)
                        {
                            if (entry.Enabled)
                            {
                                PrintListContent(entry);
                            }
                        }
                    }
                    break;

                default:
                    Monitor.Log($"{args[0]} is not a valid Content Type. Valid Content Types are \"Spring24CharBlacklist\", \"Spring24PairBlacklist\", and \"Spring24Whitelist\", and \"all\".", LogLevel.Info);
                    break;
            }
        }
        public static void CNFA_EnableContent(string command, string[] args)
        {
            if (args == Array.Empty<string>())
            {
                Monitor.Log($"No Content ID was inputted.", LogLevel.Info);
                return; 
            }
            string[] splitArgs = args[0].Split("_");
            string contentType = splitArgs[0];
            switch (contentType)
            {
                case "Spring24CharBlacklist":
                    var spring24charblacklistcase = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24CharBlacklist>("spring24charblacklist")
                    ?? new Models.Spring24.ModData_Spring24CharBlacklist();
                    if (spring24charblacklistcase.Data.FirstOrDefault(p => p.ContentID == args[0]) != null)
                    {
                        spring24charblacklistcase.Data.FirstOrDefault(p => p.ContentID == args[0]).Enabled = true;

                        Helper.Data.WriteSaveData("spring24pairblacklist", spring24charblacklistcase);
                        Monitor.Log($"Enabled Spring24CharBlacklist content {args[0]}, adding {splitArgs[splitArgs.Count() -1]} to the Spring 24 character blacklist.", LogLevel.Info);
                    }
                    else
                    {
                        Monitor.Log($"No entry in Spring24CharBlacklist with the Content ID {args[0]} could be found. Content could not be enabled.");
                    }
                    break;
                case "Spring24PairBlacklist":
                    var spring24pairblacklistcase = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24PairBlacklist>("spring24pairblacklist")
                    ?? new Models.Spring24.ModData_Spring24PairBlacklist();
                    if (spring24pairblacklistcase.Data.FirstOrDefault(p => p.ContentID == args[0]) != null)
                    {
                        spring24pairblacklistcase.Data.FirstOrDefault(p => p.ContentID == args[0]).Enabled = true;
                        Helper.Data.WriteSaveData("spring24pairblacklist", spring24pairblacklistcase);

                        string positionstrict = (spring24pairblacklistcase.Data.FirstOrDefault(p => p.ContentID == args[0]).IsPositionStrict) ? " (Position strict)" : "";
                        Monitor.Log($"Enabled Spring24PairBlacklist content {args[0]}, adding the pair {splitArgs[splitArgs.Count()-2]} and {splitArgs[splitArgs.Count()-1]}" + positionstrict + " to the Spring 24 pair blacklist.", LogLevel.Info);
                    }
                    else
                    {
                        Monitor.Log($"No entry in Spring24PairBlacklist with the Content ID {args[0]} could be found. Content could not be enabled.");
                    }
                    break;
                case "Spring24PairWhitelist":
                    var spring24pairwhitelistcase = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24PairWhitelist>("spring24pairwhitelist")
                    ?? new Models.Spring24.ModData_Spring24PairWhitelist();
                    if (spring24pairwhitelistcase.Data.FirstOrDefault(p => p.ContentID == args[0]) != null)
                    {
                        spring24pairwhitelistcase.Data.FirstOrDefault(p => p.ContentID == args[0]).Enabled = true;
                        Helper.Data.WriteSaveData("spring24pairblacklist", spring24pairwhitelistcase);

                        string positionstrict = (spring24pairwhitelistcase.Data.FirstOrDefault(p => p.ContentID == args[0]).IsPositionStrict) ? " (Position strict)" : "";
                        Monitor.Log($"Enabled Spring24PairBlacklist content {args[0]}, adding the pair {splitArgs[splitArgs.Count() - 2]} and {splitArgs[splitArgs.Count() - 1]}" + positionstrict + " to the Spring 24 pair blacklist.", LogLevel.Info);
                    }
                    else
                    {
                        Monitor.Log($"No entry in Spring24PairWhitelist with the Content ID {args[0]} could be found. Content could not be enabled.");
                    }
                    break;
                default:
                    Monitor.Log($"{args[0]} is not a valid Content ID. Content could not be enabled.", LogLevel.Info);
                    break;
            }
        }
        public static void CNFA_DisableContent(string command, string[] args)
        {
            if (args == Array.Empty<string>())
            {
                Monitor.Log($"No Content ID was inputted.", LogLevel.Info);
                return;
            }
            string[] splitargs = args[0].Split("_");
            string contentType = splitargs[0];
            switch (contentType)
            {
                case "Spring24CharBlacklist":
                    var spring24charblacklistcase = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24CharBlacklist>("spring24charblacklist")
                    ?? new Models.Spring24.ModData_Spring24CharBlacklist();
                    if (spring24charblacklistcase.Data.FirstOrDefault(p => p.ContentID == args[0]) != null)
                    {
                        spring24charblacklistcase.Data.FirstOrDefault(p => p.ContentID == args[0]).Enabled = false;
                        Helper.Data.WriteSaveData("spring24charblacklist", spring24charblacklistcase);
                        Monitor.Log($"Disabled Spring24CharBlacklist content {args[0]}, removing {splitargs[splitargs.Count()-1]} from the Spring 24 character blacklist.", LogLevel.Info);
                    }
                    else
                    {
                        Monitor.Log($"No entry in Spring24PairBlacklist with the Content ID {args[0]} could be found. Content could not be disabled.");
                    }
                    break;
                case "Spring24PairBlacklist":
                    var spring24pairblacklistcase = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24PairBlacklist>("spring24pairblacklist")
                    ?? new Models.Spring24.ModData_Spring24PairBlacklist();
                    if (spring24pairblacklistcase.Data.FirstOrDefault(p => p.ContentID == args[0]) != null)
                    {
                        spring24pairblacklistcase.Data.FirstOrDefault(p => p.ContentID == args[0]).Enabled = false;
                        Helper.Data.WriteSaveData("spring24pairblacklist", spring24pairblacklistcase);

                        string positionstrict = (spring24pairblacklistcase.Data.FirstOrDefault(p => p.ContentID == args[0]).IsPositionStrict) ? " (Position strict)" : "";
                        Monitor.Log($"Disabled Spring24PairBlacklist content {args[0]}, removing the pair {args[args.Count() - 2]} and {args[args.Count() - 1]}" + positionstrict + " from the Spring 24 pair blacklist.", LogLevel.Info);
                    }
                    else
                    {
                        Monitor.Log($"No entry in Spring24PairBlacklist with the Content ID {args[0]} could be found. Content could not be disabled.");
                    }
                    break;
                default:
                    Monitor.Log($"{args[0]} is not a valid Content ID. Content could not be disabled.", LogLevel.Info);
                    break;
            }
        }
        public static void PrintListContent(Models.ContentModel entry)
        {
            switch (entry)
            {
                case Spring24Pair pair:
                    string enabledPair = (pair.Enabled) ? " (Enabled)" : "";
                    string positionstrict = (pair.IsPositionStrict) ? " (Position strict)" : "";
                    string[] ContentIDSplitPair = entry.ContentID.Split("_");
                    if (entry.Source == "ContentPack")
                    {
                        Monitor.Log($"{ContentIDSplitPair[3]} and {ContentIDSplitPair[4]}, added by Content Pack {ContentIDSplitPair[1]}, version {ContentIDSplitPair[2]}" + positionstrict + enabledPair + $" (ContentID: {entry.ContentID})", LogLevel.Info);
                    }
                    else if (entry.Source == "Terminal" || entry.Source == "GUI")
                    {
                        Monitor.Log($"{ContentIDSplitPair[3]} and {ContentIDSplitPair[4]}, added by {entry.Source} on {ContentIDSplitPair[2]}" + positionstrict + enabledPair + $" (ContentID: {entry.ContentID})", LogLevel.Info);
                    }
                    else
                    {
                        Monitor.Log($"{ContentIDSplitPair[2]} and {ContentIDSplitPair[3]}, added by {entry.Source}" + positionstrict + enabledPair + $" (ContentID: {entry.ContentID})", LogLevel.Info);
                    }
                    break;
                case Spring24CharBlacklist chara:
                    string enabledChara = (entry.Enabled) ? " (Enabled)" : "";
                    string[] ContentIDSplitChara = entry.ContentID.Split("_");
                    if (entry.Source == "ContentPack")
                    {
                        Monitor.Log($"{ContentIDSplitChara[4]}, added by Content Pack {ContentIDSplitChara[0]}, version {ContentIDSplitChara[1]}" + enabledChara + $" (ContentID: {entry.ContentID})", LogLevel.Info);
                    }
                    else if (entry.Source == "Terminal" || entry.Source == "GUI")
                    {
                        Monitor.Log($"{ContentIDSplitChara[3]}, added by {entry.Source} on {ContentIDSplitChara[2]}" + enabledChara + $"( ContentID: {entry.ContentID})", LogLevel.Info);
                    }
                    else
                    {
                        Monitor.Log($"{ContentIDSplitChara[3]}, added by {entry.Source}" + enabledChara + $"( ContentID: {entry.ContentID})", LogLevel.Info);
                    }
                    break;
            }
        }
        public static void RemoveContent (ContentModel entry, List<ContentModel> inputList)
        {
            switch (entry.Source)
            {
                case "ContentPack":
                    entry.Enabled = false;
                    Monitor.Log($"Disabled {entry.ContentID}", LogLevel.Trace);
                    break;
                case "ModConfig":
                    entry.Enabled = false;
                    Monitor.Log($"Disabled {entry.ContentID}", LogLevel.Trace);
                    break;
                case "Terminal":
                    inputList.Remove(entry);
                    Monitor.Log($"Removed {entry.ContentID}", LogLevel.Trace);
                    break;
                case "GUI":
                    inputList.Remove(entry);
                    Monitor.Log($"Removed {entry.ContentID}", LogLevel.Trace);
                    break;
                default:
                    entry.Enabled = false;
                    Monitor.Log($"Disabled {entry.ContentID}", LogLevel.Trace);
                    break;
            }
        }
        public static void CNFA_AddContent(string command, string[] args)
        {
            if (args == Array.Empty<string>())
            {
                Monitor.Log($"No valid content was inputted.", LogLevel.Info);
                return;
            }
            switch (args[0])
            {
                case "Spring24CharBlacklist":
                    var spring24charblacklistcase = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24CharBlacklist>("spring24charblacklist")
                    ?? new Models.Spring24.ModData_Spring24CharBlacklist();

                    if (ConfigurationValidator.nameInputValidator(args[1]))
                    {
                        Spring24CharBlacklist terminalCharBlacklist = new Spring24CharBlacklist("Terminal", args[1]);
                        spring24charblacklistcase.Data.Add(terminalCharBlacklist);

                        Helper.Data.WriteSaveData("spring24charblacklist", spring24charblacklistcase);

                        Monitor.Log($"Successfully added {terminalCharBlacklist.CharacterName} (Content ID: {terminalCharBlacklist.ContentID}) to the Spring 24 Character Blacklist.", LogLevel.Info);
                    }
                    else
                    {
                        Monitor.Log($"{args[1]} is not a valid character name. Could not add {args[1]} to the Spring 24 Character Blacklist.", LogLevel.Info);
                    }
                        break;
                case "Spring24PairBlacklist":
                    break;
                case "Spring24PairWhitelist":
                    break;
                default:
                    Monitor.Log($"{args[0]} is not a valid Content Type. Valid Content Types are \"Spring24CharBlacklist\", \"Spring24PairBlacklist\", and \"Spring24PairWhitelist\". Content could not be added.", LogLevel.Info);
                    break;
            }
        }
    }
}
