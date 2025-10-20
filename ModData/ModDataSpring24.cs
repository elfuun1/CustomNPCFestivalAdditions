using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace CustomNPCFestivalAdditions.ModData
{
    public class Spring24PairWhitelist
    {
        public IContentPack? SourceContentPack { get; }
        public string ContentID { get; set; }
        public string Source { get; set; }
        public string UpperDancerName { get; set; }
        public string LowerDancerName { get; set; }
        public bool IsPositionStrict { get; set; }
        public bool HasConflicts { get; set; }
        public bool Enabled { get; set; }
        public NPC UpperDancer { get => Game1.getCharacterFromName(UpperDancerName); }
        public NPC LowerDancer { get => Game1.getCharacterFromName(LowerDancerName); }
        public Spring24PairWhitelist(Spring24PairWhitelist pairWhitelist)
        {
            this.Source = pairWhitelist.Source;
            if (pairWhitelist.Source.Equals("ContentPack") && pairWhitelist.SourceContentPack != null)
            {
                this.SourceContentPack = pairWhitelist.SourceContentPack;
                this.ContentID = $"{pairWhitelist.SourceContentPack.Manifest.UniqueID}_{pairWhitelist.SourceContentPack.Manifest.Version.ToString()}_Spring24CharacterBlacklist_{UpperDancerName}_{LowerDancerName}";
            }
            else { this.ContentID = $"{pairWhitelist.Source}_{DateTime.Now.ToString()}_Spring24CharacterBlacklist_{UpperDancerName}_{LowerDancerName}"; }
            this.UpperDancerName = pairWhitelist.UpperDancerName;
            this.LowerDancerName = pairWhitelist.LowerDancerName;
            this.IsPositionStrict = pairWhitelist.IsPositionStrict;
            this.HasConflicts = pairWhitelist.HasConflicts;
            this.Enabled = pairWhitelist.Enabled;
        }
        public Spring24PairWhitelist(string source, string upperDancerName, string lowerDancerName, bool isPositionStrict)
        {
            ContentID = $"{source}_{DateTime.Now.ToString()}_Spring24CharacterBlacklist_{upperDancerName}_{lowerDancerName}";
            Source = source;
            UpperDancerName = upperDancerName;
            LowerDancerName = lowerDancerName;
            IsPositionStrict = isPositionStrict;
            HasConflicts = false;
            Enabled = true;
        }
        public Spring24PairWhitelist (ModData.Content content)
        {
            if(content.ContentType == "Spring24CharacterWhitelist" 
                && content.Fields.Spring24PairWhitelist != null
                && content.Fields.Spring24PairWhitelist.UpperDancerName != null
                && content.Fields.Spring24PairWhitelist.LowerDancerName != null)
            {
                this.Source = "ContentPack";
                this.SourceContentPack = content.ContentPack;
                this.ContentID = $"{content.ContentPack.Manifest.UniqueID}_{content.ContentPack.Manifest.Version.ToString()}_Spring24CharacterBlacklist_{UpperDancerName}_{LowerDancerName}";
                this.UpperDancerName = content.Fields.Spring24PairWhitelist.UpperDancerName;
                this.LowerDancerName = content.Fields.Spring24PairWhitelist.LowerDancerName;
                this.IsPositionStrict = content.Fields.Spring24PairWhitelist.IsPositionStrict;
                this.HasConflicts = false;
                this.Enabled = true;
            }
            else { throw new ArgumentNullException(); }
        }
    }
    public class Spring24PairBlacklist
    {
        public IContentPack? SourceContentPack { get; }
        public string ContentID { get; set; }
        public string Source { get; set; }
        public string UpperDancerName { get; set; }
        public string LowerDancerName { get; set; }
        public bool IsPositionStrict { get; set; }
        public bool HasConflicts { get; set; }
        public bool Enabled { get; set; }
        public NPC UpperDancer { get => Game1.getCharacterFromName(UpperDancerName); }
        public NPC LowerDancer { get => Game1.getCharacterFromName (LowerDancerName); }
        public Spring24PairBlacklist (Spring24PairBlacklist pairBlacklist)
        {
            this.Source = pairBlacklist.Source;
            if (pairBlacklist.Source.Equals("ContentPack") && pairBlacklist.SourceContentPack != null)
            {
                this.SourceContentPack = pairBlacklist.SourceContentPack;
                this.ContentID = $"{pairBlacklist.SourceContentPack.Manifest.UniqueID}_{pairBlacklist.SourceContentPack.Manifest.Version.ToString()}_Spring24CharacterBlacklist_{UpperDancerName}_{LowerDancerName}";
            }
            else { this.ContentID = $"{pairBlacklist.Source}_{DateTime.Now.ToString()}_Spring24CharacterBlacklist_{UpperDancerName}_{LowerDancerName}"; }
            this.UpperDancerName = pairBlacklist.UpperDancerName ;
            this.LowerDancerName = pairBlacklist.LowerDancerName ;
            this.IsPositionStrict = pairBlacklist.IsPositionStrict ;
            this.HasConflicts = pairBlacklist.HasConflicts ;
            this.Enabled = pairBlacklist.Enabled ;
        }
        public Spring24PairBlacklist(string source, string upperDancerName, string lowerDancerName, bool isPositionStrict)
        {
            this.ContentID = $"{source}_{DateTime.Now.ToString()}_Spring24CharacterBlacklist_{upperDancerName}_{lowerDancerName}";
            this.Source = source;
            this.UpperDancerName = upperDancerName;
            this.LowerDancerName = lowerDancerName;
            this.IsPositionStrict = isPositionStrict;
            this.HasConflicts = false;
            this.Enabled = true;
        }
        public Spring24PairBlacklist(Content content)
        {
            if (content.ContentType == "Spring24PairBlacklist" 
                && content.Fields.Spring24PairBlacklist != null
                && content.Fields.Spring24PairBlacklist.UpperDancerName != null
                && content.Fields.Spring24PairBlacklist.LowerDancerName != null)
            {
                this.Source = "ContentPack";
                this.SourceContentPack = content.ContentPack;
                this.ContentID = $"{content.ContentPack.Manifest.UniqueID}_{content.ContentPack.Manifest.Version.ToString()}_Spring24CharacterBlacklist_{UpperDancerName}_{LowerDancerName}";
                this.UpperDancerName = content.Fields.Spring24PairBlacklist.UpperDancerName;
                this.LowerDancerName = content.Fields.Spring24PairBlacklist.LowerDancerName;
                this.IsPositionStrict = content.Fields.Spring24PairBlacklist.IsPositionStrict;
                this.HasConflicts = false;
                this.Enabled = true;
            }
            else { throw new ArgumentNullException(); }
        }
    }
    public class Spring24CharacterBlacklist
    {
        public IContentPack? SourceContentPack { get; }
        public string ContentID { get; set; }
        public string Source { get; set; }
        public string CharacterName { get; set; }
        public bool HasConflicts { get; set; }
        public bool Enabled { get; set; }
        public NPC Character { get => Game1.getCharacterFromName(CharacterName); }
        public Spring24CharacterBlacklist(Spring24CharacterBlacklist charBlacklist)
        {
            if (charBlacklist.Source.Equals("ContentPack") && charBlacklist.SourceContentPack != null)
            {
                this.SourceContentPack = charBlacklist.SourceContentPack;
                this.ContentID = $"{charBlacklist.SourceContentPack.Manifest.UniqueID}_{charBlacklist.SourceContentPack.Manifest.Version.ToString()}_Spring24CharacterBlacklist_{CharacterName}";
            }
            else { this.ContentID = $"{charBlacklist.Source}_{DateTime.Now.ToString()}_Spring24CharacterBlacklist_{CharacterName}"; }
            this.Source = charBlacklist.Source;
            this.CharacterName = charBlacklist.CharacterName;
            this.HasConflicts = charBlacklist.HasConflicts;
            this.Enabled = charBlacklist.Enabled;
        }
        public Spring24CharacterBlacklist(string source, string characterName)
        {
            ContentID = $"{source}_{DateTime.Now.ToString()}_Spring24CharacterBlacklist_{characterName}";
            Source = source;
            CharacterName = characterName;
            HasConflicts = false;
            Enabled = true;
        }
        public Spring24CharacterBlacklist(Content content)
        {
            if (content.ContentType == "Spring24CharacterBlacklist"
                && content.Fields.Spring24CharacterBlacklist != null
                && content.Fields.Spring24CharacterBlacklist.CharacterName != null)
            {
                this.Source = "ContentPack";
                this.SourceContentPack = content.ContentPack;
                this.ContentID = $"{content.ContentPack.Manifest.UniqueID}_{content.ContentPack.Manifest.Version.ToString()}_Spring24CharacterBlacklist_{CharacterName}";
                this.CharacterName = content.Fields.Spring24CharacterBlacklist.CharacterName;
                this.HasConflicts = false;
                this.Enabled = true;
            }
            else { throw new ArgumentNullException(); }
        }
    }
    public class Spring24Sprite
    {
        public IContentPack SourceContentPack { get; }
        public string Source { get; set; }
        public string ContentID { get; set; }
        public string SpritesheetName { get; set; }
        public string SpritesheetPath { get; set; }
        public bool SpriteEnabled { get; set; }
        public string CharacterName { get; set; }
        public NPC Character { get => Game1.getCharacterFromName(CharacterName);}
        public Spring24Sprite(Spring24Sprite sprite)
        {

            this.SourceContentPack = sprite.SourceContentPack;
            this.ContentID = $"{sprite.Source}_{sprite.SourceContentPack.Manifest.UniqueID}_{sprite.SourceContentPack.Manifest.Version.ToString()}_Spring24Sprite_{CharacterName}";
            this.SpritesheetName = sprite.SpritesheetName;
            this.SpritesheetPath = sprite.SpritesheetPath;
            this.SpriteEnabled = sprite.SpriteEnabled;
            this.CharacterName = sprite.CharacterName;
        }
        public Spring24Sprite(Content content)
        {
            if (content.ContentType == "Spring24Sprite"
                && content.Fields.Spring24Sprite != null
                && content.Fields.Spring24Sprite.SpritesheetPath != null
                && content.Fields.Spring24Sprite.CharacterName != null)
            {
                this.Source = "ContentPack";
                this.ContentID = $"{content.ContentPack.Manifest.UniqueID}_{content.ContentPack.Manifest.Version.ToString()}_Spring24Sprite_{CharacterName}";
                this.SourceContentPack = content.ContentPack;
                this.SpritesheetName = content.Fields.Spring24Sprite.SpritesheetName;
                this.SpritesheetPath = content.Fields.Spring24Sprite.SpritesheetPath;
                this.CharacterName = content.Fields.Spring24Sprite.CharacterName;
            }
            else { throw new ArgumentNullException(); }
        }
    }

    //old methods
    internal class ModDataSpring24
    {
        public static IMonitor Monitor;
        public static IModHelper Helper;
        public static ModConfig Config;

        public static void Initialize(IMonitor monitor, ModConfig config, IModHelper helper)
        {
            Monitor = monitor;
            Helper = helper;
            Config = config;
        }
        public static void updateModDataSpring24PairBlackList(List<NPC> actors)
        {
            string[] pairBlackList = Config.CNFAspring24PairBlacklist.Split(", ");

            Lookup<string, string> pairBlackListLookup = (Lookup<string, string>)pairBlackList.Select(pair => pair.Split(" & ")).Where(parts => parts.Length == 2).ToLookup(parts => parts[0], parts => parts[1]);
            foreach (NPC target in actors)
            {
                if (pairBlackListLookup.Contains(target.Name))
                {
                    foreach (string blackListedChar in pairBlackListLookup[target.Name])
                    {
                        if (!ModDataGeneric.tryGetModData(target, "elfuun.CustomNPCFestivalAdditions/Spring24PairBlackList").Split(", ").Contains($"{blackListedChar}"))
                        {
                            if (target.modData["elfuun.CustomNPCFestivalAdditions/Spring24PairBlackList"].Equals(""))
                            {
                                target.modData[$"elfuun.CustomNPCFestivalAdditions/Spring24PairBlackList"] = $"{blackListedChar}";
                            }
                            else
                            {
                                target.modData[$"elfuun.CustomNPCFestivalAdditions/Spring24PairBlackList"] += $", {blackListedChar}";
                            }
                            Monitor.Log($"Appending new blacklisted pair member to {target.Name}'s ModData: {blackListedChar}");
                        }
                    }
                }
                if (!ModData.ModDataGeneric.tryGetModData(target, "elfuun.CustomNPCFestivalAdditions/Spring24PairBlackList").Equals(""))
                {
                    Monitor.Log($"{target.Name} will not dance with the following NPCs, per blacklist: {ModDataGeneric.tryGetModData(target, "elfuun.CustomNPCFestivalAdditions/Spring24PairBlackList")}");
                }
                continue;
            }
        }

        //Mod data reset
        public static void clearSpring24CharBlacklist(NPC target)
        {
            target.modData.Remove("elfuun.CustomNPCFestivalAdditions/isSpring24BlackListed");
        }
        public static void clearSpring24PairBlacklist(NPC target)
        {
            target.modData.Remove("elfuun.CustomNPCFestivalAdditions/Spring24PairBlackList");
        }
        public static void clearSpring24CustomSprites(NPC target)
        {
            target.modData.Remove("elfuun.CustomNPCFestivalAdditions/hasSpring24Sprite");
        }
        public static void clearAllCNFASpring24(NPC target)
        {
            clearSpring24CharBlacklist(target);
            clearSpring24PairBlacklist(target);
            clearSpring24CustomSprites(target);
        }
    }
}
