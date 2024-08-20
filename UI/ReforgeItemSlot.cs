using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.Localization;
using Terraria.UI.Chat;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.DataStructures;
using ReLogic.Graphics;

namespace ImprovedReforging.UI
{
    public class ReforgeItemSlot : UIPanel
    {
        public Texture2D backgroundTexture;
        internal float scale;
        public Item item;
        internal event Func<bool> CanClick;

        public ReforgeItemSlot(float scale = 1f)
        {
            this.scale = scale;
            item = new Item();
            backgroundTexture = TextureAssets.InventoryBack9.Value; //took me way to long to figure out where this texture was
            Width.Set(backgroundTexture.Width * scale, 0f);
            Height.Set(backgroundTexture.Height * scale, 0f);
        }

        public override void LeftMouseDown(UIMouseEvent evt)
        {
            Player player = Main.LocalPlayer;
            if (player.itemAnimation == 0 && player.itemTime == 0) //not using an item
            {
                if (CanClick?.Invoke() ?? true)
                {
                    if (Main.mouseItem.CanHavePrefixes() || Main.mouseItem.IsAir) //either putting something valid in, or holding nothing and taking something out
                    {
                        Utils.Swap(ref item, ref Main.mouseItem);
                        if (item.type == ItemID.None || item.stack < 1)
                        {
                            item = new Item();
                        }
                    }
                    if (Main.mouseItem.Equals(item)) //add to stack if the items are the same. not really needed for reforging but its good to keep it just in case
                    {
                        Utils.Swap(ref item.favorited, ref Main.mouseItem.favorited);
                        if (item.stack != item.maxStack && Main.mouseItem.stack != Main.mouseItem.maxStack)
                        {
                            if (Main.mouseItem.stack + item.stack <= Main.mouseItem.maxStack)
                            {
                                item.stack += Main.mouseItem.stack;
                                Main.mouseItem.stack = 0;
                            }
                            else
                            {
                                int giveAmount = Main.mouseItem.maxStack - item.stack;
                                item.stack += giveAmount;
                                Main.mouseItem.stack -= giveAmount;
                            }
                        }
                    }
                    if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
                    {
                        Main.mouseItem = new Item();
                    }

                    if (Main.mouseItem.CanHavePrefixes() || (Main.mouseItem.IsAir && item.type > ItemID.None)) //successful swap, so play sound
                    {
                        Recipe.FindRecipes();
                        SoundEngine.PlaySound(SoundID.Grab);
                    }
                    base.LeftMouseDown(evt);
                }
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch) //this method and everything above it was adapted from autoreforge because im dumb and couldnt figure out how to get this method in particular to work
        {
            Vector2 position = GetInnerDimensions().Position();
            spriteBatch.Draw(backgroundTexture, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            if (item != null && !item.IsAir)
            {
                Texture2D itemTexture;
                Rectangle textureFrame;
                Main.GetItemDrawFrame(item.type, out itemTexture, out textureFrame);

                Color newColor = Color.White;
                float pulseScale = 1f;
                ItemSlot.GetItemLight(ref newColor, ref pulseScale, item, false);
                int height = textureFrame.Height;
                int width = textureFrame.Width;
                float drawScale = 1f;
                float availableWidth = 32; // defaultBackgroundTexture.Width * scale;
                if (width > availableWidth || height > availableWidth)
                {
                    drawScale = availableWidth / (width > height ? width : height);
                }
                drawScale *= scale;
                Vector2 itemPosition = position + backgroundTexture.Size() * scale / 2f - textureFrame.Size() * drawScale / 2f;
                Vector2 itemOrigin = textureFrame.Size() * (pulseScale / 2f - 0.5f);
                if (ItemLoader.PreDrawInInventory(item, spriteBatch, itemPosition, textureFrame, item.GetAlpha(newColor),
                    item.GetColor(Color.White), itemOrigin, drawScale * pulseScale))
                {
                    spriteBatch.Draw(itemTexture, itemPosition, textureFrame, item.GetAlpha(newColor), 0f, itemOrigin, drawScale * pulseScale, SpriteEffects.None, 0f);
                    if (item.color != Color.Transparent)
                    {
                        spriteBatch.Draw(itemTexture, itemPosition, textureFrame, item.GetColor(Color.White), 0f, itemOrigin, drawScale * pulseScale, SpriteEffects.None, 0f);
                    }
                }
                ItemLoader.PostDrawInInventory(item, spriteBatch, itemPosition, textureFrame, item.GetAlpha(newColor),
                    item.GetColor(Color.White), itemOrigin, drawScale * pulseScale);
                if (ItemID.Sets.TrapSigned[item.type])
                {
                    spriteBatch.Draw((Texture2D)TextureAssets.WireUi.GetValue(0), position + new Vector2(40f) * scale, new Rectangle(4, 58, 8, 8), Color.White, 0f, new Vector2(4f), 1f, SpriteEffects.None, 0f);
                }

                if (item.stack > 1)
                {
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, item.stack.ToString(), position + new Vector2(10f, 26f) * scale, Color.White, 0f, Vector2.Zero, new Vector2(scale), -1f, scale);
                }

                if (IsMouseHovering)
                {
                    Main.HoverItem = item.Clone();
                    Main.hoverItemName = Main.HoverItem.Name;
                }
            }
        }
        public override void OnDeactivate()
        {
            base.OnDeactivate();
            if(!item.IsAir)
            {
                if (Main.mouseItem.IsAir) //other menus in terraria dont usually put it in your hand as far as im aware, but i like this better 
                {
                    Utils.Swap(ref item, ref Main.mouseItem);
                    SoundEngine.PlaySound(SoundID.Grab);
                } 
                else //this bit is from HEROsMod because I could not figure out how to put the item in the player's inventory correctly
                {
                    Item item3 = item.Clone();

                    Player player = Main.LocalPlayer;
                    item3.position = player.Center;
                    Item item2 = player.GetItem(player.whoAmI, item3, GetItemSettings.GetItemInDropItemCheck);
                    if (item2.stack > 0)
                    {
                        int num = Item.NewItem(player.GetSource_Misc("PlayerDropItemCheck"), (int)player.position.X, (int)player.position.Y, player.width, player.height, item2.type, item2.stack, false, (int)item.prefix, true, false);
                        Main.item[num].newAndShiny = false;
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            NetMessage.SendData(MessageID.SyncItem, -1, -1, null, num, 1f, 0f, 0f, 0, 0, 0);
                        }
                        else
                        {
                            
                        }

                    }
                    item.TurnToAir();
                }
            }
        }
    }
}
