using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewModdingAPI;
using StardewValley.Network;
using Microsoft.Xna.Framework.Graphics;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using StardewValley.Objects;
using StardewValley.Mods;
using StardewModdingAPI.Events;
using StardewValley.Locations;


namespace CustomNPCFestivalAdditions.Spring24
{
    internal class DanceGenerator
    {
        public static IMonitor Monitor;
        public static IModHelper Helper;


        public static void Initialize(IMonitor monitor, IModHelper helper)
        {
            Monitor = monitor;
            Helper = helper;
        }
        public static string BuildChangeSpriteBlock(List<NetDancePartner> upperLine, List<NetDancePartner> lowerLine)
        {

            StringBuilder eventChangeSprite = new StringBuilder();
            foreach (NetDancePartner upper in upperLine)
            {
                if (FlowerDancePatch.hasCNFASpring24Sprite(upper))
                {
                    eventChangeSprite.Append($"/changeSprite {upper.TryGetVillager().Name} CNFASpring24");
                }
            }
            foreach (NetDancePartner lower in lowerLine)
            {
                if (FlowerDancePatch.hasCNFASpring24Sprite(lower))
                {
                    eventChangeSprite.Append($"/changeSprite {lower.TryGetVillager().Name} CNFASpring24");
                }
            }
            return eventChangeSprite.ToString();
        }

        /// <summary>
        /// Builds a large block of simultaneous warps for the start of the event template, based on the number of items in inputted list.
        /// </summary>
        /// <param name="upperLineWarp">A list of NetDancePartners to dance in the upper dance line.</param>
        /// <returns>Warp block for the Spring 24 Festival Event template.</returns>
        public static string BuildEventWarpBlock(List<NetDancePartner> upperLineWarp)
        {

            /* There will eventually be some code here straightening out issues between custom spectator animations
                * that involve NPCs you can dance with
                * it'll be messy, ugh I'm not looking forward to it
                */

            int n = upperLineWarp.Count;
            int q = n % 2;

            StringBuilder eventWarpDancer = new StringBuilder();

            //I gave up on using math to get x coordinate, so here's some arrays instead lmao

            switch (q)
            {
                case 0:

                    int counti = 1;

                    int[] even = { 0, 13, 15, 11, 17, 9, 19, 7, 21, 6, 22, 8, 20, 10, 18, 12, 16 };

                    while (counti <= 16 && counti <= n)
                    {
                        eventWarpDancer.Append($"/warp Girl{counti} {even[counti]} 24 false");
                        eventWarpDancer.Append($"/warp Guy{counti} {even[counti]} 27 false");
                        counti++;
                    }

                    break;

                case 1:

                    int countj = 1;

                    int[] odd = { 0, 14, 16, 12, 18, 10, 20, 8, 22, 6, 21, 7, 19, 9, 17, 11, 15, 13 };

                    while (countj <= 17 && countj <= n)
                    {
                        eventWarpDancer.Append($"/warp Girl{countj} {odd[countj]} 24 false");
                        eventWarpDancer.Append($"/warp Guy{countj} {odd[countj]} 27 false");
                        countj++;
                    }
                    break;
            }
            return eventWarpDancer.ToString();
        }
        /// <summary>
        /// Builds a large block of simultaneous "show frame" commands for the start of the event template, based on the number of items in inputted list.
        /// </summary>
        /// <param name="upperLineShowFrame">A list of NetDancePartners to dance in the upper dance line.</param>
        /// <returns>"Show Frame" block for the Spring 24 Festival Event template.</returns>
        public static string BuildShowFrameBlock(List<NetDancePartner> upperLineShowFrame)
        {

            int n = upperLineShowFrame.Count();
            int count = 1;

            StringBuilder eventShowFrame = new StringBuilder();

            eventShowFrame.Append("/beginSimultaneousCommand");
            while (count <= n)
            {
                eventShowFrame.Append($"/showFrame Girl{count} 40");
                eventShowFrame.Append($"/showFrame Guy{count} 44");
                count++;
            }
            eventShowFrame.Append("/endSimultaneousCommand");

            return eventShowFrame.ToString();
        }
        /// <summary>
        /// Builds a large block of simultaneous animation commands for the first "dance move" of the event template, based on the number of items in inputted list.
        /// </summary>
        /// <param name="upperLineAnimate1">A list of NetDancePartners to dance in the upper dance line.</param>
        /// <returns>Animation block for the Spring 24 Festival Event template.</returns>
        public static string BuildAnimateBlock1(List<NetDancePartner> upperLineAnimate1)
        {
            int n = upperLineAnimate1.Count();
            int count = 1;

            StringBuilder eventAnimate1 = new StringBuilder();

            eventAnimate1.Append("/beginSimultaneousCommand");
            while (count <= n)
            {
                eventAnimate1.Append($"/animate Girl{count} false true 600 43 41 43 42");
                eventAnimate1.Append($"/animate Guy{count} false true 600 44 45");

                count++;
            }
            eventAnimate1.Append("/endSimultaneousCommand");

            return eventAnimate1.ToString();
        }
        /// <summary>
        /// Builds a large block of simultaneous animation commands for the second "dance move" of the event template, based on the number of items in inputted list.
        /// </summary>
        /// <param name="upperLineAnimate2">A list of NetDancePartners to dance in the upper dance line.</param>
        /// <returns>Animation block 2 for the Spring 24 Festival Event template.</returns>
        public static string BuildAnimateBlock2(List<NetDancePartner> upperLineAnimate2)
        {
            int n = upperLineAnimate2.Count();
            int count = 1;

            StringBuilder eventAnimate2 = new StringBuilder();

            eventAnimate2.Append("/beginSimultaneousCommand");
            while (count <= n)
            {
                eventAnimate2.Append($"/animate Girl{count} false true 600 46 47");
                eventAnimate2.Append($"/animate Guy{count} false true 600 46 47");

                count++;
            }
            eventAnimate2.Append("/endSimultaneousCommand");

            return eventAnimate2.ToString();
        }
        /// <summary>
        /// Builds a large block of simultaneous animation commands for the third "dance move" of the event template, based on the number of items in inputted list.
        /// </summary>
        /// <param name="upperLineAnimate3">A list of NetDancePartners to dance in the upper dance line.</param>
        /// <returns>Animation block 3 for the Spring 24 Festival Event template.</returns>
        public static string BuildAnimateBlock3(List<NetDancePartner> upperLineAnimate3)
        {
            int n = upperLineAnimate3.Count();
            int count = 1;

            StringBuilder eventAnimate3 = new StringBuilder();

            eventAnimate3.Append("/beginSimultaneousCommand");
            while (count <= n)
            {
                eventAnimate3.Append($"/animate Girl{count} false true 600 43 41 43 42");
                eventAnimate3.Append($"/animate Guy{count} false true 600 44 45");

                count++;
            }
            eventAnimate3.Append("/endSimultaneousCommand");

            return eventAnimate3.ToString();
        }

