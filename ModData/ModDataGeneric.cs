using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CustomNPCFestivalAdditions.ModData
{
    public class RawCNFAContentPack : ICollection<Content>
    {
        public int ContentsIndex;
        public IContentPack ContentPack { get; }
        public string Format { get; set; }
        Content[] Contents { get; set; }

        private List<Content> _contents;

        public RawCNFAContentPack()
        {
            _contents = new List<Content>();
        }

        //Sets up ICollection and relevant methods
        public Content this[int index]
        {
            get { return (Content)_contents[index]; }
            set { _contents[index] = value; }
        }
        public bool Contains(Content content)
        {
            bool found = false;
            foreach (Content ctnt in _contents)
            {
                if (ctnt.Equals(content)) { found = true;}
            }
            return found;
        }
        public void Add(Content content)
        {
            if(!Contains(content))
            {
                _contents.Add(content);
            }
        }
        public void Clear()
        { 
            _contents.Clear(); 
        }
        public void CopyTo(Content[] array, int arrayIndex)
        {
            if (array == null)
            { throw new ArgumentNullException("array"); }
            for (int i = 0; i < _contents.Count; i++)
            {
                array[i + arrayIndex] = _contents[i];
            }
        }
        public int Count
        { get { return _contents.Count; } }

        public bool IsReadOnly { get { return false; } }

        public bool Remove(Content item)
        {
            bool result = false;
            for (int i=0; i < _contents.Count; i++)
            {
                Content cntnt = (Content)_contents[i];
                if(new Content.ContentIDEquality().Equals(cntnt, item))
                { _contents.RemoveAt(i);
                    result = true;
                    break;
                }
            }
            return result;
        }
        //sets up IEnumerator and relevant methods
        public IEnumerator<Content> GetEnumerator()
        {
            return new ContentEnumerator(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ContentEnumerator(this);
        }
        public class ContentEnumerator : IEnumerator<Content>
        {
            private RawCNFAContentPack _contentPack;
            private int curIndex;
            private Content curContent;
            public ContentEnumerator(RawCNFAContentPack contentPack)
            {
                _contentPack = contentPack;
                curIndex = -1;
                curContent = default(Content);
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
            public Content Current
            { get { return curContent; } }
            object IEnumerator.Current { get { return Current; } }

        }

    }

    public class Content : IEquatable<Content>
    {
        public IContentPack ContentPack { get; }
        public virtual string ContentID { get; set; }
        public string ContentType { get; set; }
        public string[] Fields {get; set;}
        public Content(Content thing)
        {
            this.ContentPack = thing.ContentPack;
            this.ContentID = $"{thing.ContentPack.Manifest.UniqueID}/{thing.ContentPack.Manifest.Version.ToString()}/{thing.ContentType}";
            this.ContentType = thing.ContentType;
            this.Fields = thing.Fields;

        }
        public bool Equals(Content other)
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
        public class ContentIDEquality : EqualityComparer<Content>
        {
            public override bool Equals(Content? x, Content? y)
            {
                if (x.ContentID == y.ContentID)
                { return true; }
                else { return false; }
            }
            public override int GetHashCode(Content content)
            {
                string hCode = content.ContentID;
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
}
