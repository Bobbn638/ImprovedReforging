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

namespace ImprovedReforging.UI
{
	public class ReforgePanel : UIPanel
	{
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            if (ContainsPoint(Main.MouseScreen)) //no using items while hovering over this panel
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if (IsMouseHovering) //disable hotbar scrolling while hovering over this panel
            {
                PlayerInput.LockVanillaMouseScroll("MyMod/ScrollListA"); // The passed in string can be anything.
            }
        }
    }
}
