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
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;
using Terraria.Localization;
using Terraria.UI.Chat;
using Terraria.GameInput;
using static System.Net.Mime.MediaTypeNames;
using ReLogic.Content;
using Terraria.ModLoader.Config;
using Terraria.ID;
using Humanizer;
using Terraria.ModLoader.IO;
using System.Security.AccessControl;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;

namespace ImprovedReforging.UI
{
	public class ScrollPanel : UIGrid //ui grid hides the parts of the buttons outside of the panel.
	{
        public ReforgeItemSlot itemSlot;
        Item oldItem;
        //UIScrollbar scrollbar;

        public ScrollPanel(ReforgeItemSlot i)
        {
            itemSlot = i;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Item item = itemSlot.item;
            if(item != oldItem) //only when the item changes
            {
                UpdateButtons();
                oldItem = item;
            }
        }
        public override void ScrollWheel(UIScrollWheelEvent evt) //runs when you scroll
        {
            base.ScrollWheel(evt);
            if (Children.First().Top.Pixels != 0 && evt.ScrollWheelValue > 0 || Children.Last().Top.Pixels != 480 && evt.ScrollWheelValue <= 0) //stops you from scolling too far
            {
                foreach (ModifierButton button in Children)
                {
                    button.Top.Pixels += evt.ScrollWheelValue > 0 ? 160 : -160; //scrolls the buttons, 110 is the distance between them
                }
            }
        }
        private void UpdateButtons() //remove all buttons and then re-add them
        {
            Item item = itemSlot.item;
            Player player = Main.LocalPlayer;
            RemoveAllChildren();
            if (item.IsAir)
            {
                return;
            }
            List<Item> PrefixList = GetPrefixList();
            for(int i = 0; i < PrefixList.Count; i++)//create all the buttons
            {
                ModifierButton button = new ModifierButton(PrefixList[i]);
                button.Width.Set(250, 0);
                button.Height.Set(150, 0);
                button.HAlign = (i % 3) * 0.5f; //aligns 3 in each row
                button.Top.Set((i / 3) * 160, 0);//after every 3 buttons moves down 160 pixels to the next row
                button.OnLeftClick += OnButtonClick; //run this method on left click
                long price = (PrefixList[i].value / 3 - item.value / 3) * 10; //reforge price of the prefix you want minus reforge price of the prefix you have, then times 10 so it's not super cheap
                Item item2 = item.Clone();
                item2.ResetPrefix(); //to get the price of an item with no prefix
                if (price < item2.value / 3) //and then make the standard price of an item with no prefix the minimum price
                    price = item2.value / 3;
                if (player.discountEquipped) //discount card
                    price = (int)(price * 0.8f);
                price = (int)Math.Round((double)price * player.currentShoppingSettings.PriceAdjustment); //happiness adjustment
                price = (int)Math.Round((double)price * (ModContent.GetInstance<ImprovedReforgingConfig>().ReforgePricePercentage * 0.01)); //config adujustment
                UIMoneyDisplay moneyDisplay = new UIMoneyDisplay(price); //display the price to the right of the prefix name which we create next
                moneyDisplay.Left.Set(125, 0);
                button.Append(moneyDisplay);
                UIText text = new UIText(Lang.prefix[PrefixList[i].prefix].Value); //this is the prefix name
                text.TextColor = rarityColors[PrefixList[i].rare]; //color based on what the rarity would be with that prefix
                text.HAlign = text.VAlign = 0f;
                button.Append(text);
                List<String> tooltip = GenerateReforgeTooltip(PrefixList[i]); //shows prefix stats for the item
                for (int j = 0; j < tooltip.Count; j++)
                {
                    UIText text2 = new UIText(tooltip[j]);
                    if (text2.Text.StartsWith("+") ^ text2.Text.EndsWith("mana cost")) // ^ is XOR, this flips the color for mana cost
                        text2.TextColor = new Color(126, 158, 133); //pulled these colors out of a screenshot because i couldn't figure out exactly what the color was
                    else
                        text2.TextColor = new Color(146, 108, 103);
                    text2.HAlign = 0f;
                    text2.Top.Set((j + 1) * 23, 0);
                    button.Append(text2);
                }
                Append(button);

            }
        }
        private void OnButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            //long savings = listeningElement.Parent.Children.OfType<UIMoneyDisplay>().First().money; //since this isnt used i can cap it with no worries in UIMoneyDisplay
            long price = listeningElement.Children.OfType<UIMoneyDisplay>().First().money;
            Player player = Main.LocalPlayer;
            if (itemSlot.item.prefix == ((ModifierButton)listeningElement).PrefixItem.prefix) //no wasting money on the same modifier you already have
                return;
            if (player.BuyItem(price)) //true if success
            {
                itemSlot.item = ((ModifierButton)listeningElement).PrefixItem.Clone();
                SoundEngine.PlaySound(SoundID.Item37); //reforge sound
            }
        }
        private List<Item> GetPrefixList() //from the prefix editor in HEROsMod because I couldn't figure out how to do this part myself
        {
            List<Item> validPrefixes = new List<Item>();
            if (itemSlot.item.IsAir)
                return validPrefixes;
            Item item = itemSlot.item.Clone();

            var validPrefixValues = new HashSet<int>();
            int remainingAttempts = 100;
            while (remainingAttempts > 0)
            {
                item.SetDefaults(item.type);
                if (ModContent.GetInstance<ImprovedReforgingConfig>().PrefixRework) //if the prefix rework is enabled in the config
                    PrefixChanges.MyPrefix(-2, item); //prefix rework
                else
                    item.Prefix(-2); //vanillas prefix code
                remainingAttempts--;
                if (item.prefix != 0 && validPrefixValues.Add(item.prefix))
                {
                    remainingAttempts = 100;
                    validPrefixes.Add(item.Clone());
                }
            }

            return (validPrefixes.OrderBy(x => -x.value - x.rare).ToList());
        }
        private static Dictionary<int, Color> rarityColors = new Dictionary<int, Color>()
        {
            {-11, Terraria.ID.Colors.RarityAmber },
            {-1, Terraria.ID.Colors.RarityTrash },
            {0, Color.White },
            {1, Terraria.ID.Colors.RarityBlue },
            {2, Terraria.ID.Colors.RarityGreen },
            {3, Terraria.ID.Colors.RarityOrange },
            {4, Terraria.ID.Colors.RarityRed },
            {5, Terraria.ID.Colors.RarityPink },
            {6, Terraria.ID.Colors.RarityPurple },
            {7, Terraria.ID.Colors.RarityLime },
            {8, Terraria.ID.Colors.RarityYellow },
            {9, Terraria.ID.Colors.RarityCyan },
            {10, new Color(255, 40, 100) },
            {11, new Color(180, 40, 255) },
        };
        public List<String> GenerateReforgeTooltip(Item comparisonItem) //adapted from vanilla code that generates tooltips, Main.MouseText_DrawItemTooltip_GetLinesInfo
        {
            Item item2 = itemSlot.item.Clone();
            item2.ResetPrefix();
            Item item = comparisonItem;
            List<String> toolTipLine = new List<String>(10);
            int numLines = 0;
            if (item2 == null || item2.netID != item.netID)
            {
                item2 = new Item();
                item2.netDefaults(item.netID);
            }
            if (item2.damage != item.damage)
            {
                double num8 = (float)item.damage - (float)item2.damage;
                num8 = num8 / (double)item2.damage * 100.0;
                num8 = Math.Round(num8);
                if (num8 > 0.0)
                    toolTipLine.Add("+" + num8 + Lang.tip[39].Value);
                else
                    toolTipLine.Add(num8 + Lang.tip[39].Value);

                /*if (num8 < 0.0)
                    badPreFixLine[numLines] = true;

                preFixLine[numLines] = true;
                toolTipNames[numLines] = "PrefixDamage";*/
                numLines++;
            }

            if (item2.useAnimation != item.useAnimation)
            {
                String s;
                if (item2.useTime != item.useTime)
                {
                    s = Lang.tip[40].Value; //speed
                }
                else
                {
                    s = Lang.tip[47].Value; //melee speed
                }
                double num9 = (float)item.useAnimation - (float)item2.useAnimation;
                num9 = num9 / (double)item2.useAnimation * 100.0;
                num9 = Math.Round(num9);
                num9 *= -1.0;
                if (num9 > 0.0)
                    toolTipLine.Add("+" + num9 + s);
                else
                    toolTipLine.Add(num9 + s);

                /*if (num9 < 0.0)
                    badPreFixLine[numLines] = true;

                preFixLine[numLines] = true;
                toolTipNames[numLines] = "PrefixSpeed";*/
                numLines++;
            }

            if (item2.crit != item.crit)
            {
                double num10 = (float)item.crit - (float)item2.crit;
                if (num10 > 0.0)
                    toolTipLine.Add("+" + num10 + Lang.tip[41].Value);
                else
                    toolTipLine.Add(num10 + Lang.tip[41].Value);

                /*if (num10 < 0.0)
                    badPreFixLine[numLines] = true;

                preFixLine[numLines] = true;
                toolTipNames[numLines] = "PrefixCritChance";*/
                numLines++;
            }

            if (item2.mana != item.mana)
            {
                double num11 = (float)item.mana - (float)item2.mana;
                num11 = num11 / (double)item2.mana * 100.0;
                num11 = Math.Round(num11);
                if (num11 > 0.0)
                    toolTipLine.Add("+" + num11 + Lang.tip[42].Value);
                else
                    toolTipLine.Add(num11 + Lang.tip[42].Value);

                /*if (num11 > 0.0)
                    badPreFixLine[numLines] = true;

                preFixLine[numLines] = true;
                toolTipNames[numLines] = "PrefixUseMana";*/
                numLines++;
            }

            if (item2.scale != item.scale)
            {
                double num12 = item.scale - item2.scale;
                num12 = num12 / (double)item2.scale * 100.0;
                num12 = Math.Round(num12);
                if (num12 > 0.0)
                    toolTipLine.Add("+" + num12 + Lang.tip[43].Value);
                else
                    toolTipLine.Add(num12 + Lang.tip[43].Value);

                /*if (num12 < 0.0)
                    badPreFixLine[numLines] = true;

                preFixLine[numLines] = true;
                toolTipNames[numLines] = "PrefixSize";*/
                numLines++;
            }

            if (item2.shootSpeed != item.shootSpeed)
            {
                double num13 = item.shootSpeed - item2.shootSpeed;
                num13 = num13 / (double)item2.shootSpeed * 100.0;
                num13 = Math.Round(num13);
                if (num13 > 0.0)
                    toolTipLine.Add("+" + num13 + Lang.tip[44].Value);
                else
                    toolTipLine.Add(num13 + Lang.tip[44].Value);

                /*if (num13 < 0.0)
                    badPreFixLine[numLines] = true;

                preFixLine[numLines] = true;
                toolTipNames[numLines] = "PrefixShootSpeed";*/
                numLines++;
            }
            /*int[] accuracyPrefixes = { 16, 17, 20, 21, 25, 58 }; //for accuracy, which didn't work out
            if (accuracyPrefixes.Contains(item.prefix))
            {
                float accuracyMultiplier = 1f;
                switch (item.prefix)
                {
                    case 16:
                        accuracyMultiplier = 0.5f;
                        break;
                    case 17:
                        accuracyMultiplier = 1.1f;
                        break;
                    case 20:
                        accuracyMultiplier = 0.9f;
                        break;
                    case 21:
                        accuracyMultiplier = 1.1f;
                        break;
                    case 25:
                        accuracyMultiplier = 0.9f;
                        break;
                    case 58:
                        accuracyMultiplier = 1.8f;
                        break;
                    default:
                        break;
                }
                String s = accuracyMultiplier < 1f ? "+" : "-";
                s += (int)(Math.Abs(1 - accuracyMultiplier) * 100f + 0.5f) + "% accuracy";
                toolTipLine.Add(s);
            }*/
            float oldKB = item.knockBack;
            if (item2.knockBack != oldKB) //idk why old knockback is passed in the original vanilla function unlike everything else
            {
                double num14 = oldKB - item2.knockBack;
                num14 = num14 / (double)item2.knockBack * 100.0;
                num14 = Math.Round(num14);
                if (num14 > 0.0)
                    toolTipLine.Add("+" + num14 + Lang.tip[45].Value);
                else
                    toolTipLine.Add(num14 + Lang.tip[45].Value);

                /*if (num14 < 0.0)
                    badPreFixLine[numLines] = true;

                preFixLine[numLines] = true;
                toolTipNames[numLines] = "PrefixKnockback";*/
                numLines++;
            }

            if (item.prefix == 62)
            {
                toolTipLine.Add("+1" + Lang.tip[25].Value);
                //preFixLine[numLines] = true;
                //toolTipNames[numLines] = "PrefixAccDefense";
                numLines++;
            }

            if (item.prefix == 63)
            {
                toolTipLine.Add("+2" + Lang.tip[25].Value);
                //preFixLine[numLines] = true;
                //toolTipNames[numLines] = "PrefixAccDefense";
                numLines++;
            }

            if (item.prefix == 64)
            {
                toolTipLine.Add("+3" + Lang.tip[25].Value);
                //preFixLine[numLines] = true;
                //toolTipNames[numLines] = "PrefixAccDefense";
                numLines++;
            }

            if (item.prefix == 65)
            {
                toolTipLine.Add("+4" + Lang.tip[25].Value);
                //preFixLine[numLines] = true;
                //toolTipNames[numLines] = "PrefixAccDefense";
                numLines++;
            }

            if (item.prefix == 66)
            {
                toolTipLine.Add("+20 " + Lang.tip[31].Value);
                //preFixLine[numLines] = true;
                //toolTipNames[numLines] = "PrefixAccMaxMana";
                numLines++;
            }

            if (item.prefix == 67)
            {
                toolTipLine.Add("+2" + Lang.tip[5].Value);
                //preFixLine[numLines] = true;
                //toolTipNames[numLines] = "PrefixAccCritChance";
                numLines++;
            }

            if (item.prefix == 68)
            {
                toolTipLine.Add("+4" + Lang.tip[5].Value);
                //preFixLine[numLines] = true;
                //toolTipNames[numLines] = "PrefixAccCritChance";
                numLines++;
            }

            if (item.prefix == 69)
            {
                toolTipLine.Add("+1" + Lang.tip[39].Value);
                //preFixLine[numLines] = true;
                //toolTipNames[numLines] = "PrefixAccDamage";
                numLines++;
            }

            if (item.prefix == 70)
            {
                toolTipLine.Add("+2" + Lang.tip[39].Value);
                //preFixLine[numLines] = true;
                //toolTipNames[numLines] = "PrefixAccDamage";
                numLines++;
            }

            if (item.prefix == 71)
            {
                toolTipLine.Add("+3" + Lang.tip[39].Value);
                //preFixLine[numLines] = true;
                //toolTipNames[numLines] = "PrefixAccDamage";
                numLines++;
            }

            if (item.prefix == 72)
            {
                toolTipLine.Add("+4" + Lang.tip[39].Value);
                //preFixLine[numLines] = true;
                //toolTipNames[numLines] = "PrefixAccDamage";
                numLines++;
            }

            if (item.prefix == 73)
            {
                toolTipLine.Add("+1" + Lang.tip[46].Value);
                //preFixLine[numLines] = true;
                //toolTipNames[numLines] = "PrefixAccMoveSpeed";
                numLines++;
            }

            if (item.prefix == 74)
            {
                toolTipLine.Add("+2" + Lang.tip[46].Value);
                //preFixLine[numLines] = true;
                //toolTipNames[numLines] = "PrefixAccMoveSpeed";
                numLines++;
            }

            if (item.prefix == 75)
            {
                toolTipLine.Add("+3" + Lang.tip[46].Value);
                //preFixLine[numLines] = true;
                //toolTipNames[numLines] = "PrefixAccMoveSpeed";
                numLines++;
            }

            if (item.prefix == 76)
            {
                toolTipLine.Add("+4" + Lang.tip[46].Value);
                //preFixLine[numLines] = true;
                //toolTipNames[numLines] = "PrefixAccMoveSpeed";
                numLines++;
            }

            if (item.prefix == 77)
            {
                toolTipLine.Add("+1" + Lang.tip[47].Value);
                //preFixLine[numLines] = true;
                //toolTipNames[numLines] = "PrefixAccMeleeSpeed";
                numLines++;
            }

            if (item.prefix == 78)
            {
                toolTipLine.Add("+2" + Lang.tip[47].Value);
                //preFixLine[numLines] = true;
                //toolTipNames[numLines] = "PrefixAccMeleeSpeed";
                numLines++;
            }

            if (item.prefix == 79)
            {
                toolTipLine.Add("+3" + Lang.tip[47].Value);
                //preFixLine[numLines] = true;
                //toolTipNames[numLines] = "PrefixAccMeleeSpeed";
                numLines++;
            }

            if (item.prefix == 80)
            {
                toolTipLine.Add("+4" + Lang.tip[47].Value);
                //preFixLine[numLines] = true;
                //toolTipNames[numLines] = "PrefixAccMeleeSpeed";
                numLines++;
            }

            //prefixlineIndex = numLines;
            return toolTipLine;
        }
    }
}
