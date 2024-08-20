using ImprovedReforging.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace ImprovedReforging
{
    [Autoload(Side = ModSide.Client)]
    public class ReforgeModSystem : ModSystem
	{
        internal ReforgeState reforgeState;
        private UserInterface ui;
        private bool oldRework;
        public override void OnWorldLoad() //prefixes are one of the many things reset when loading a world, and this reset uses the vanilla prefix method.        
        {                                  //Here, I'm going through every item and using my prefix method on it to override this with my changed stats
            base.OnWorldLoad();
            oldRework = ModContent.GetInstance<ImprovedReforgingConfig>().PrefixRework;
            if (ModContent.GetInstance<ImprovedReforgingConfig>().PrefixRework) //skip all of this you turn the prefix rework off in the config
                RefreshPrefixes();
            
        }
        public override void PostUpdateItems() //if you change the config, refresh the prefixes
        {
            base.PostUpdateItems();
            if(oldRework != ModContent.GetInstance<ImprovedReforgingConfig>().PrefixRework)
            {
                RefreshPrefixes();
                oldRework = ModContent.GetInstance<ImprovedReforgingConfig>().PrefixRework;
            }
        }
        private void RefreshPrefixes()
        {
            bool rework = ModContent.GetInstance<ImprovedReforgingConfig>().PrefixRework;
            foreach (Item item in Main.ActiveItems)
            {
                if (item.prefix != 0)
                {
                    int pre = item.prefix;
                    item.ResetPrefix();
                    if (rework)
                        PrefixChanges.MyPrefix(pre, item);
                    else
                        item.Prefix(pre);
                }
            }
            for (int i = 0; i < Main.player.Length; i++) //items in a players inventory (or piggy bank, safe, defenders forge, and void bag)
            {
                Player player = Main.player[i];
                foreach (Item item in player.inventory)
                {
                    if (item.prefix != 0)
                    {
                        int pre = item.prefix;
                        item.ResetPrefix();
                        if (rework)
                            PrefixChanges.MyPrefix(pre, item);
                        else
                            item.Prefix(pre);
                    }
                }
                foreach (Item item in player.bank.item)
                {
                    if (item.prefix != 0)
                    {
                        int pre = item.prefix;
                        item.ResetPrefix();
                        if (rework)
                            PrefixChanges.MyPrefix(pre, item);
                        else
                            item.Prefix(pre);
                    }
                }
                foreach (Item item in player.bank2.item)
                {
                    if (item.prefix != 0)
                    {
                        int pre = item.prefix;
                        item.ResetPrefix();
                        if (rework)
                            PrefixChanges.MyPrefix(pre, item);
                        else
                            item.Prefix(pre);
                    }
                }
                foreach (Item item in player.bank3.item)
                {
                    if (item.prefix != 0)
                    {
                        int pre = item.prefix;
                        item.ResetPrefix();
                        if (rework)
                            PrefixChanges.MyPrefix(pre, item);
                        else
                            item.Prefix(pre);
                    }
                }
                foreach (Item item in player.bank4.item)
                {
                    if (item.prefix != 0)
                    {
                        int pre = item.prefix;
                        item.ResetPrefix();
                        if (rework)
                            PrefixChanges.MyPrefix(pre, item);
                        else
                            item.Prefix(pre);
                    }
                }
            }
            for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++) //every chest in the world
            {
                Chest chest = Main.chest[chestIndex];
                if (chest == null)
                {
                    continue;
                }
                for (int inventoryIndex = 0; inventoryIndex < Chest.maxItems; inventoryIndex++)
                {
                    if (chest.item[inventoryIndex].prefix != 0)
                    {
                        int pre = chest.item[inventoryIndex].prefix;
                        chest.item[inventoryIndex].ResetPrefix();
                        if (rework)
                            PrefixChanges.MyPrefix(pre, chest.item[inventoryIndex]);
                        else
                            chest.item[inventoryIndex].Prefix(pre);
                    }
                }
            }
        }
        internal void ShowUI()
        {
            ui?.SetState(reforgeState);
        }

        internal void HideUI()
        {
            ui?.SetState(null);
        }
        public override void Load()
        {
            reforgeState = new ReforgeState();
            reforgeState.Activate();
            ui = new UserInterface();
            ui.SetState(null);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            if (Main.LocalPlayer.talkNPC == -1) //hides ui when not talking to an npc
                HideUI();
            if(ui?.CurrentState == reforgeState) //forces inventory open when reforging
                Main.playerInventory = true;
            ui?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) //required for ui
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "YourMod: A Description",
                    delegate
                    {
                        ui.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
