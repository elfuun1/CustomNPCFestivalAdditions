using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Transactions;

namespace CustomNPCFestivalAdditions.ModData
{

    public class RawCNFAContentPack
    {
        [JsonIgnore]
        public IContentPack? ContentPack { get; set; }
        [JsonPropertyName("Format")]
        public string Format { get; set; }
        [JsonPropertyName("Entries")]
        public List<RawContent> Entries { get; set; }

        private List<RawContent> _entries;

        [JsonConstructor]
        public RawCNFAContentPack(RawCNFAContentPack contentPack)
        {
            this.ContentPack = contentPack?.ContentPack;
            this.Format = contentPack?.Format;
            this.Entries = contentPack?.Entries;
            _entries = contentPack?.Entries;
        }
        public RawCNFAContentPack() 
        {
            this.ContentPack = null;
            this.Format = "";
            this.Entries = new List<RawContent>();
            _entries = new List<RawContent>();
        }
    }

    public class RawContent : IEquatable<RawContent>
    {
        [JsonIgnore]
        public IContentPack? ContentPack { get; set; }
        [JsonPropertyName("ContentType")]
        public string ContentType { get; set; }
        [JsonPropertyName("ContentFields")]
        public Fields ContentFields { get; set; }

        [JsonConstructor]
        public RawContent(RawContent thing)
        {
            this.ContentType = thing?.ContentType;
            this.ContentFields = thing?.ContentFields;
        }

        public bool Equals(RawContent other)
        {
            if (new ContentIDEquality().Equals(this, other))
            { return true; }
            else { return false; }
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public class ContentIDEquality : EqualityComparer<RawContent>
        {
            public override bool Equals(RawContent? x, RawContent? y)
            {
                if (x.ContentPack.Manifest.UniqueID == y.ContentPack.Manifest.UniqueID
                    && x.ContentPack.Manifest.Version == y.ContentPack.Manifest.Version
                    && x.ContentType == y.ContentType
                    && x.ContentFields == y.ContentFields)
                { return true; }
                else { return false; }
            }
            public override int GetHashCode(RawContent content)
            {
                string hCode = "";
                if (content.ContentType == "Spring24CharacterBlacklist" || content.ContentType == "Spring24Sprite")
                {
                    hCode = $"{content.ContentPack.Manifest.UniqueID}_{content.ContentPack.Manifest.Version}_{content.ContentType}_{content.ContentFields.CharacterName}";
                }
                if (content.ContentType.StartsWith("Spring24Pair"))
                {
                    hCode = $"{content.ContentPack.Manifest.UniqueID}_{content.ContentPack.Manifest.Version}_{content.ContentType}_{content.ContentFields.UpperDancerName}_{content.ContentFields.LowerDancerName}";
                }
                return hCode.GetHashCode();
            }
        }
    }
    public class SpritesheetModel
    {
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }
        public int FrameIndex { get; set; }
    }

internal class ModDataGeneric
    {
        public static string tryGetModData(NPC target, string key)
        {
            if (!target.modData.ContainsKey(key))
            {
                target.modData[key] = "";
            }
            return target.modData[key];
        }

        public static void tryAppendModDataValue(NPC target, string key, string addition, string delimiter)
        {
            if (!target.modData.ContainsKey(key) || target.modData[key] == "")
            {
                target.modData[key] = "addition";
            }
            else
            {
                target.modData[key] += (delimiter + addition);
            }
        }
        public static string? tryAccessModDataValue(NPC target, string key)
        {
            if (target.modData.ContainsKey(key))
            {
                return target.modData[key];
            }
            else { return null; };
        }

        public static string[]? tryAccessModDataSubstrings(NPC target, string key, string delimiter)
        {
            if (target.modData.ContainsKey(key))
            {
                return target.modData[key].Split(delimiter);
            }
            else { return null; }
        }
    }
    
    public class Fields
    {
        //Generic Use Properties
        [JsonIgnore]
        public bool? Enabled { get; set; } = true;
        [JsonIgnore]
        public bool? HasConflicts { get; set; } = false;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("CharacterName")]
        public string? CharacterName { get; set; } = null;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("SpritesheetPath")]
        public string? SpritesheetPath { get; set; } = null;

        //Spring24 Specific Properties
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("UpperDancerName")]
        public string? UpperDancerName { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("LowerDancerName")]
        public string? LowerDancerName { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("IsPositionStrict")]
        public bool? IsPositionStrict { get; set; } = null;

        [JsonConstructor]
        public Fields (Fields fields)
        {
            //Generic Use Properties
            this.Enabled = true;
            this.HasConflicts = false;
            
            this.CharacterName = fields?.CharacterName;
            this.SpritesheetPath = fields?.SpritesheetPath;

            //Spring24 Specific Properties
            this.UpperDancerName = fields?.UpperDancerName;
            this.LowerDancerName = fields?.LowerDancerName;
            this.IsPositionStrict = fields?.IsPositionStrict;
        }
    }
}
