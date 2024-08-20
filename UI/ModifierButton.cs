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
	public class ModifierButton : UIPanel
	{
		public Item PrefixItem;
		public ModifierButton(Item p)
		{ 
			PrefixItem = p;
		}
    }
}
