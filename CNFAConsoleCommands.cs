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

        public void spring24EmptyCharBlacklist(string command, string[] args)
        {

        }
        public static void CNFA_PrintLoadedContent(string command, string[] args)
        {
            try 
            {
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
                            string enabled = (entry.Enabled) ? " (Enabled)" : "";
                            string[] ContentIDSplit = entry.ContentID.Split("_");
                            if (entry.Source == "ContentPack")
                            {
                                Monitor.Log($"{ContentIDSplit[4]}, added by Content Pack {ContentIDSplit[0]}, version {ContentIDSplit[1]}" + enabled, LogLevel.Info);
                            }
                            else if (entry.Source == "Terminal" || entry.Source == "GUI")
                            {
                                Monitor.Log($"{ContentIDSplit[3]}, added by {entry.Source} on {ContentIDSplit[2]}" + enabled, LogLevel.Info);
                            }
                            else
                            {
                                Monitor.Log($"{ContentIDSplit[3]}, added by {entry.Source}" + enabled, LogLevel.Info);
                            }
                        }

                        break;
                    case "Spring24PairBlacklist":
                        var spring24pairblacklistcase = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24PairBlacklist>("spring24pairblacklist")
                        ?? new Models.Spring24.ModData_Spring24PairBlacklist();
                        if (!spring24pairblacklistcase.Data.Any())
                        {
                            Monitor.Log("Spring 24 Character Blacklist does not contain any valid content.", LogLevel.Info);
                            break;
                        }
                        Monitor.Log("Spring 24 Character Blacklist contains the following content:", LogLevel.Info);
                        foreach (Models.Spring24.Spring24PairBlacklist entry in spring24pairblacklistcase.Data)
                        {
                            string enabled = (entry.Enabled) ? " (Enabled)" : "";
                            string positionstrict = (entry.IsPositionStrict) ? " (Position strict)" : "";
                            string[] ContentIDSplit = entry.ContentID.Split("_");
                            if (entry.Source == "ContentPack")
                            {
                                Monitor.Log($"{ContentIDSplit[3]} and {ContentIDSplit[4]}, added by Content Pack {ContentIDSplit[1]}, version {ContentIDSplit[2]}" + positionstrict + enabled, LogLevel.Info);
                                Monitor.Log($"ContentID: {entry.ContentID}", LogLevel.Trace);
                            }
                            else if (entry.Source == "Terminal" || entry.Source == "GUI")
                            {
                                Monitor.Log($"{ContentIDSplit[3]} and {ContentIDSplit[4]}, added by {entry.Source} on {ContentIDSplit[2]}" + positionstrict + enabled, LogLevel.Info);
                                Monitor.Log($"ContentID: {entry.ContentID}", LogLevel.Trace);
                            }
                            else
                            {
                                Monitor.Log($"{ContentIDSplit[2]} and {ContentIDSplit[3]}, added by {entry.Source}" + positionstrict + enabled, LogLevel.Info);
                                Monitor.Log($"ContentID: {entry.ContentID}", LogLevel.Trace);
                            }
                        }
                        break;
                    case "all":
                        goto default;

                    default:
                        var spring24pairwhitelist = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24PairWhitelist>("spring24pairwhitelist")
                        ?? new Models.Spring24.ModData_Spring24PairWhitelist();
                        var spring24pairblacklist = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24PairBlacklist>("spring24pairblacklist")
                            ?? new Models.Spring24.ModData_Spring24PairBlacklist();
                        var spring24charblacklist = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24CharBlacklist>("spring24charblacklist")
                            ?? new Models.Spring24.ModData_Spring24CharBlacklist();
                        Monitor.Log("test", LogLevel.Info);
                        break;
                }

            }
            catch (IndexOutOfRangeException e)
            {
                Monitor.Log("Not Found:" + e, LogLevel.Info);
            }
        }
        public static void CNFA_PrintEnabledContent(string command, string[] args)
        {
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
                        if (entry.Enabled)
                        {
                            Monitor.Log($"{entry.ContentID}", LogLevel.Info);
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
                        if (entry.Enabled)
                        {
                            Monitor.Log($"{entry.ContentID}", LogLevel.Info);
                        }
                    }
                    break;

                default:
                    break;
            }
        }
        public static void CNFA_EnableContent(string command, string[] args)
        {
            string contentType = args[0].Split("_")[0];
            switch (contentType)
            {
                case "Spring24PairBlacklist":
                    var spring24pairblacklistcase = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24PairBlacklist>("spring24pairblacklist")
                    ?? new Models.Spring24.ModData_Spring24PairBlacklist();
                    if (spring24pairblacklistcase.Data.FirstOrDefault(p => p.ContentID == args[0]) != null)
                    {
                        spring24pairblacklistcase.Data.FirstOrDefault(p => p.ContentID == args[0]).Enabled = true;

                        Helper.Data.WriteSaveData("spring24pairblacklist", spring24pairblacklistcase);
                        Monitor.Log($"Enabled Spring24PairBlacklist content {args[0]}.", LogLevel.Info);
                    }
                    else
                    {
                        Monitor.Log($"No entry in Spring24PairBlacklist with the Content ID {args[0]} could be found. Content could not be enabled.");
                    }
                    break;
                default:
                    Monitor.Log($"{args[0]} is not a valid Content ID. Content could not be enabled.", LogLevel.Info);
                    break;
            }
        }
        public static void CNFA_DisableContent(string command, string[] args)
        {
            string contentType = args[0].Split("_")[0];
            switch (contentType)
            {
                case "Spring24PairBlacklist":
                    var spring24pairblacklistcase = Helper.Data.ReadSaveData<Models.Spring24.ModData_Spring24PairBlacklist>("spring24pairblacklist")
                    ?? new Models.Spring24.ModData_Spring24PairBlacklist();
                    if (spring24pairblacklistcase.Data.FirstOrDefault(p => p.ContentID == args[0]) != null)
                    {
                        spring24pairblacklistcase.Data.FirstOrDefault(p => p.ContentID == args[0]).Enabled = false;
                        Helper.Data.WriteSaveData("spring24pairblacklist", spring24pairblacklistcase);
                        Monitor.Log($"Disabled Spring24PairBlacklist content {args[0]}.", LogLevel.Info);
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
    }
}
