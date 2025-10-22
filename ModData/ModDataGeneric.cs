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

    public class RawCNFAContentPack : ICollection<RawContent>
    {
        [JsonIgnore]
        public IContentPack? ContentPack { get; set; }
        [JsonPropertyName("Format")]
        public string Format { get; set; }
        [JsonPropertyName("Entries")]
        public List<RawContent> Entries { get; set; }

        private List<RawContent> _entries;

        public RawCNFAContentPack(RawCNFAContentPack contentPack)
        {
            this.ContentPack = contentPack.ContentPack;
            this.Format = contentPack.Format;
            this.Entries = contentPack.Entries;
            _entries = contentPack.Entries;
        }
        public RawCNFAContentPack(RawContent content)
        {
            this.Format = "0.0.0";
            this.Entries = new List<RawContent>();
            Entries.Add(content);
            _entries = Entries;
        }

        //Sets up ICollection and relevant methods
        public RawContent this[int index]
        {
            get { return (RawContent)_entries[index]; }
            set { _entries[index] = value; }
        }
        public bool Contains(RawContent content)
        {
            bool found = false;
            foreach (RawContent ctnt in _entries)
            {
                if (ctnt.Equals(content)) { found = true;}
            }
            return found;
        }
        public void Add(RawContent content)
        {
            if(!Contains(content))
            {
                _entries.Add(content);
            }
        }
        public void Clear()
        {
            _entries.Clear(); 
        }
        public void CopyTo(RawContent[] array, int arrayIndex)
        {
            if (array == null)
            { throw new ArgumentNullException("array"); }
            for (int i = 0; i < _entries.Count; i++)
            {
                array[i + arrayIndex] = _entries[i];
            }
        }
        public int Count
        { get { return _entries.Count; } }

        public bool IsReadOnly { get { return false; } }

        public bool Remove(RawContent item)
        {
            bool result = false;
            for (int i=0; i < _entries.Count; i++)
            {
                RawContent cntnt = (RawContent)_entries[i];
                if(new RawContent.ContentIDEquality().Equals(cntnt, item))
                {
                    _entries.RemoveAt(i);
                    result = true;
                    break;
                }
            }
            return result;
        }
        //sets up IEnumerator and relevant methods
        public IEnumerator<RawContent> GetEnumerator()
        {
            return new ContentEnumerator(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ContentEnumerator(this);
        }
        public class ContentEnumerator : IEnumerator<RawContent>
        {
            private RawCNFAContentPack _contentPack;
            private int curIndex;
            private RawContent curContent;
            public ContentEnumerator(RawCNFAContentPack contentPack)
            {
                _contentPack = contentPack;
                curIndex = -1;
                curContent = default(RawContent);
            }
            public bool MoveNext()
            {
                if(++curIndex >= _contentPack.Count)
                {
                    return false;
                }
                else 
                {
                    curContent = _contentPack[curIndex];
                }
                return true;
            }
            public void Reset() { curIndex = -1;}
            void IDisposable.Dispose() { }
            public RawContent Current
            { get { return curContent; } }
            object IEnumerator.Current { get { return Current; } }

        }

    }

    public class RawContent : IEquatable<RawContent>
    {
        [JsonIgnore]
        public IContentPack? ContentPack { get; }
        [JsonPropertyName("ContentType")]
        public string ContentType { get; set; }
        [JsonPropertyName("ContentFields")]
        public Fields ContentFields { get; set; }

        public RawContent(RawContent thing)
        {
            this.ContentPack = thing.ContentPack;
            this.ContentType = thing.ContentType;
            this.ContentFields = thing.ContentFields;
        }

        public RawContent(Fields testFields)
        {
            this.ContentType = "Manual";
            this.ContentFields = testFields;
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
        public bool? Enabled { get; set; }
        [JsonIgnore]
        public bool? HasConflicts { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("CharacterName")]
        public string? CharacterName { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("SpritesheetPath")]
        public string? SpritesheetPath { get; set; }

        //Spring24 Specific Properties
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("UpperDancerName")]
        public string? UpperDancerName { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("LowerDancerName")]
        public string? LowerDancerName { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("IsPositionStrict")]
        public bool? IsPositionStrict { get; set; }

        public Fields (Fields fields)
        {
            //Generic Use Properties
            this.Enabled = true;
            this.HasConflicts = false;
            this.CharacterName = fields.CharacterName ?? null;
            this.SpritesheetPath = fields.SpritesheetPath ?? null;

            //Spring24 Specific Properties
            this.UpperDancerName = fields.UpperDancerName ?? null;
            this.LowerDancerName = fields.LowerDancerName ?? null;
            this.IsPositionStrict = fields.IsPositionStrict ?? false;
        }
        public Fields (Spring24PairWhitelist whitelist)
        {
            this.Enabled = true;
            this.HasConflicts = false;
            this.UpperDancerName = whitelist.UpperDancerName;
            this.LowerDancerName = whitelist.LowerDancerName;
            this.IsPositionStrict = whitelist.IsPositionStrict;
        }


    }
}
