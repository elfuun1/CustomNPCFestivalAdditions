using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFestivalAdditions
{
    public sealed class ModConfig
    {
        //Flower Festival (Spring 24 event) configs
        public bool EnableCNFAspring24 { get; set; } = true;
        public bool CNFAspring24RandomizeDancersInPairs { get; set; } = true;
        public int CNFASpring24NumberDancerPairs { get; set; } = 6;
        public bool CNFAspring24AllowMixedGenderDanceLines { get; set; }
        //public string CNFAspring24UserDefinedPairs { get; set; } = "";
        public string CNFAspring24NPCBlacklist { get; set; } = "";
        public string CNFAspring24PairBlacklist { get; set; } = "";

        /*
        Scope creep ahead, oh no, don't look!

        //Egg Hunt Festival (Spring 13 event) configs
        public bool DisableCNFAspring13 { get; set; } = false;

        //Desert Festival (Spring 15-17 event) configs
        public bool DisableCNFAspring1517 { get; set; } = false;

        //Luau (Summer 11 Event) configs
        public bool DisableCNFAsummer11 { get; set; } = false;

        //Trout Derby (Summer 20-21 event) configs
        public bool DisableCNFAsummer2021 { get; set; } = false;

        //Moonlight Jellies (Summer 28 event) configs
        public bool DisableCNFAsummer28 { get; set; } = false;

        //Fair (Fall 16 event) configs
        public bool DisableCNFAfall16 { get; set; } = false;

        //Spirits Eve (Fall 28 event) configs
        public bool DisableCNFAfall28 { get; set; } = false;

        //Ice Festival (Winter 8 event) configs
        public bool DisableCNFAwinter8 { get; set; } = false;

        //Squid Fest (Winter 12-13 event) configs
        public bool DisableCNFAwinter1213 { get; set; } = false;

        //Night Market (Winter 15-17 event) configs
        public bool DisableCNFAwinter1517 { get; set; } = false;

        //Winter Star (Winter 25 event) configs
        public bool DisableCNFAwinter25 { get; set; } = false;

        */
    }
}
