using log4net.Repository.Hierarchy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace ImprovedReforging
{
	public class ReforgePlayer : ModPlayer
	{
        //this doesn't work, the game also makes stuff inaccurate after this runs
        /*public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (ModContent.GetInstance<ImprovedReforgingConfig>().PrefixRework)
            {
                int[] accuracyPrefixes = { 16, 17, 20, 21, 25, 58 };
                if (accuracyPrefixes.Contains(item.prefix))
                {
                    float accuracyMultiplier = 1f;
                    switch (item.prefix)
                    {
                        case 16:
                            accuracyMultiplier = 0f;
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
                            accuracyMultiplier = 5f;
                            break;
                        default:
                            break;
                    }
                    //Main.NewText("", 150, 250, 150);
                    Vector2 mouseDirection = Main.MouseWorld - Player.Center;
                    mouseDirection.Normalize();
                    float speed = velocity.Length();
                    velocity.Normalize();
                    velocity = Vector2.Lerp(mouseDirection, velocity, accuracyMultiplier);
                    velocity *= speed;
                }
            }
            base.ModifyShootStats(item, ref position, ref velocity, ref type, ref damage, ref knockback);
        }*/
    }
}
