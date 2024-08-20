using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ImprovedReforging
{
    public class ReforgeItem : GlobalItem
    {
        public override bool ReforgePrice(Item item, ref int reforgePrice, ref bool canApplyDiscount)
        {
            reforgePrice = (int)(reforgePrice * (ModContent.GetInstance<ImprovedReforgingConfig>().ReforgePricePercentage * 0.01)); //applies config price if tinkerer rework is off
            return base.ReforgePrice(item, ref reforgePrice, ref canApplyDiscount);
        }
        public override void PostReforge(Item item) //if tinkerer rework is off put prefix rework is on, then this applies the changed prefixes after a vanilla reforge
        {
            base.PostReforge(item);
            if (ModContent.GetInstance<ImprovedReforgingConfig>().PrefixRework)
            {
                int pre = item.prefix;
                item.ResetPrefix();

                PrefixChanges.MyPrefix(pre, item);
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(item, tooltips);
            if (ModContent.GetInstance<ImprovedReforgingConfig>().PrefixRework)
            {
                if (item.prefix == 4) //savage, changed to have a melee speed (but not use time) buff. if i give this to another prefix, i need to put it here
                {
                    foreach (TooltipLine tip in tooltips)
                    {
                        if (tip.IsModifier && tip.Text.EndsWith("% speed"))
                        {
                            tip.Text = tip.Text.Substring(0, tip.Text.Length - 7) + Lang.tip[47].Value; //get rid of last 7 chars (% speed), and replace with melee speed
                        }
                    }
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
                    TooltipLine line = new TooltipLine(Mod, "PrefixAccuracy", s);
                    line.IsModifier = true;
                    if (accuracyMultiplier > 1f)
                        line.IsModifierBad = true;
                    String[] lineNames = { "PrefixDamage", "PrefixSpeed", "PrefixCritChance", "PrefixUseMana", "PrefixSize", "PrefixShootSpeed", "PrefixKnockback" };
                    for (int i = tooltips.Count - 1; i >= 0; i--)
                    {
                        if (lineNames.Contains(tooltips.ElementAt(i).Name))
                        {
                            tooltips.Insert(i + 1, line);
                            break;
                        }
                    }
                }*/
            }           
        }
    }
}
