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
using Terraria.ID;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Localization;

namespace ImprovedReforging.UI
{
	public class ReforgeState : UIState
	{
        ReforgePanel panel;
        ReforgeItemSlot itemSlot;
        ScrollPanel scrollPanel;
        UIMoneyDisplay moneyDisplay;

        public override void OnInitialize()
        {
            panel = new ReforgePanel();
            panel.Width.Set(850, 0);          
            panel.Height.Set(740, 0);
            panel.HAlign = 0.65f;
            panel.VAlign = 0.5f;
            Append(panel);

            itemSlot = new ReforgeItemSlot();
            itemSlot.Left.Set(25, 0);
            panel.Append(itemSlot);

            moneyDisplay = new UIMoneyDisplay();
            moneyDisplay.Left.Set(100, 0);
            moneyDisplay.Top.Set(20, 0);
            panel.Append(moneyDisplay);

            scrollPanel = new ScrollPanel(itemSlot);
            scrollPanel.Width.Set(800, 0);
            scrollPanel.Height.Set(640, 0);
            scrollPanel.HAlign = 0.5f;
            scrollPanel.Top.Set(75, 0);
            panel.Append(scrollPanel);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
