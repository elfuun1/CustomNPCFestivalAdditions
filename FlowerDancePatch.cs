using StardewModdingAPI;
using StardewValley.Network;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CustomNPCFestivalAdditions.Spring24
{
    internal class FlowerDancePatch
    {
        public static IMonitor Monitor;
        public static IModHelper Helper;
        public static ModConfig Config;

        public static void Initialize(IMonitor monitor, IModHelper helper, ModConfig config)
        {
            Monitor = monitor;
            Helper = helper;
            Config = config;
        }
        public static void FlowerDanceSetUp(Event __instance)
        {
            try
            {
                //Sets up lists for pair generation
                List<NetDancePartner> upperline = new List<NetDancePartner>();
                List<NetDancePartner> lowerline = new List<NetDancePartner>();

                List<string> poolUpper = new List<string>();
                List<string> poolLower = new List<string>();

                List<Farmer> farmers = (from f in Game1.getOnlineFarmers()
                                        orderby f.UniqueMultiplayerID
                                        select f).ToList();

                //Creates and populates list of actors that are datable and not children
                List<NPC> strictActorList = new List<NPC>();
                List<NPC> flexibleActorList = new List<NPC>();

                foreach (NPC actor in __instance.actors)
                {
                    /*
                    if (actor.datable.Equals(true) && actor.Age != 2 && AllowMixedGenderDanceLines.Equals(true) && actor has FDF sprites)
                        {
                        flexableActorList.Add(actor);
                        }
                    else if (actor.datable.Equals(true) && actor.Age !=2)
                        {
                        strictActorList.Add(actor);
                        }
                    */
                    if (actor.datable.Equals(true) && actor.Age != 2)
                    {
                        strictActorList.Add(actor);
                    }
                }

                //Populates "leftoverGender" lists with datable actors of the correct gender for selection
                foreach (NPC actor in strictActorList)
                {
                    switch (actor.Gender)
                    {
                        case Gender.Male:
                            poolLower.Add(actor.Name);
                            break;

                        case Gender.Female:
                            poolUpper.Add(actor.Name);
                            break;

                        case Gender.Undefined:
                            //TO-DO - Non-binary character pairing
                            Monitor.Log($"Characters with \"undefined\" gender are not currently supported by CNFA. {actor.Name} will be skipped.", LogLevel.Warn);
                            break;
                        default:
                            Monitor.Log($"Failed to add {actor.Name} to a dancer selection pool. {actor.Name} will be skipped.", LogLevel.Warn);
                            break;
                    }
                    continue;
                }
                Monitor.Log($"Successfully added the following NPCs to the lower line dancer selection pool: {string.Join(", ", poolLower)}", LogLevel.Trace);
                Monitor.Log($"Successfully added the following NPCs to the upper line dancer selection pool: {string.Join(", ", poolUpper)}", LogLevel.Trace);

                //TO-DO - Alternate code for mixed-gender dancing lines

                //Set-up farmer pairings for dance

                while (farmers.Count > 0)
                {
                    Farmer f2 = farmers[0];
                    farmers.RemoveAt(0);
                    if (Game1.Multiplayer.isDisconnecting(f2) || f2.dancePartner.Value == null)
                    {
                        continue;
                    }

                    //Removes farmer-farmer pairs from pool
                    if (f2.dancePartner.IsFarmer())
                    {
                        farmers.Remove(f2.dancePartner.TryGetFarmer());
                    }

                    //Adds farmer-NPC pairs to dance lines and removes NPCs from dancer pools
                    try
                    {
                        if (f2.dancePartner.GetGender() == Gender.Female)
                        {
                            upperline.Add(f2.dancePartner);
                            if (f2.dancePartner.IsVillager())
                            {
                                poolUpper.Remove(f2.dancePartner.TryGetVillager().Name);
                            }
                            lowerline.Add(new NetDancePartner(f2));
                        }
                        if (f2.dancePartner.GetGender() == Gender.Male)
                        {
                            lowerline.Add(f2.dancePartner);
                            if (f2.dancePartner.IsVillager())
                            {
                                poolLower.Remove(f2.dancePartner.TryGetVillager().Name);
                            }
                            upperline.Add(new NetDancePartner(f2));
                        }
                        if (f2.dancePartner.GetGender() == Gender.Undefined)
                        {
                            //TO-DO - Non-binary character pairing
                            Monitor.Log($"Characters with \"undefined\" gender are not currently supported by CNFA. Player {f2.Name} and {f2.dancePartner.GetCharacter().Name} will not be added to dance lines and will be skipped.", LogLevel.Warn);
                        }
                    }
                    catch (Exception e)
                    {
                        Monitor.Log($"Failed to populate dancer pools. Exception: {e}", LogLevel.Debug);
                    }

                    Monitor.Log("All farmers successfully paired or skipped.", LogLevel.Trace);
                }

                //Generates NPC-NPC pairs
                while (poolUpper.Any() && poolLower.Any() && upperline.Count() < Config.CNFASpring24NumberDancerPairs)
                {
                    Random rnd = new Random();
                    int rUpper = rnd.Next(poolUpper.Count);
                    string upperDancer = poolUpper[rUpper];

                    if (Config.CNFAspring24RandomizeDancersInPairs.Equals(false))
                    {
                        DanceGenerator.createMutualDispositionLoveInterestPair(poolUpper, upperDancer, poolLower, upperline, lowerline);
                    }
                    else
                    {
                        DanceGenerator.createRandomNPCPair(poolUpper, upperDancer, poolLower, upperline, lowerline);
                    }
                    //TO-DO - Config moderated pairing
                }


                //Determine selected pairs of dancers and remaining unselected NPCs (debugging aid- can be commented out later)
                List<string> dancerPairs = new List<string>();
                foreach (NetDancePartner dancer in upperline)
                {
                    dancerPairs.Add($"{dancer.GetCharacter().Name} and {lowerline[upperline.IndexOf(dancer)].GetCharacter().Name}");
                }
                Monitor.Log($"After pair generation, the following pairs of farmers and/or NPCs were selected to dance: {string.Join(", ", dancerPairs)}.", LogLevel.Debug);

                if (poolUpper.Any() | poolLower.Any())
                {
                    List<string> unselectedDancers = poolUpper.Concat(poolLower).ToList();
                    Monitor.Log($"After pair generation, the following NPCs were not selected to dance: {string.Join(", ", unselectedDancers)}.", LogLevel.Debug);
                }

                //Generate custom flower dance main event commands template
                string rawFestivalData = "";
                try
                {
                    StringBuilder buildFestivalData = new StringBuilder();

                    buildFestivalData.Append("pause 500/playMusic none/pause 500/globalFade/viewport -1000 -1000/loadActors MainEvent/warp farmer1 5 21/warp farmer2 11 21/warp farmer3 23 21/warp farmer4 12 21/faceDirection farmer1 2/faceDirection farmer3 2/faceDirection farmer4 2/faceDirection farmer 2");
                    buildFestivalData.Append(DanceGenerator.BuildEventWarpBlock(upperline));
                    buildFestivalData.Append(DanceGenerator.BuildShowFrameBlock(upperline));
                    buildFestivalData.Append("/viewport 14 25 clamp true/pause 2000/playMusic FlowerDance/pause 600");
                    buildFestivalData.Append(DanceGenerator.BuildAnimateBlock1(upperline));
                    buildFestivalData.Append(DanceGenerator.BuildAnimateBlock2(upperline));
                    buildFestivalData.Append("/pause 9600");
                    buildFestivalData.Append(DanceGenerator.BuildGiantOffsetBlock(upperline));
                    buildFestivalData.Append(DanceGenerator.BuildAnimateBlock3(upperline));
                    buildFestivalData.Append("/pause 7600");
                    buildFestivalData.Append(DanceGenerator.BuildStopAnimationBlock(upperline));
                    buildFestivalData.Append("/pause 3000/globalFade/viewport -1000 -1000/message \"That was fun! Time to go home...\"/waitForOtherPlayers festivalEnd/end");

                    rawFestivalData = buildFestivalData.ToString();
                }

                //Catch returns festival data to that in "Data/Festivals/Spring24.json", in English
                //TO-DO- return to festival data with localized end message

                catch (Exception e)
                {
                    IDictionary<string, string> defaultFestivalData = Helper.GameContent.Load<Dictionary<string, string>>($"Content/Data/Festivals/Spring24.json");
                    rawFestivalData = defaultFestivalData["mainEvent"];
                    Monitor.Log($"Failed to generate a custom main event set-up- reverting to vanilla main event set-up from \"Data/Festivals/spring24.json\" with English end-scene text. Exception: " + e, LogLevel.Debug);
                }

                //Prints completed custom flower dance main event commands template (debugging aid- can be commented out later)
                //Monitor.Log(rawFestivalData, LogLevel.Trace);


                //Begins replacing placeholder "guys" and "girls" in main event commands with dancer pairs
                int i = 1;

                while (i <= upperline.Count())
                {
                    string upperDancerScript = !upperline[i - 1].IsVillager() ? "farmer" + Utility.getFarmerNumberFromFarmer(upperline[i - 1].TryGetFarmer()) : upperline[i - 1].TryGetVillager().Name;
                    string lowerDancerScript = !lowerline[i - 1].IsVillager() ? "farmer" + Utility.getFarmerNumberFromFarmer(lowerline[i - 1].TryGetFarmer()) : lowerline[i - 1].TryGetVillager().Name;
                    rawFestivalData = rawFestivalData.Replace("Girl" + i, upperDancerScript);
                    rawFestivalData = rawFestivalData.Replace("Guy" + i, lowerDancerScript);
                    i++;
                }

                Regex regex = new Regex("showFrame (?<farmerName>farmer\\d) 44");
                Regex showFrameGirl = new Regex("showFrame (?<farmerName>farmer\\d) 40");
                Regex animation1Guy = new Regex("animate (?<farmerName>farmer\\d) false true 600 44 45");
                Regex animation1Girl = new Regex("animate (?<farmerName>farmer\\d) false true 600 43 41 43 42");
                Regex animation2Guy = new Regex("animate (?<farmerName>farmer\\d) false true 300 46 47");
                Regex animation2Girl = new Regex("animate (?<farmerName>farmer\\d) false true 600 46 47");
                rawFestivalData = regex.Replace(rawFestivalData, "showFrame $1 12/faceDirection $1 0");
                rawFestivalData = showFrameGirl.Replace(rawFestivalData, "showFrame $1 0/faceDirection $1 2");
                rawFestivalData = animation1Guy.Replace(rawFestivalData, "animate $1 false true 600 12 13 12 14");
                rawFestivalData = animation1Girl.Replace(rawFestivalData, "animate $1 false true 596 4 0");
                rawFestivalData = animation2Guy.Replace(rawFestivalData, "animate $1 false true 150 12 13 12 14");
                rawFestivalData = animation2Girl.Replace(rawFestivalData, "animate $1 false true 600 0 3");

                //Prints completed custom flower dance main event commands (debugging aid- can be commented out later)
                //Monitor.Log(rawFestivalData, LogLevel.Trace);

                string[] newCommands = __instance.eventCommands = rawFestivalData.Split('/');
            }
            catch (Exception e)
            {
                Monitor.Log($"Failed in {nameof(FlowerDanceSetUp)}:\n{e}", LogLevel.Error);
            }
        }
    }
}
