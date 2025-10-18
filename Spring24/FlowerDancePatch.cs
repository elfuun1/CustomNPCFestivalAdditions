using StardewModdingAPI;
using StardewValley.Network;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.CompilerServices;
using System.Numerics;
using System.Threading.Tasks.Dataflow;
using HarmonyLib;
using CustomNPCFestivalAdditions.ModData;
using System.Reflection;

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

                List<NPC> poolUpper = new List<NPC>();
                List<NPC> poolLower = new List<NPC>();

                List<Farmer> farmers = (from f in Game1.getOnlineFarmers()
                                        orderby f.UniqueMultiplayerID
                                        select f).ToList();


                //Load and process character blacklist
                string[] charBlackList = Config.CNFAspring24NPCBlacklist.Split(", ");
                foreach (NPC actor in __instance.actors)
                {
                    if (charBlackList.Contains(actor.Name))
                    {
                        actor.modData[$"elfuun.CustomNPCFestivalAdditions/isSpring24BlackListed"] = "true";
                    }
                    else
                    {
                        actor.modData[$"elfuun.CustomNPCFestivalAdditions/isSpring24BlackListed"] = "false";
                    }
                }

                //Creates and populates list of actors that are datable and not children
                List<NPC> actorList = new List<NPC>();
                foreach (NPC actor in __instance.actors)
                {
                    if (actor.modData[$"elfuun.CustomNPCFestivalAdditions/isSpring24BlackListed"] == "true")
                    {
                        Monitor.Log($"Did not add {actor.Name} to dancer pools, per blacklist.", LogLevel.Debug);
                        continue;
                    }
                    if (actor.datable.Equals(true))
                    {
                        IAssetName tryGetSprite = Helper.GameContent.ParseAssetName($"Characters/{actor.Name}_CNFASpring24");
                        if (Config.CNFAspring24AllowMixedGenderDanceLines.Equals(true) && Helper.GameContent.DoesAssetExist<Texture2D>(tryGetSprite))
                        {
                            actor.modData[$"elfuun.CustomNPCFestivalAdditions/hasSpring24Sprite"] = "true";
                        }
                        actorList.Add(actor);
                        continue;

                    }
                }

                //Load and process pair blacklist
                ModDataSpring24.updateModDataSpring24PairBlackList(actorList);

                Utilities.Shuffle(actorList);

                //Populates "leftoverGender" lists with datable actors of the correct gender for selection

                int populateIndex = 0;
                while ((populateIndex) < actorList.Count())
                {
                    switch (Config.CNFAspring24RandomizeDancersInPairs.ToString() + Config.CNFAspring24AllowMixedGenderDanceLines.ToString())
                    {
                    //love interest pairing, mixed gender lines

                    case "FalseTrue":
                        //mutual love interest, both have sprites
                            
                        NPC? loveInterest = getMutualDispositionLoveInterest(actorList[populateIndex]);
                        int loveInterestIndex = actorList.FindIndex(NPC => NPC.Name == loveInterest?.Name);
                        if (loveInterest != null
                        && actorList.Any(NPC => NPC.Name == actorList[populateIndex].loveInterest)
                        && hasCNFASpring24Sprite(actorList[populateIndex])
                        && hasCNFASpring24Sprite(actorList[loveInterestIndex]))
                        {
                            Random rnd = new Random();
                            int randomLine = rnd.Next(2);
                            switch (randomLine)
                            {
                                case 1:
                                    poolLower.Add(actorList[populateIndex]);
                                    poolUpper.Add(actorList[loveInterestIndex]);
                                    Monitor.Log($"Added mutual pair of {actorList[populateIndex].Name} and {loveInterest.Name} randomly to the upper pool and lower pool, respectively.", LogLevel.Trace);
                                    actorList.Remove(actorList[loveInterestIndex]);
                                    if (populateIndex < actorList.Count())
                                        { populateIndex++; }
                                    break;
                                case 0:
                                    poolUpper.Add(actorList[populateIndex]);
                                    poolLower.Add(actorList[loveInterestIndex]);
                                    Monitor.Log($"Added mutual pair of {actorList[populateIndex].Name} and {loveInterest.Name} randomly to the lower pool and upper pool, respectively.", LogLevel.Trace);
                                    actorList.Remove(actorList[loveInterestIndex]);
                                        if (populateIndex < actorList.Count())
                                        { populateIndex++; }
                                    break;
                            }
                        }
                        else if ((actorList[populateIndex].loveInterest == null 
                                && hasCNFASpring24Sprite(actorList[populateIndex])) 
                                || 
                                (loveInterest != null
                                && !actorList.Any(NPC => NPC.Name == actorList[populateIndex].loveInterest)
                                && hasCNFASpring24Sprite(actorList[populateIndex])))
                        {
                            switch (poolLower.Count() - poolUpper.Count())
                            {
                                case < 0:
                                    poolLower.Add(actorList[populateIndex]);
                                    populateIndex++;
                                    break;

                                case 0:
                                    poolUpper.Add(actorList[populateIndex]);
                                    populateIndex++;
                                    break;

                                case > 0:
                                    poolUpper.Add(actorList[populateIndex]);
                                    populateIndex++;
                                    break;
                                }
                        }
                        //mutual love interest, one or both lack sprites
                        else if (getMutualDispositionLoveInterest(actorList[populateIndex]) != null)
                        { goto default; }
                        //no mutual love interest exists
                        else { goto case "TrueTrue"; }
                        break;

                    //random pairings, mixed gender lines
                    case "TrueTrue":
                            if (hasCNFASpring24Sprite(actorList[populateIndex]))
                            {
                                if (poolLower.Count() < poolUpper.Count())
                                {
                                    poolLower.Add(actorList[populateIndex]);
                                    populateIndex++;
                                }
                                if (poolUpper.Count() < poolLower.Count())
                                {
                                    poolUpper.Add(actorList[populateIndex]);
                                    populateIndex++;

                                }
                                if (poolUpper.Count() == poolLower.Count())
                                {
                                    poolUpper.Add(actorList[populateIndex]);
                                    populateIndex++;
                                }
                                break;
                            }
                            else { goto default; };
                        

                    //strict dance lines
                    default:
                        switch (actorList[populateIndex].Gender)
                        {
                            case Gender.Male:
                                poolLower.Add(actorList[populateIndex]);
                                break;

                            case Gender.Female:
                                poolUpper.Add(actorList[populateIndex]);
                                break;

                            default:
                                Monitor.Log($"Failed to add {actorList[populateIndex].Name} to a dancer selection pool. {actorList[populateIndex].Name} will be skipped.", LogLevel.Warn);
                                break;
                        }
                        populateIndex++;
                        break;
                    }
                }

                Monitor.Log($"Successfully added the following NPCs to the lower line dancer selection pool:" + string.Join(", ", poolLower.Select(NPC => NPC.Name)), LogLevel.Trace);
                Monitor.Log($"Successfully added the following NPCs to the upper line dancer selection pool:" + string.Join(", ", poolUpper.Select(NPC => NPC.Name)), LogLevel.Trace);

                //Set-up farmer pairings for dance

                List<NPC> invalidatedDancers = new List<NPC>();

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
                                poolUpper.Remove(f2.dancePartner.TryGetVillager());
                            }
                            lowerline.Add(new NetDancePartner(f2));
                        }
                        if (f2.dancePartner.GetGender() == Gender.Male)
                        {
                            lowerline.Add(f2.dancePartner);
                            if (f2.dancePartner.IsVillager())
                            {
                                poolLower.Remove(f2.dancePartner.TryGetVillager());
                            }
                            upperline.Add(new NetDancePartner(f2));
                        }
                        if (f2.dancePartner.GetGender() == Gender.Undefined)
                        {
                            if (Config.CNFAspring24AllowMixedGenderDanceLines.Equals(true) && hasCNFASpring24Sprite(f2.dancePartner))
                            {
                                if (poolLower.Contains(f2.dancePartner.TryGetVillager()))
                                {
                                    lowerline.Add(f2.dancePartner);
                                    poolLower.Remove(f2.dancePartner.TryGetVillager());
                                    upperline.Add(new NetDancePartner(f2));
                                }
                                if (poolUpper.Contains(f2.dancePartner.TryGetVillager()))
                                {
                                    upperline.Add(f2.dancePartner);
                                    poolUpper.Remove(f2.dancePartner.TryGetVillager());
                                    lowerline.Add(new NetDancePartner(f2));
                                }
                            }
                            else
                            {
                                Monitor.Log($"Characters with \"undefined\" gender require that \"Mixed Gender Lines\" be enabled, and custom Spring 24 CNFA sprites to dance. Player {f2.Name} and {f2.dancePartner.GetCharacter().Name} will not be added to dance lines and will be skipped.", LogLevel.Warn);
                                if (poolLower.Contains(f2.dancePartner.TryGetVillager()))
                                {
                                    poolLower.Remove(f2.dancePartner.TryGetVillager());
                                }
                                if (poolUpper.Contains(f2.dancePartner.TryGetVillager()))
                                {
                                    poolUpper.Remove(f2.dancePartner.TryGetVillager());
                                }
                            }
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
                    Utilities.Shuffle(poolUpper);
                    NPC upperDancer = poolUpper[0];

                    if (Config.CNFAspring24RandomizeDancersInPairs.Equals(false))
                    {
                        if (getMutualDispositionLoveInterest(upperDancer) != null)
                        {
                            if (poolLower.Any(NPC => NPC.Name == getMutualDispositionLoveInterest(upperDancer).Name) && isPairBlackListed(upperDancer, getMutualDispositionLoveInterest(upperDancer)).Equals(false))
                            {
                                upperline.Add(new NetDancePartner(upperDancer.Name));
                                lowerline.Add(new NetDancePartner(getMutualDispositionLoveInterest(upperDancer).Name));
                                Monitor.Log($"Made a mutual NPC Disposition \"Love Interest\" pair with {upperDancer.Name} and {getMutualDispositionLoveInterest(upperDancer).Name}, and successfully entered pair into NetDancePartner.", LogLevel.Trace);

                                poolUpper.Remove(upperDancer);
                                int removeMember = poolLower.FindIndex(NPC => NPC.Name == getMutualDispositionLoveInterest(upperDancer).Name);
                                poolLower.RemoveAt(removeMember);
                            }
                            else
                            {
                                Monitor.Log($"Could not make a mutual NPC Disposition \"Love Interest\" pair with {upperDancer.Name} and {getMutualDispositionLoveInterest(upperDancer).Name} as {getMutualDispositionLoveInterest(upperDancer).Name} is not a valid member of the lower dancer pool. Will attempt to pair {upperDancer.Name} with a random lower dancer that lacks a mutual NPC Disposition \"love interest\".", LogLevel.Trace);
                                {
                                    Utilities.Shuffle(poolLower);
                                    int ind = 0;
                                    while (ind < poolLower.Count())
                                    {
                                        if (getMutualDispositionLoveInterest(poolLower[ind]) == null || !poolUpper.Any(NPC => NPC.Name == poolLower[ind].loveInterest))
                                        {
                                            upperline.Add(new NetDancePartner(upperDancer.Name));
                                            lowerline.Add(new NetDancePartner(poolLower[ind].Name));
                                            Monitor.Log($"Randomly made a pair with {upperDancer.Name} and {poolLower[ind].Name} and successfully entered pair into NetDancePartner.", LogLevel.Trace);

                                            poolUpper.Remove(upperDancer);
                                            int removeMember = poolLower.FindIndex(NPC => NPC.Name == poolLower[ind].Name);
                                            poolLower.RemoveAt(removeMember);
                                            break;
                                        }
                                        ind++;
                                    }
                                    if (ind == poolLower.Count())
                                    {
                                        Monitor.Log($"Failed to match {upperDancer.Name} with a random valid dance partner, as no other valid candidates exist in the lower pool. {upperDancer.Name} will be removed from dancer pools and skipped.", LogLevel.Debug);
                                        invalidatedDancers.Add(upperDancer);
                                        poolUpper.Remove(upperDancer);
                                    }
                                    /*
                                    int reshuffle2 = 0;

                                    while (reshuffle2 < 6)
                                    {
                                        Utilities.Shuffle(poolLower);
                                        if (getMutualDispositionLoveInterest(poolLower[0]) == null && isPairBlackListed(upperDancer, poolLower[0]).Equals(false))
                                        {
                                            upperline.Add(new NetDancePartner(upperDancer.Name));
                                            lowerline.Add(new NetDancePartner(poolLower[0].Name));
                                            Monitor.Log($"Randomly made a pair with {upperDancer.Name} and {poolLower[0].Name} and successfully entered pair into NetDancePartner.", LogLevel.Trace);

                                            poolUpper.Remove(upperDancer);
                                            int removeMember = poolLower.FindIndex(NPC => NPC.Name == poolLower[0].Name);
                                            poolLower.RemoveAt(removeMember);
                                            break;
                                        }
                                        else
                                        {
                                            Monitor.Log($"Attempt {reshuffle2}: Attempted to make a pair between {upperDancer.Name} and {poolLower[0].Name}. Failed.");
                                            reshuffle2++;
                                        }
                                    }
                                    if (reshuffle2 == 6)
                                    {
                                        Monitor.Log($"Failed to match {upperDancer.Name} with a random valid dance partner after 6 attempts. {upperDancer.Name} will be removed from dancer pools and skipped.", LogLevel.Debug);
                                        poolUpper.Remove(upperDancer);
                                    }
                                    */

                                }
                            }
                        }
                        else
                        {
                            Monitor.Log($"Could not make a mutual NPC Disposition \"Love Interest\" pair with {upperDancer.Name} as {upperDancer.Name} does not have a listed NPC Disposition \"Love Interest\". Will attempt to pair {upperDancer.Name} with a random lower dancer that lacks a mutual NPC Disposition \"love interest\".", LogLevel.Debug);
                            Utilities.Shuffle(poolLower);
                            int ind = 0;
                            while (ind < poolLower.Count())
                            {
                                if (getMutualDispositionLoveInterest(poolLower[ind]) == null || !poolUpper.Any(NPC => NPC.Name == poolLower[ind].loveInterest))
                                {
                                    upperline.Add(new NetDancePartner(upperDancer.Name));
                                    lowerline.Add(new NetDancePartner(poolLower[ind].Name));
                                    Monitor.Log($"Randomly made a pair with {upperDancer.Name} and {poolLower[ind].Name} and successfully entered pair into NetDancePartner.", LogLevel.Trace);

                                    poolUpper.Remove(upperDancer);
                                    int removeMember = poolLower.FindIndex(NPC => NPC.Name == poolLower[ind].Name);
                                    poolLower.RemoveAt(removeMember);
                                    break;
                                }
                                ind++;
                            }
                            if (ind == poolLower.Count())
                            {
                                Monitor.Log($"Failed to match {upperDancer.Name} with a random valid dance partner, as no other valid candidates exist in the lower pool. {upperDancer.Name} will be removed from dancer pools and skipped.", LogLevel.Debug);
                                invalidatedDancers.Add(upperDancer);
                                poolUpper.Remove(upperDancer);
                            
                            }
                        }
                    }
                    else
                    {
                        int reshuffle2 = 0;

                        while (reshuffle2 < 6)
                        {
                            Utilities.Shuffle(poolLower);
                            if (isPairBlackListed(upperDancer, poolLower[0]).Equals(false))
                            {
                                upperline.Add(new NetDancePartner(upperDancer.Name));
                                lowerline.Add(new NetDancePartner(poolLower[0].Name));
                                Monitor.Log($"Randomly made a pair with {upperDancer.Name} and {poolLower[0].Name} and successfully entered pair into NetDancePartner.", LogLevel.Trace);
                                poolUpper.Remove(upperDancer);
                                poolLower.Remove(poolLower[0]);   
                                break;
                            }
                            else
                            {
                                reshuffle2++;
                                Monitor.Log($"Attempt {reshuffle2}: Attempted to make a pair between {upperDancer.Name} and {poolLower[0].Name}. Failed as a pair consisting of {upperDancer.Name} and {poolLower[0].Name} is blacklisted, per configuration.");
                            }
                        }
                        if (reshuffle2 == 6)
                        {
                            Monitor.Log($"Failed to match {upperDancer.Name} with a random valid dance partner after 6 attempts. {upperDancer.Name} will be removed from dancer pools and skipped.", LogLevel.Debug);
                            invalidatedDancers.Add(upperDancer);
                            poolUpper.Remove(upperDancer);
                        }
                    }
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
                    List<NPC> unselectedDancers = poolUpper.Concat(poolLower).ToList().Concat(invalidatedDancers).ToList();
                    Monitor.Log($"After pair generation, the following NPCs were not selected to dance: {string.Join(", ", unselectedDancers.Select(n => n.Name))}.", LogLevel.Debug);
                }

                //Generate custom flower dance main event commands template
                string rawFestivalData = "";
                try
                {
                    StringBuilder buildFestivalData = new StringBuilder();

                    buildFestivalData.Append("pause 500/playMusic none/pause 500/globalFade/viewport -1000 -1000/loadActors MainEvent/warp farmer1 5 21/warp farmer2 11 21/warp farmer3 23 21/warp farmer4 12 21/faceDirection farmer1 2/faceDirection farmer2 2/faceDirection farmer3 2/faceDirection farmer4 2");
                    if (Config.CNFAspring24AllowMixedGenderDanceLines.Equals(true))
                    {
                        buildFestivalData.Append(DanceGenerator.BuildChangeSpriteBlock(upperline, lowerline));
                    }
                    buildFestivalData.Append(DanceGenerator.BuildEventWarpBlock(upperline));
                    buildFestivalData.Append(DanceGenerator.BuildShowFrameBlock(upperline));
                    buildFestivalData.Append("/viewport 14 25 clamp true/pause 2000/playMusic FlowerDance/pause 600");
                    buildFestivalData.Append(DanceGenerator.BuildAnimateBlock1(upperline));
                    buildFestivalData.Append("/pause 9600");
                    buildFestivalData.Append(DanceGenerator.BuildStopAnimationBlock(upperline));
                    buildFestivalData.Append(DanceGenerator.BuildAnimateBlock2(upperline));
                    buildFestivalData.Append(DanceGenerator.BuildGiantOffsetBlock(upperline));
                    buildFestivalData.Append(DanceGenerator.BuildStopAnimationBlock(upperline));
                    buildFestivalData.Append(DanceGenerator.BuildAnimateBlock3(upperline));
                    buildFestivalData.Append("/pause 7600");
                    buildFestivalData.Append(DanceGenerator.BuildStopAnimationBlock(upperline));
                    buildFestivalData.Append(DanceGenerator.BuildShowFrameBlock2(upperline));
                    buildFestivalData.Append($"/pause 3000/globalFade/viewport -1000 -1000/message \"{Helper.Translation.Get("Spring24.EndMessage")}\"/waitForOtherPlayers festivalEnd/end");

                    rawFestivalData = buildFestivalData.ToString();
                }
                catch (Exception e)
                {
                    IDictionary<string, string> defaultFestivalData = Helper.GameContent.Load<Dictionary<string, string>>($"\\Data\\Festivals\\Spring24");
                    rawFestivalData = defaultFestivalData["mainEvent"];
                    Monitor.Log($"Failed to generate a custom main event set-up- reverting to vanilla main event set-up from \"Data/Festivals/spring24.json\" with English end-scene text. Exception: " + e, LogLevel.Debug);
                }


                //Begins replacing placeholder "guys" and "girls" in main event commands with dancer pairs
                int i = 1;

                while (i <= upperline.Count())
                {
                    string upperDancerScript = !upperline[i - 1].IsVillager() ? "farmer" + Utility.getFarmerNumberFromFarmer(upperline[i - 1].TryGetFarmer()) : upperline[i - 1].TryGetVillager().Name;
                    string lowerDancerScript = !lowerline[i - 1].IsVillager() ? "farmer" + Utility.getFarmerNumberFromFarmer(lowerline[i - 1].TryGetFarmer()) : lowerline[i - 1].TryGetVillager().Name;
                    rawFestivalData = rawFestivalData.Replace($"Girl{i} ", $"{upperDancerScript} ");
                    rawFestivalData = rawFestivalData.Replace($"Guy{i} ", $"{lowerDancerScript} ");
                    i++;
                }

                Regex farmerGuyShowFrameRegex = new Regex("showFrame (?<farmerName>farmer\\d) 44");
                Regex farmerGirlShowFrameRegex = new Regex("showFrame (?<farmerName>farmer\\d) 40");
                Regex farmerAnimation1GuyRegex = new Regex("animate (?<farmerName>farmer\\d) false true 600 44 45");
                Regex farmerAnimation1GirlRegex = new Regex("animate (?<farmerName>farmer\\d) false true 600 43 41 43 42");
                Regex farmerAnimation2GuyRegex = new Regex("animate (?<farmerName>farmer\\d) false true 300 46 47");
                Regex farmerAnimation2GirlRegex = new Regex("animate (?<farmerName>farmer\\d) false true 600 46 47");
                rawFestivalData = farmerGuyShowFrameRegex.Replace(rawFestivalData, "showFrame $1 12/faceDirection $1 0");
                rawFestivalData = farmerGirlShowFrameRegex.Replace(rawFestivalData, "showFrame $1 0/faceDirection $1 2");
                rawFestivalData = farmerAnimation1GuyRegex.Replace(rawFestivalData, "animate $1 false true 600 12 13 12 14");
                rawFestivalData = farmerAnimation1GirlRegex.Replace(rawFestivalData, "animate $1 false true 596 4 0");
                rawFestivalData = farmerAnimation2GuyRegex.Replace(rawFestivalData, "animate $1 false true 150 12 13 12 14");
                rawFestivalData = farmerAnimation2GirlRegex.Replace(rawFestivalData, "animate $1 false true 600 0 3");

                foreach (NetDancePartner dancerUpper in upperline)
                {
                    if (Config.CNFAspring24AllowMixedGenderDanceLines.Equals(true) && hasCNFASpring24Sprite(dancerUpper))
                    {
                        Regex CNFASpring24GirlShowFrame1 = new Regex($"/showFrame {dancerUpper.TryGetVillager().Name} 40");
                        Regex CNFASpring24GirlAnimation1 = new Regex($"/animate {dancerUpper.TryGetVillager().Name} false true 600 43 41 43 42");
                        Regex CNFASpring24GirlAnimation2 = new Regex($"/animate {dancerUpper.TryGetVillager().Name} false true 600 46 47");
                        Regex CNFASpring24GirlStopAnimation = new Regex($"/stopAnimation {dancerUpper.TryGetVillager().Name} 40");
                        Regex CNFASpring24GirlShowFrame2 = new Regex($"/showFrame {dancerUpper.TryGetVillager().Name} 46");

                        rawFestivalData = CNFASpring24GirlShowFrame1.Replace(rawFestivalData, $"/showFrame {dancerUpper.TryGetVillager().Name} 0");
                        rawFestivalData = CNFASpring24GirlAnimation1.Replace(rawFestivalData, $"/animate {dancerUpper.TryGetVillager().Name} false true 600 3 1 3 2");
                        rawFestivalData = CNFASpring24GirlAnimation2.Replace(rawFestivalData, $"/animate {dancerUpper.TryGetVillager().Name} false true 600 6 7");
                        rawFestivalData = CNFASpring24GirlStopAnimation.Replace(rawFestivalData, $"/stopAnimation {dancerUpper.TryGetVillager().Name} 0");
                        rawFestivalData = CNFASpring24GirlShowFrame2.Replace(rawFestivalData, $"/showFrame {dancerUpper.TryGetVillager().Name} 6");
                    }
                    continue;
                }

                foreach (NetDancePartner dancerLower in lowerline)
                {
                    if (Config.CNFAspring24AllowMixedGenderDanceLines.Equals(true) && hasCNFASpring24Sprite(dancerLower))
                    {
                        Regex CNFASpring24GuyShowFrame1 = new Regex($"showFrame {dancerLower.TryGetVillager().Name} 44");
                        Regex CNFASpring24GuyAnimation1 = new Regex($"animate {dancerLower.TryGetVillager().Name} false true 600 44 45");
                        Regex CNFASpring24GuyAnimation2 = new Regex($"animate {dancerLower.TryGetVillager().Name} false true 600 46 47");
                        Regex CNFASpring24GuyStopAnimation = new Regex($"stopAnimation {dancerLower.TryGetVillager().Name} 44");

                        rawFestivalData = CNFASpring24GuyShowFrame1.Replace(rawFestivalData, $"showFrame {dancerLower.TryGetVillager().Name} 8");
                        rawFestivalData = CNFASpring24GuyAnimation1.Replace(rawFestivalData, $"animate {dancerLower.TryGetVillager().Name} false true 600 8 9");
                        rawFestivalData = CNFASpring24GuyAnimation2.Replace(rawFestivalData, $"animate {dancerLower.TryGetVillager().Name} false true 600 10 11");
                        rawFestivalData = CNFASpring24GuyStopAnimation.Replace(rawFestivalData, $"stopAnimation {dancerLower.TryGetVillager().Name} 8");
                    }
                }

                //Prints completed custom flower dance main event commands (debugging aid- can be commented out later)
                //Monitor.Log(rawFestivalData, LogLevel.Trace);

                string[] newCommands = __instance.eventCommands = rawFestivalData.Split('/');
                
            }
            catch (Exception e)
            {
                Monitor.Log($"Failed to create a custom Flower Dance set-up. Will revert to vanilla set-up. Error: {nameof(FlowerDanceSetUp)}:\n{e}", LogLevel.Error);
            }
        }
        /// <summary>
        /// Retrieves the NPC.loveInterest for a given NPC, and determines if said love interest's NPC.loveInterest value is the original NPC.
        /// </summary>
        /// <param name="target">The NPC who's NPC.loveInterest value is being investigated.</param>
        /// <returns>NPC.loveInterest if the love interest is mutual, null if not.</returns>
        public static NPC? getMutualDispositionLoveInterest(NPC target)
        {
            try
            {
                if (target.loveInterest == null || target.loveInterest == "" || target.loveInterest == "null")
                { return null; }

                NPC targetLoveInterest = Game1.getCharacterFromName(target.loveInterest);
                if (target.loveInterest == targetLoveInterest.Name && targetLoveInterest.loveInterest == target.Name)
                {
                    return targetLoveInterest;
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }
        public static bool hasCNFASpring24Sprite(NetDancePartner? dancer)
        {
            if (dancer == null) { return false; }
            if (dancer.TryGetVillager().modData.ContainsKey("elfuun.CustomNPCFestivalAdditions/hasSpring24Sprite") && dancer.TryGetVillager().modData["elfuun.CustomNPCFestivalAdditions/hasSpring24Sprite"] == "true") { return true; }
            else { return false; }
        }
        public static bool hasCNFASpring24Sprite(NPC? dancer)
        {
            if (dancer == null) { return false; }
            if (dancer.modData.ContainsKey("elfuun.CustomNPCFestivalAdditions/hasSpring24Sprite") && dancer.modData["elfuun.CustomNPCFestivalAdditions/hasSpring24Sprite"] == "true") { return true; }
            else { return false; }
        }
        public static bool isPairBlackListed(NetDancePartner dancer, NetDancePartner partner)
        {
            if (dancer.TryGetVillager().modData.ContainsKey("elfuun.CustomNPCFestivalAdditions/Spring24PairBlackList")
                && dancer.TryGetVillager().modData["elfuun.CustomNPCFestivalAdditions/Spring24PairBlackList"].ToString().Split(",").Contains(partner.TryGetVillager().Name))
            { return true; }

            else if (partner.TryGetVillager().modData.ContainsKey("elfuun.CustomNPCFestivalAdditions/Spring24PairBlackList")
                && partner.TryGetVillager().modData["elfuun.CustomNPCFestivalAdditions/Spring24PairBlackList"].ToString().Split(",").Contains(dancer.TryGetVillager().Name))
            { return true; }

            else { return false; }
        }
        public static bool isPairBlackListed(NPC dancer, NPC partner)
        {
            if (dancer.modData.ContainsKey("elfuun.CustomNPCFestivalAdditions/Spring24PairBlackList")
                && dancer.modData["elfuun.CustomNPCFestivalAdditions/Spring24PairBlackList"].ToString().Split(",").Contains(partner.Name))
            { return true; }

            else if (partner.modData.ContainsKey("elfuun.CustomNPCFestivalAdditions/Spring24PairBlackList")
                && partner.modData["elfuun.CustomNPCFestivalAdditions/Spring24PairBlackList"].ToString().Split(",").Contains(dancer.Name))
            { return true; }
            else { return false; }
        }
    }
}
