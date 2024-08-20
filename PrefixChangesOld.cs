/*using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace ImprovedReforging
{
	public class PrefixChanges
	{
        public static bool MyPrefix(int pre, Item item) //the Item.Prefix method from the vanilla source code, but with edited values
        {
            if (!WorldGen.gen && Main.rand == null)
            {
                Main.rand = new UnifiedRandom();
            }
            if (pre == 0 || item.type == 0)
            {
                return false;
            }
            UnifiedRandom unifiedRandom = (WorldGen.gen ? WorldGen.genRand : Main.rand);
            int num = pre;
            float damageMultiplier = 1f;
            float knockBackMultiplier = 1f;
            float useSpeedMultiplier = 1f;
            float meleeSpeedMultiplier = 1f;
            float sizeMultiplier = 1f;
            float velocityMultiplier = 1f;
            //float accuracyMultiplier = 1f; //didnt work out
            float manaCostMultiplier = 1f;
            int critChanceAdded = 0;
            bool flag = true;
            while (flag)
            {
                damageMultiplier = 1f;
                knockBackMultiplier = 1f;
                useSpeedMultiplier = 1f;
                sizeMultiplier = 1f;
                velocityMultiplier = 1f;
                manaCostMultiplier = 1f;
                critChanceAdded = 0;
                flag = false;
                if (num == -1 && unifiedRandom.Next(4) == 0)
                {
                    num = 0;
                }
                if (pre < -1)
                {
                    num = -1;
                }
                if (num == -1 || num == -2 || num == -3)
                {
                    if (item.type == 1 || item.type == 4 || item.type == 6 || item.type == 7 || item.type == 10 || item.type == 24 || item.type == 45 || item.type == 46 || item.type == 65 || item.type == 103 || item.type == 104 || item.type == 121 || item.type == 122 || item.type == 155 || item.type == 190 || item.type == 196 || item.type == 198 || item.type == 199 || item.type == 200 || item.type == 201 || item.type == 202 || item.type == 203 || item.type == 4258 || item.type == 204 || item.type == 213 || item.type == 217 || item.type == 273 || item.type == 367 || item.type == 368 || item.type == 426 || item.type == 482 || item.type == 483 || item.type == 484 || item.type == 653 || item.type == 654 || item.type == 656 || item.type == 657 || item.type == 659 || item.type == 660 || item.type == 671 || item.type == 672 || item.type == 674 || item.type == 675 || item.type == 676 || item.type == 723 || item.type == 724 || item.type == 757 || item.type == 776 || item.type == 777 || item.type == 778 || item.type == 787 || item.type == 795 || item.type == 797 || item.type == 798 || item.type == 799 || item.type == 881 || item.type == 882 || item.type == 921 || item.type == 922 || item.type == 989 || item.type == 990 || item.type == 991 || item.type == 992 || item.type == 993 || item.type == 1123 || item.type == 1166 || item.type == 1185 || item.type == 1188 || item.type == 1192 || item.type == 1195 || item.type == 1199 || item.type == 1202 || item.type == 1222 || item.type == 1223 || item.type == 1224 || item.type == 1226 || item.type == 1227 || item.type == 1230 || item.type == 1233 || item.type == 1234 || item.type == 1294 || item.type == 1304 || item.type == 1305 || item.type == 1306 || item.type == 1320 || item.type == 1327 || item.type == 1506 || item.type == 1507 || item.type == 1786 || item.type == 1826 || item.type == 1827 || item.type == 1909 || item.type == 1917 || item.type == 1928 || item.type == 2176 || item.type == 2273 || item.type == 2608 || item.type == 2341 || item.type == 2330 || item.type == 2320 || item.type == 2516 || item.type == 2517 || item.type == 2746 || item.type == 2745 || item.type == 3063 || item.type == 3018 || item.type == 3211 || item.type == 3013 || item.type == 3258 || item.type == 3106 || item.type == 3065 || item.type == 2880 || item.type == 3481 || item.type == 3482 || item.type == 3483 || item.type == 3484 || item.type == 3485 || item.type == 3487 || item.type == 3488 || item.type == 3489 || item.type == 3490 || item.type == 3491 || item.type == 3493 || item.type == 3494 || item.type == 3495 || item.type == 3496 || item.type == 3497 || item.type == 3499 || item.type == 3500 || item.type == 3501 || item.type == 3502 || item.type == 3503 || item.type == 3505 || item.type == 3506 || item.type == 3507 || item.type == 3508 || item.type == 3509 || item.type == 3511 || item.type == 3512 || item.type == 3513 || item.type == 3514 || item.type == 3515 || item.type == 3517 || item.type == 3518 || item.type == 3519 || item.type == 3520 || item.type == 3521 || item.type == 3522 || item.type == 3523 || item.type == 3524 || item.type == 3525 || item.type == 3462 || item.type == 3465 || item.type == 3466 || item.type == 2772 || item.type == 2775 || item.type == 2776 || item.type == 2777 || item.type == 2780 || item.type == 2781 || item.type == 2782 || item.type == 2785 || item.type == 2786 || item.type == 3349 || item.type == 3352 || item.type == 3351 || (item.type >= 3764 && item.type <= 3769) || item.type == 4259 || item.type == 3772 || item.type == 3823 || item.type == 3827 || item.type == 186 || item.type == 946 || item.type == 4059 || item.type == 4317 || item.type == 4463 || item.type == 486 || item.type == 4707 || item.type == 4711 || item.type == 4956 || item.type == 4923 || item.type == 4672 || item.type == 4913 || item.type == 4912 || item.type == 4911 || item.type == 4678 || item.type == 4679 || item.type == 4680 || item.type == 4914 || item.type == 5074)
                    {
                        int num9 = unifiedRandom.Next(40);
                        if (num9 == 0)
                        {
                            num = 1;
                        }
                        if (num9 == 1)
                        {
                            num = 2;
                        }
                        if (num9 == 2)
                        {
                            num = 3;
                        }
                        if (num9 == 3)
                        {
                            num = 4;
                        }
                        if (num9 == 4)
                        {
                            num = 5;
                        }
                        if (num9 == 5)
                        {
                            num = 6;
                        }
                        if (num9 == 6)
                        {
                            num = 7;
                        }
                        if (num9 == 7)
                        {
                            num = 8;
                        }
                        if (num9 == 8)
                        {
                            num = 9;
                        }
                        if (num9 == 9)
                        {
                            num = 10;
                        }
                        if (num9 == 10)
                        {
                            num = 11;
                        }
                        if (num9 == 11)
                        {
                            num = 12;
                        }
                        if (num9 == 12)
                        {
                            num = 13;
                        }
                        if (num9 == 13)
                        {
                            num = 14;
                        }
                        if (num9 == 14)
                        {
                            num = 15;
                        }
                        if (num9 == 15)
                        {
                            num = 36;
                        }
                        if (num9 == 16)
                        {
                            num = 37;
                        }
                        if (num9 == 17)
                        {
                            num = 38;
                        }
                        if (num9 == 18)
                        {
                            num = 53;
                        }
                        if (num9 == 19)
                        {
                            num = 54;
                        }
                        if (num9 == 20)
                        {
                            num = 55;
                        }
                        if (num9 == 21)
                        {
                            num = 39;
                        }
                        if (num9 == 22)
                        {
                            num = 40;
                        }
                        if (num9 == 23)
                        {
                            num = 56;
                        }
                        if (num9 == 24)
                        {
                            num = 41;
                        }
                        if (num9 == 25)
                        {
                            num = 57;
                        }
                        if (num9 == 26)
                        {
                            num = 42;
                        }
                        if (num9 == 27)
                        {
                            num = 43;
                        }
                        if (num9 == 28)
                        {
                            num = 44;
                        }
                        if (num9 == 29)
                        {
                            num = 45;
                        }
                        if (num9 == 30)
                        {
                            num = 46;
                        }
                        if (num9 == 31)
                        {
                            num = 47;
                        }
                        if (num9 == 32)
                        {
                            num = 48;
                        }
                        if (num9 == 33)
                        {
                            num = 49;
                        }
                        if (num9 == 34)
                        {
                            num = 50;
                        }
                        if (num9 == 35)
                        {
                            num = 51;
                        }
                        if (num9 == 36)
                        {
                            num = 59;
                        }
                        if (num9 == 37)
                        {
                            num = 60;
                        }
                        if (num9 == 38)
                        {
                            num = 61;
                        }
                        if (num9 == 39)
                        {
                            num = 81;
                        }
                    }
                    else if (item.type == 162 || item.type == 5011 || item.type == 5012 || item.type == 160 || item.type == 163 || item.type == 220 || item.type == 274 || item.type == 277 || item.type == 280 || item.type == 383 || item.type == 384 || item.type == 385 || item.type == 386 || item.type == 387 || item.type == 388 || item.type == 389 || item.type == 390 || item.type == 406 || item.type == 537 || item.type == 550 || item.type == 579 || item.type == 756 || item.type == 759 || item.type == 801 || item.type == 802 || item.type == 1186 || item.type == 1189 || item.type == 1190 || item.type == 1193 || item.type == 1196 || item.type == 1197 || item.type == 1200 || item.type == 1203 || item.type == 1204 || item.type == 1228 || item.type == 1231 || item.type == 1232 || item.type == 1259 || item.type == 1262 || item.type == 1297 || item.type == 1314 || item.type == 1325 || item.type == 1947 || item.type == 2332 || item.type == 2331 || item.type == 2342 || item.type == 2424 || item.type == 2611 || item.type == 2798 || item.type == 3012 || item.type == 3473 || item.type == 3098 || item.type == 3368 || item.type == 3835 || item.type == 3836 || item.type == 3858 || item.type == 4061 || item.type == 4144 || item.type == 4272 || item.type == 2774 || item.type == 2773 || item.type == 2779 || item.type == 2778 || item.type == 2784 || item.type == 2783 || item.type == 3464 || item.type == 3463 || item.type == 4788 || item.type == 4789 || item.type == 4790)
                    {
                        int num10 = unifiedRandom.Next(14);
                        if (num10 == 0)
                        {
                            num = 36;
                        }
                        if (num10 == 1)
                        {
                            num = 37;
                        }
                        if (num10 == 2)
                        {
                            num = 38;
                        }
                        if (num10 == 3)
                        {
                            num = 53;
                        }
                        if (num10 == 4)
                        {
                            num = 54;
                        }
                        if (num10 == 5)
                        {
                            num = 55;
                        }
                        if (num10 == 6)
                        {
                            num = 39;
                        }
                        if (num10 == 7)
                        {
                            num = 40;
                        }
                        if (num10 == 8)
                        {
                            num = 56;
                        }
                        if (num10 == 9)
                        {
                            num = 41;
                        }
                        if (num10 == 10)
                        {
                            num = 57;
                        }
                        if (num10 == 11)
                        {
                            num = 59;
                        }
                        if (num10 == 12)
                        {
                            num = 60;
                        }
                        if (num10 == 13)
                        {
                            num = 61;
                        }
                    }
                    else if (item.type == 39 || item.type == 44 || item.type == 95 || item.type == 96 || item.type == 98 || item.type == 99 || item.type == 120 || item.type == 164 || item.type == 197 || item.type == 219 || item.type == 266 || item.type == 281 || item.type == 434 || item.type == 435 || item.type == 436 || item.type == 481 || item.type == 506 || item.type == 533 || item.type == 534 || item.type == 578 || item.type == 655 || item.type == 658 || item.type == 661 || item.type == 679 || item.type == 682 || item.type == 725 || item.type == 758 || item.type == 759 || item.type == 760 || item.type == 796 || item.type == 800 || item.type == 905 || item.type == 923 || item.type == 964 || item.type == 986 || item.type == 1156 || item.type == 1187 || item.type == 1194 || item.type == 1201 || item.type == 1229 || item.type == 1254 || item.type == 1255 || item.type == 1258 || item.type == 1265 || item.type == 1319 || item.type == 1553 || item.type == 1782 || item.type == 1784 || item.type == 1835 || item.type == 1870 || item.type == 1910 || item.type == 1929 || item.type == 1946 || item.type == 2223 || item.type == 2269 || item.type == 2270 || item.type == 2624 || item.type == 2515 || item.type == 2747 || item.type == 2796 || item.type == 2797 || item.type == 3052 || item.type == 2888 || item.type == 3019 || item.type == 3029 || item.type == 3007 || item.type == 3008 || item.type == 3210 || item.type == 3107 || item.type == 3475 || item.type == 3540 || item.type == 3854 || item.type == 3859 || item.type == 3821 || item.type == 3930 || item.type == 3480 || item.type == 3486 || item.type == 3492 || item.type == 3498 || item.type == 3504 || item.type == 3510 || item.type == 3516 || item.type == 3350 || item.type == 3546 || item.type == 3788 || item.type == 4058 || item.type == 4060 || item.type == 4381 || item.type == 4703 || item.type == 4953)
                    {
                        int num11 = unifiedRandom.Next(35);
                        if (num11 == 0)
                        {
                            num = 16;
                        }
                        if (num11 == 1)
                        {
                            num = 17;
                        }
                        if (num11 == 2)
                        {
                            num = 18;
                        }
                        if (num11 == 3)
                        {
                            num = 19;
                        }
                        if (num11 == 4)
                        {
                            num = 20;
                        }
                        if (num11 == 5)
                        {
                            num = 21;
                        }
                        if (num11 == 6)
                        {
                            num = 22;
                        }
                        if (num11 == 7)
                        {
                            num = 23;
                        }
                        if (num11 == 8)
                        {
                            num = 24;
                        }
                        if (num11 == 9)
                        {
                            num = 25;
                        }
                        if (num11 == 10)
                        {
                            num = 58;
                        }
                        if (num11 == 11)
                        {
                            num = 36;
                        }
                        if (num11 == 12)
                        {
                            num = 37;
                        }
                        if (num11 == 13)
                        {
                            num = 38;
                        }
                        if (num11 == 14)
                        {
                            num = 53;
                        }
                        if (num11 == 15)
                        {
                            num = 54;
                        }
                        if (num11 == 16)
                        {
                            num = 55;
                        }
                        if (num11 == 17)
                        {
                            num = 39;
                        }
                        if (num11 == 18)
                        {
                            num = 40;
                        }
                        if (num11 == 19)
                        {
                            num = 56;
                        }
                        if (num11 == 20)
                        {
                            num = 41;
                        }
                        if (num11 == 21)
                        {
                            num = 57;
                        }
                        if (num11 == 22)
                        {
                            num = 42;
                        }
                        if (num11 == 23)
                        {
                            num = 44;
                        }
                        if (num11 == 24)
                        {
                            num = 45;
                        }
                        if (num11 == 25)
                        {
                            num = 46;
                        }
                        if (num11 == 26)
                        {
                            num = 47;
                        }
                        if (num11 == 27)
                        {
                            num = 48;
                        }
                        if (num11 == 28)
                        {
                            num = 49;
                        }
                        if (num11 == 29)
                        {
                            num = 50;
                        }
                        if (num11 == 30)
                        {
                            num = 51;
                        }
                        if (num11 == 31)
                        {
                            num = 59;
                        }
                        if (num11 == 32)
                        {
                            num = 60;
                        }
                        if (num11 == 33)
                        {
                            num = 61;
                        }
                        if (num11 == 34)
                        {
                            num = 82;
                        }
                    }
                    else if (item.type == 64 || item.type == 112 || item.type == 113 || item.type == 127 || item.type == 157 || item.type == 165 || item.type == 218 || item.type == 272 || item.type == 494 || item.type == 495 || item.type == 496 || item.type == 514 || item.type == 517 || item.type == 518 || item.type == 519 || item.type == 683 || item.type == 726 || item.type == 739 || item.type == 740 || item.type == 741 || item.type == 742 || item.type == 743 || item.type == 744 || item.type == 788 || item.type == 1121 || item.type == 1155 || item.type == 1157 || item.type == 1178 || item.type == 1244 || item.type == 1256 || item.type == 1260 || item.type == 1264 || item.type == 1266 || item.type == 1295 || item.type == 1296 || item.type == 1308 || item.type == 1309 || item.type == 1313 || item.type == 1336 || item.type == 1444 || item.type == 1445 || item.type == 1446 || item.type == 1572 || item.type == 1801 || item.type == 1802 || item.type == 1930 || item.type == 1931 || item.type == 2188 || item.type == 2622 || item.type == 2621 || item.type == 2584 || item.type == 2551 || item.type == 2366 || item.type == 2535 || item.type == 2365 || item.type == 2364 || item.type == 2623 || item.type == 2750 || item.type == 2795 || item.type == 3053 || item.type == 3051 || item.type == 3209 || item.type == 3014 || item.type == 3105 || item.type == 2882 || item.type == 3269 || item.type == 3006 || item.type == 3377 || item.type == 3069 || item.type == 2749 || item.type == 3249 || item.type == 3476 || item.type == 3474 || item.type == 3531 || item.type == 3541 || item.type == 3542 || item.type == 3569 || item.type == 3570 || item.type == 3571 || item.type == 3779 || item.type == 3787 || item.type == 3531 || item.type == 3852 || item.type == 3870 || item.type == 4269 || item.type == 4273 || item.type == 4281 || item.type == 4347 || item.type == 4348 || item.type == 4270 || item.type == 4758 || item.type == 4715 || item.type == 4952 || item.type == 4607 || item.type == 5005 || item.type == 5065 || item.type == 5069 || item.type == 3824 || item.type == 3818 || item.type == 3829 || item.type == 3832 || item.type == 3825 || item.type == 3819 || item.type == 3830 || item.type == 3833 || item.type == 3826 || item.type == 3820 || item.type == 3831 || item.type == 3834 || item.type == 4062)
                    {
                        int num12 = unifiedRandom.Next(36);
                        if (num12 == 0)
                        {
                            num = 26;
                        }
                        if (num12 == 1)
                        {
                            num = 27;
                        }
                        if (num12 == 2)
                        {
                            num = 28;
                        }
                        if (num12 == 3)
                        {
                            num = 29;
                        }
                        if (num12 == 4)
                        {
                            num = 30;
                        }
                        if (num12 == 5)
                        {
                            num = 31;
                        }
                        if (num12 == 6)
                        {
                            num = 32;
                        }
                        if (num12 == 7)
                        {
                            num = 33;
                        }
                        if (num12 == 8)
                        {
                            num = 34;
                        }
                        if (num12 == 9)
                        {
                            num = 35;
                        }
                        if (num12 == 10)
                        {
                            num = 52;
                        }
                        if (num12 == 11)
                        {
                            num = 36;
                        }
                        if (num12 == 12)
                        {
                            num = 37;
                        }
                        if (num12 == 13)
                        {
                            num = 38;
                        }
                        if (num12 == 14)
                        {
                            num = 53;
                        }
                        if (num12 == 15)
                        {
                            num = 54;
                        }
                        if (num12 == 16)
                        {
                            num = 55;
                        }
                        if (num12 == 17)
                        {
                            num = 39;
                        }
                        if (num12 == 18)
                        {
                            num = 40;
                        }
                        if (num12 == 19)
                        {
                            num = 56;
                        }
                        if (num12 == 20)
                        {
                            num = 41;
                        }
                        if (num12 == 21)
                        {
                            num = 57;
                        }
                        if (num12 == 22)
                        {
                            num = 42;
                        }
                        if (num12 == 23)
                        {
                            num = 43;
                        }
                        if (num12 == 24)
                        {
                            num = 44;
                        }
                        if (num12 == 25)
                        {
                            num = 45;
                        }
                        if (num12 == 26)
                        {
                            num = 46;
                        }
                        if (num12 == 27)
                        {
                            num = 47;
                        }
                        if (num12 == 28)
                        {
                            num = 48;
                        }
                        if (num12 == 29)
                        {
                            num = 49;
                        }
                        if (num12 == 30)
                        {
                            num = 50;
                        }
                        if (num12 == 31)
                        {
                            num = 51;
                        }
                        if (num12 == 32)
                        {
                            num = 59;
                        }
                        if (num12 == 33)
                        {
                            num = 60;
                        }
                        if (num12 == 34)
                        {
                            num = 61;
                        }
                        if (num12 == 35)
                        {
                            num = 83;
                        }
                    }
                    else if (item.type == 55 || item.type == 119 || item.type == 191 || item.type == 284 || item.type == 670 || item.type == 1122 || item.type == 1513 || item.type == 1569 || item.type == 1571 || item.type == 1825 || item.type == 1918 || item.type == 3054 || item.type == 3262 || (item.type >= 3278 && item.type <= 3292) || (item.type >= 3315 && item.type <= 3317) || item.type == 3389 || item.type == 3030 || item.type == 3543 || item.type == 4764 || item.type == 4818 || item.type == 4760)
                    {
                        int num13 = unifiedRandom.Next(14);
                        if (item.type == 3389)
                        {
                            num13 = unifiedRandom.Next(15);
                        }
                        if (num13 == 0)
                        {
                            num = 36;
                        }
                        if (num13 == 1)
                        {
                            num = 37;
                        }
                        if (num13 == 2)
                        {
                            num = 38;
                        }
                        if (num13 == 3)
                        {
                            num = 53;
                        }
                        if (num13 == 4)
                        {
                            num = 54;
                        }
                        if (num13 == 5)
                        {
                            num = 55;
                        }
                        if (num13 == 6)
                        {
                            num = 39;
                        }
                        if (num13 == 7)
                        {
                            num = 40;
                        }
                        if (num13 == 8)
                        {
                            num = 56;
                        }
                        if (num13 == 9)
                        {
                            num = 41;
                        }
                        if (num13 == 10)
                        {
                            num = 57;
                        }
                        if (num13 == 11)
                        {
                            num = 59;
                        }
                        if (num13 == 12)
                        {
                            num = 60;
                        }
                        if (num13 == 13)
                        {
                            num = 61;
                        }
                        if (num13 == 14)
                        {
                            num = 84;
                        }
                    }
                    else
                    {
                        if (!item.IsAPrefixableAccessory())
                        {
                            return false;
                        }
                        num = unifiedRandom.Next(62, 81);
                    }
                }
                switch (pre)
                {
                    case -3:
                        return true;
                    case -1:
                        if ((num == 7 || num == 8 || num == 9 || num == 10 || num == 11 || num == 22 || num == 23 || num == 24 || num == 29 || num == 30 || num == 31 || num == 39 || num == 40 || num == 56 || num == 41 || num == 47 || num == 48 || num == 49) && unifiedRandom.Next(3) != 0)
                        {
                            num = 0;
                        }
                        break;
                }
                switch (num) //where the stats are set
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
                        damageMultiplier *= 1.05f;
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
                    case int pref when PrefixLoader.GetPrefix(pref) is ModPrefix modPrefix:
                        if (!modPrefix.AllStatChangesHaveEffectOn(item))
                            return false;

                        modPrefix.SetStats(ref damageMultiplier, ref knockBackMultiplier, ref useSpeedMultiplier, ref sizeMultiplier, ref velocityMultiplier, ref manaCostMultiplier, ref critChanceAdded);
                        break;
                }
                if (damageMultiplier != 1f && Math.Round((float)item.damage * damageMultiplier) == (double)item.damage)
                {
                    flag = true;
                    num = -1;
                }
                if (useSpeedMultiplier != 1f && Math.Round((float)item.useAnimation * useSpeedMultiplier) == (double)item.useAnimation)
                {
                    flag = true;
                    num = -1;
                }
                if (manaCostMultiplier != 1f && Math.Round((float)item.mana * manaCostMultiplier) == (double)item.mana)
                {
                    flag = true;
                    num = -1;
                }
                if (knockBackMultiplier != 1f && item.knockBack == 0f)
                {
                    flag = true;
                    num = -1;
                }
                if (pre == -2 && num == 0)
                {
                    num = -1;
                    flag = true;
                }
            }
            item.damage = (int)Math.Round((float)item.damage * damageMultiplier);
            item.useAnimation = (int)Math.Round((float)item.useAnimation * useSpeedMultiplier);
            item.useAnimation = (int)Math.Round((float)item.useAnimation * meleeSpeedMultiplier); //only affects melee swing speed
            item.useTime = (int)Math.Round((float)item.useTime * useSpeedMultiplier);
            item.reuseDelay = (int)Math.Round((float)item.reuseDelay * useSpeedMultiplier);
            item.mana = (int)Math.Round((float)item.mana * manaCostMultiplier);
            item.knockBack *= knockBackMultiplier;
            item.scale *= sizeMultiplier;
            item.shootSpeed *= velocityMultiplier;
            item.crit += critChanceAdded;
            //determines rarity and value, i changed how much most things affect it, damage, crit, and use speed are weighed more, and knockback, size, and
            //velocity are less (crit is still less than vanilla tho). also use speed and mana are divided instead of multiplying by 2 - the value.
            float num14 = 1f * ((damageMultiplier - 1) * 1.5f + 1) * ((1 / useSpeedMultiplier - 1) * 1.5f + 1) * 
            ((1 / meleeSpeedMultiplier - 1) * 1.25f + 1) * ((1 / manaCostMultiplier - 1) * 0.5f + 1) * ((sizeMultiplier - 1) * 0.5f + 1) * 
            ((knockBackMultiplier - 1) * 0.25f + 1) * ((velocityMultiplier - 1) * 0.5f + 1) * (1f + (float)critChanceAdded * 0.015f);
            if (num == 62 || num == 69 || num == 73 || num == 77) //tier 1 accessory prefixes
            {
                num14 *= 1.05f;
            }
            if (num == 63 || num == 70 || num == 74 || num == 78 || num == 67) //tier 2 accessory prefixes
            {
                num14 *= 1.1f;
            }
            if (num == 64 || num == 71 || num == 75 || num == 79 || num == 66) //tier 3 accessory prefixes
            {
                num14 *= 1.15f;
            }
            if (num == 65 || num == 72 || num == 76 || num == 80 || num == 68) //tier 4 accessory prefixes
            {
                num14 *= 1.2f;
            }
            if ((double)num14 >= 1.2)
            {
                item.rare += 2;
            }
            else if ((double)num14 >= 1.05)
            {
                item.rare++;
            }
            else if ((double)num14 <= 0.8)
            {
                item.rare -= 2;
            }
            else if ((double)num14 <= 0.95)
            {
                item.rare--;
            }
            if (item.rare > -11)
            {
                if (item.rare < -1)
                {
                    item.rare = -1;
                }
                if (item.rare > 11)
                {
                    item.rare = 11;
                }
            }
            num14 *= num14;
            item.value = (int)((float)item.value * num14);
            item.prefix = (byte)num;
            return true;
        }
    }
}*/