        /// <summary>
        /// Builds a large block of simultaneous event commands to halt all animation, based on the number of items in inputted list.
        /// </summary>
        /// <param name="upperLineAnimateStop">A list of NetDancePartners to dance in the upper dance line.</param>
        /// <returns>Stop Animation block for the Spring 24 Festival Event template.</returns>
        public static string BuildStopAnimationBlock(List<NetDancePartner> upperLineAnimateStop)
        {
            int n = upperLineAnimateStop.Count();
            int count = 1;

            StringBuilder eventStopAnimation = new StringBuilder();

            eventStopAnimation.Append("/beginSimultaneousCommand");
            while (count <= n)
            {
                eventStopAnimation.Append($"/stopAnimation Girl{count} 40");
                eventStopAnimation.Append($"/stopAnimation Guy{count} 44");
                count++;
            }
            eventStopAnimation.Append("/endSimultaneousCommand");

            return eventStopAnimation.ToString();
        }

        /// <summary>
        /// Builds a large block of simultaneous event commands to move all lower line dancers up, based on the number of items in inputted list.
        /// </summary>
        /// <param name="upperLineOffset">A list of NetDancePartners to dance in the upper dance line.</param>
        /// <returns>Offset command block for the Spring 24 Festival Event template.</returns>
        public static string BuildOffsetBlock(List<NetDancePartner> upperLineOffset)
        {
            int n = upperLineOffset.Count();
            int count = 1;

            StringBuilder eventOffset = new StringBuilder();

            eventOffset.Append("/beginSimultaneousCommand");
            while (count <= n)
            {
                eventOffset.Append($"/positionOffset Guy{count} 0 -2");
                count++;
            }
            eventOffset.Append("/endSimultaneousCommand");

            return eventOffset.ToString();
        }

        public static string BuildGiantOffsetBlock(List<NetDancePartner> upperLineOffsetGiant)
        {
            string offsetBlock = BuildOffsetBlock(upperLineOffsetGiant);

            StringBuilder eventOffsetGiant = new StringBuilder();
            for (int z = 0; z < 28; z++)
            {
                eventOffsetGiant.Append(offsetBlock);
                eventOffsetGiant.Append("/pause 300");
            }
            eventOffsetGiant.Append(offsetBlock);

            return eventOffsetGiant.ToString();
        }

        public static string BuildShowFrameBlock2(List<NetDancePartner> upperLineShowFrame)
        {

            int n = upperLineShowFrame.Count();
            int count = 1;

            StringBuilder eventShowFrame = new StringBuilder();

            eventShowFrame.Append("/beginSimultaneousCommand");
            while (count <= n)
            {
                eventShowFrame.Append($"/showFrame Girl{count} 46");
                eventShowFrame.Append($"/showFrame Guy{count} 44");
                count++;
            }
            eventShowFrame.Append("/endSimultaneousCommand");

            return eventShowFrame.ToString();
        }




    }  
}
    
