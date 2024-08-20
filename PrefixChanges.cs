using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using static System.Net.Mime.MediaTypeNames;

namespace ImprovedReforging
{
    public class PrefixChanges
    {
        public static bool MyPrefix(int prefixWeWant, Item item)
        {
            if (!WorldGen.gen && Main.rand == null)
                Main.rand = new UnifiedRandom();

            if (prefixWeWant == 0)
                return false;

            if (!item.CanHavePrefixes())
                return false;

            if (prefixWeWant > 0 && !item.CanApplyPrefix(prefixWeWant))
                return false;

            //#StackablePrefixWeapons: Stackable items will not spawn with a prefix on craft/generate. Only when deliberately reforged.
            if (prefixWeWant == -1 && item.maxStack > 1)
                return false;

            UnifiedRandom unifiedRandom = (WorldGen.gen ? WorldGen.genRand : Main.rand);

            bool? applyPrefixOverride = ItemLoader.PrefixChance(item, prefixWeWant, unifiedRandom);
            if (applyPrefixOverride is false)
                return false;

            int rolledPrefix = prefixWeWant;
            float dmg = 1f;
            float kb = 1f;
            float spd = 1f;
            float meleeSpd = 1f;
            float size = 1f;
            float shtspd = 1f;
            float mcst = 1f;
            int crt = 0;
            bool flag = true;
            while (flag)
            {
                flag = false;

                if (applyPrefixOverride is true) { }
                else
                if (rolledPrefix == -1 && unifiedRandom.Next(4) == 0)
                    return true; //rolledPrefix = 0;

                if (prefixWeWant < -1)
                    rolledPrefix = -1;

                // Faster check implementation than actually rolling.
                if (prefixWeWant == -3)
                    return PrefixLoader.Roll(item, unifiedRandom, out _, justCheck: true);

                if ((rolledPrefix == -1 || rolledPrefix == -2 || rolledPrefix == -3) && !RollAPrefix(unifiedRandom, ref rolledPrefix, item))
                    return false;

                switch (prefixWeWant)
                {
                    case -3:
                        return true;
                    case -1:
                        if (applyPrefixOverride is true) { }
                        else
                        if (PrefixID.Sets.ReducedNaturalChance[rolledPrefix] && unifiedRandom.Next(3) != 0)
                            return true; //rolledPrefix = 0;
                        break;
                }

                if (!TryGetPrefixStatMultipliersForItem(item, rolledPrefix, out dmg, out kb, out spd, out size, out shtspd, out mcst, out crt, out meleeSpd))
                {
                    flag = true;
                    rolledPrefix = -1;
                }

                if (prefixWeWant == -2 && rolledPrefix == 0)
                {
                    rolledPrefix = -1;
                    flag = true;
                }
            }

            //UndoItemAnimationCompensations(item);

            item.damage = (int)Math.Round((float)item.damage * dmg);
            item.useAnimation = (int)Math.Round((float)item.useAnimation * meleeSpd); //only affects melee swing speed
            item.useAnimation = (int)Math.Round((float)item.useAnimation * spd);
            item.useTime = (int)Math.Round((float)item.useTime * spd);
            item.reuseDelay = (int)Math.Round((float)item.reuseDelay * spd);
            item.mana = (int)Math.Round((float)item.mana * mcst);
            item.knockBack *= kb;
            item.scale *= size;
            item.shootSpeed *= shtspd;
            item.crit += crt;

            if (rolledPrefix >= PrefixID.Count)
                PrefixLoader.GetPrefix(rolledPrefix)?.Apply(item);

            //ApplyItemAnimationCompensationsToVanillaItems(item);

            float num = 1f * ((dmg - 1) * 1.5f + 1) * ((1 / spd - 1) * 1.5f + 1) *
            ((1 / meleeSpd - 1) * 1.25f + 1) * ((1 / mcst - 1) * 0.5f + 1) * ((size - 1) * 0.5f + 1) *
            ((kb - 1) * 0.25f + 1) * ((shtspd - 1) * 0.5f + 1) * (1f + (float)crt * 0.015f);
            if (rolledPrefix == 62 || rolledPrefix == 69 || rolledPrefix == 73 || rolledPrefix == 77)
                num *= 1.05f;

            if (rolledPrefix == 63 || rolledPrefix == 70 || rolledPrefix == 74 || rolledPrefix == 78 || rolledPrefix == 67)
                num *= 1.1f;

            if (rolledPrefix == 64 || rolledPrefix == 71 || rolledPrefix == 75 || rolledPrefix == 79 || rolledPrefix == 66)
                num *= 1.15f;

            if (rolledPrefix == 65 || rolledPrefix == 72 || rolledPrefix == 76 || rolledPrefix == 80 || rolledPrefix == 68)
                num *= 1.2f;

            if (rolledPrefix >= PrefixID.Count)
                PrefixLoader.GetPrefix(rolledPrefix)?.ModifyValue(ref num);

            int baseRarity = item.rare;

            if ((double)num >= 1.2)
                item.rare += 2;
            else if ((double)num >= 1.05)
                item.rare++;
            else if ((double)num <= 0.8)
                item.rare -= 2;
            else if ((double)num <= 0.95)
                item.rare--;

            if (baseRarity >= ItemRarityID.Count)
                item.rare = RarityLoader.GetRarity(baseRarity).GetPrefixedRarity(item.rare - baseRarity, num);
            else if (item.rare > ItemRarityID.Purple)
                item.rare = ItemRarityID.Purple;

            if (item.rare > -11)
            {
                if (item.rare < -1)
                    item.rare = -1;

                if (item.rare > RarityLoader.RarityCount - 1)
                    item.rare = RarityLoader.RarityCount - 1;
            }

            num *= num;
            item.value = (int)((float)item.value * num);
            item.prefix = rolledPrefix;
            return true;
        }
        /*private static void UndoItemAnimationCompensations(Item item)
        {
            item.useAnimation -= item.currentUseAnimationCompensation;
            currentUseAnimationCompensation = 0;
        }*/
        /*private void ApplyItemAnimationCompensationsToVanillaItems(Item item)
        {
            // #2351
            // Compensate for the change of itemAnimation getting reset at 0 instead of vanilla's 1.
            // all items with autoReuse in vanilla are affected, but the animation only has a physical effect for !noMelee items
            // for those items, we want the faster animation as that governs reuse time as dps is determined by swing speed.
            // for the others like ranged weapons, it's fine to keep the animation matching the use time, as dps is determined by item use speed
            currentUseAnimationCompensation = 0;

            if (type < ItemID.Count && autoReuse && !noMelee)
            {
                useAnimation--;
                currentUseAnimationCompensation--;
            }

            // in vanilla, items without autoReuse get a frame where itemAnimation == 0 between uses allowing the direction change checks in HorizontalMovement to apply
            // in tML we need to explicitly enable this behavior
            if (type < ItemID.Count && useStyle != 0 && !autoReuse && !useTurn && shoot == 0 && damage > 0)
            {
                useTurnOnAnimationStart = true;
            }
        }*/
        private static bool TryGetPrefixStatMultipliersForItem(Item item, int rolledPrefix, out float damageMultiplier, out float knockBackMultiplier, out float useSpeedMultiplier, out float sizeMultiplier, out float velocityMultiplier, out float manaCostMultiplier, out int critChanceAdded, out float meleeSpeedMultiplier)
        {
            damageMultiplier = 1f;
            knockBackMultiplier = 1f;
            useSpeedMultiplier = 1f;
            sizeMultiplier = 1f;
            velocityMultiplier = 1f;
            manaCostMultiplier = 1f;
            critChanceAdded = 0;
            meleeSpeedMultiplier = 1f;
            switch (rolledPrefix) //where the stats are set
            {
                case 1: //large
                    sizeMultiplier = 1.4f;
                    break;
                case 2: //massive
                    sizeMultiplier = 1.8f;
                    useSpeedMultiplier = 1.1f;
                    break;
                case 3: //dangerous
                    damageMultiplier = 1.05f;
                    critChanceAdded = 2;
                    sizeMultiplier = 1.05f;
                    knockBackMultiplier = 0.95f;
                    break;
                case 4: //savage - does a ton of true melee damage but makes your reach much shorter
                    damageMultiplier = 1.1f;
                    sizeMultiplier = 0.5f;
                    meleeSpeedMultiplier = 0.6f; //doesn't increase the speed projectiles are fired, except for the terra blade and many of it's compenents. those projectile are affected by the size unlike most of them. savage terra blade is probably still OP tho.
                    break;
                case 5: //sharp
                    damageMultiplier = 1.2f;
                    break;
                case 6: //pointy
                    damageMultiplier = 1.15f;
                    break;
                case 81: //legendary
                    knockBackMultiplier = 1.15f;
                    damageMultiplier = 1.15f;
                    critChanceAdded = 5;
                    useSpeedMultiplier = 0.9f;
                    sizeMultiplier = 1.1f;
                    break;
                case 7: //tiny
                    sizeMultiplier = 0.75f;
                    break;
                case 8: //terrible
                    knockBackMultiplier = 0.85f;
                    damageMultiplier = 0.85f;
                    sizeMultiplier = 0.87f;
                    break;
                case 9: //small
                    sizeMultiplier = 0.9f;
                    break;
                case 10: //dull
                    damageMultiplier = 0.85f;
                    break;
                case 11: //unhappy
                    useSpeedMultiplier = 1.1f;
                    knockBackMultiplier = 0.9f;
                    sizeMultiplier = 0.9f;
                    break;
                case 12: //bulky
                    knockBackMultiplier = 1.1f;
                    damageMultiplier = 1.05f;
                    sizeMultiplier = 1.1f;
                    useSpeedMultiplier = 1.15f;
                    break;
                case 13: //shameful
                    knockBackMultiplier = 0.8f;
                    damageMultiplier = 0.9f;
                    sizeMultiplier = 1.1f;
                    break;
                case 14: //heavy
                    knockBackMultiplier = 1.4f;
                    useSpeedMultiplier = 1.1f;
                    break;
                case 15: //light
                    knockBackMultiplier = 0.9f;
                    useSpeedMultiplier = 0.8f; //i guess this is a little buff to mining speed
                    break;
                case 16: //sighted
                    damageMultiplier = 1.1f;
                    critChanceAdded = 10;
                    break;
                case 17: //rapid
                    damageMultiplier = 0.95f;
                    useSpeedMultiplier = 0.75f;
                    velocityMultiplier = 1.15f;
                    break;
                case 18: //hasty
                    useSpeedMultiplier = 0.85f;
                    velocityMultiplier = 1.2f;
                    break;
                case 19: //intimidating
                    knockBackMultiplier = 1.25f;
                    velocityMultiplier = 1.05f;
                    break;
                case 20: //deadly
                    knockBackMultiplier = 1.05f;
                    velocityMultiplier = 1.05f;
                    damageMultiplier = 1.1f;
                    useSpeedMultiplier = 0.95f;
                    critChanceAdded = 2;
                    break;
                case 21: //staunch
                    knockBackMultiplier = 1.15f;
                    damageMultiplier = 1.2f;
                    useSpeedMultiplier = 0.8f;
                    velocityMultiplier = 0.5f;
                    break;
                case 82: //unreal
                    knockBackMultiplier = 1.15f;
                    damageMultiplier = 1.15f;
                    critChanceAdded = 5;
                    useSpeedMultiplier = 0.9f;
                    velocityMultiplier = 1.1f;
                    break;
                case 22: //awful
                    knockBackMultiplier = 0.9f;
                    velocityMultiplier = 0.9f;
                    damageMultiplier = 0.85f;
                    break;
                case 23: //lethargic
                    useSpeedMultiplier = 1.15f;
                    velocityMultiplier = 0.9f;
                    break;
                case 24: //awkward
                    useSpeedMultiplier = 1.15f;
                    knockBackMultiplier = 0.8f;
                    break;
                case 25: //powerful
                    useSpeedMultiplier = 1.15f;
                    damageMultiplier = 1.2f;
                    knockBackMultiplier = 1.4f;
                    break;
                case 58: //frenzying
                    useSpeedMultiplier = 0.6f;
                    damageMultiplier = 0.8f;
                    break;
                case 26: //mystic
                    manaCostMultiplier = 1.5f;
                    damageMultiplier = 1.3f;
                    critChanceAdded = 20;
                    break;
                case 27: //adept
                    manaCostMultiplier = 0.6f;
                    break;
                case 28: //masterful
                    manaCostMultiplier = 0.85f;
                    damageMultiplier = 1.15f;
                    knockBackMultiplier = 1.05f;
                    break;
                case 83: //mythical
                    knockBackMultiplier = 1.15f;
                    damageMultiplier = 1.15f;
                    critChanceAdded = 5;
                    useSpeedMultiplier = 0.9f;
                    manaCostMultiplier = 0.9f;
                    break;
                case 29: //inept
                    manaCostMultiplier = 1.1f;
                    break;
                case 30: //ignorant
                    manaCostMultiplier = 1.2f;
                    damageMultiplier = 0.9f;
                    break;
                case 31: //deranged
                    knockBackMultiplier = 0.9f;
                    damageMultiplier = 0.9f;
                    break;
                case 32: //intense
                    manaCostMultiplier = 1.15f;
                    damageMultiplier = 1.1f;
                    break;
                case 33: //taboo
                    manaCostMultiplier = 1.1f;
                    knockBackMultiplier = 1.1f;
                    useSpeedMultiplier = 0.9f;
                    break;
                case 34: //celestial
                    manaCostMultiplier = 0.8f;
                    knockBackMultiplier = 1.2f;
                    useSpeedMultiplier = 1.2f;
                    damageMultiplier = 1.15f;
                    break;
                case 35: //furious
                    manaCostMultiplier = 1.3f;
                    damageMultiplier = 1.2f;
                    knockBackMultiplier = 1.25f;
                    break;
                case 52: //manic
                    manaCostMultiplier = 0.8f;
                    damageMultiplier = 0.8f;
                    useSpeedMultiplier = 0.8f;
                    break;
                case 84: //legendary (terrarian variant)
                    knockBackMultiplier = 1.17f;
                    damageMultiplier = 1.17f;
                    critChanceAdded = 8;
                    break;
                case 36: //keen
                    critChanceAdded = 15;
                    break;
                case 37: //superior
                    damageMultiplier = 1.1f;
                    critChanceAdded = 3;
                    knockBackMultiplier = 1.1f;
                    break;
                case 38: //forceful
                    knockBackMultiplier = 2f;
                    break;
                case 53: //hurtful
                    damageMultiplier = 1.15f;
                    break;
                case 54: //strong
                    knockBackMultiplier = 1.5f;
                    break;
                case 55: //unpleasant
                    knockBackMultiplier = 1.15f;
                    damageMultiplier = 1.05f;
                    break;
                case 59: //godly
                    knockBackMultiplier = 1.15f;
                    damageMultiplier = 1.15f;
                    critChanceAdded = 5;
                    break;
                case 60: //demonic
                    damageMultiplier = 1.15f;
                    critChanceAdded = 5;
                    break;
                case 61: //zealous
                    critChanceAdded = 25;
                    break;
                case 39: //broken
                    damageMultiplier = 0.7f;
                    knockBackMultiplier = 0.8f;
                    break;
                case 40: //damaged
                    damageMultiplier = 0.85f;
                    critChanceAdded = 5;
                    break;
                case 56: //weak
                    knockBackMultiplier = 0.6f;
                    break;
                case 41: //shoddy
                    knockBackMultiplier = 0.85f;
                    damageMultiplier = 0.9f;
                    break;
                case 57: //ruthless
                    knockBackMultiplier = 0.75f;
                    damageMultiplier = 1.3f; //indirect summoner buff right here
                    break;
                case 42: //quick
                    useSpeedMultiplier = 0.85f;
                    break;
                case 43: //deadly
                    damageMultiplier = 1.1f;
                    useSpeedMultiplier = 0.9f;
                    break;
                case 44: //agile
                    useSpeedMultiplier = 0.9f;
                    critChanceAdded = 10;
                    break;
                case 45: //nimble
                    useSpeedMultiplier = 0.90f;
                    break;
                case 46: //murderous
                    critChanceAdded = 3;
                    useSpeedMultiplier = 0.94f;
                    damageMultiplier = 1.07f;
                    break;
                case 47: //slow
                    useSpeedMultiplier = 1.15f;
                    break;
                case 48: //sluggish
                    useSpeedMultiplier = 1.2f;
                    break;
                case 49: //lazy
                    damageMultiplier = 1.05f;
                    useSpeedMultiplier = 1.08f;
                    break;
                case 50: //annoying
                    damageMultiplier = 0.8f;
                    useSpeedMultiplier = 1.15f;
                    break;
                case 51: //nasty
                    knockBackMultiplier = 0.85f;
                    useSpeedMultiplier = 0.9f;
                    damageMultiplier = 1.05f;
                    critChanceAdded = 5;
                    break;
                // Handle mod prefixes.
                case int pre when PrefixLoader.GetPrefix(pre) is ModPrefix modPrefix:
                    if (!modPrefix.AllStatChangesHaveEffectOn(item))
                        return false;

                    modPrefix.SetStats(ref damageMultiplier, ref knockBackMultiplier, ref useSpeedMultiplier, ref sizeMultiplier, ref velocityMultiplier, ref manaCostMultiplier, ref critChanceAdded);
                    break;
            }
            if (damageMultiplier != 1f && Math.Round((float)item.damage * damageMultiplier) == (double)item.damage)
            {
                return false;
            }
            if (useSpeedMultiplier != 1f && Math.Round((float)item.useAnimation * useSpeedMultiplier) == (double)item.useAnimation)
            {
                return false;
            }
            if (manaCostMultiplier != 1f && Math.Round((float)item.mana * manaCostMultiplier) == (double)item.mana)
            {
                return false;
            }
            if (knockBackMultiplier != 1f && item.knockBack == 0f)
            {
                return false;
            }
            return true;
        }
            private static bool RollAPrefix(UnifiedRandom random, ref int rolledPrefix, Item item)
        {
            /*
            int[] rollablePrefixes = GetRollablePrefixes();
            if (rollablePrefixes == null)
                return false;

            rolledPrefix = rollablePrefixes[random.Next(rollablePrefixes.Length)];

            return true;
            */

            return PrefixLoader.Roll(item, random, out rolledPrefix, justCheck: false);
        }
    }
}
