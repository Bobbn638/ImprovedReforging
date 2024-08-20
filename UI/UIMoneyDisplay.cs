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
using Terraria.GameContent;

namespace ImprovedReforging.UI
{
    public class UIMoneyDisplay : UIElement //from ExampleMod
    {
        // coins in copper
        public long money;
        public bool savingsDisplay;        
        // Saving coin textures to an array to make them easier to access
        private readonly Texture2D[] coinsTextures = new Texture2D[4];

        public UIMoneyDisplay() //always a savings display
        {
            savingsDisplay = true;
            money = CalcSavings();
            UIText text = new UIText("Savings");
            text.HAlign = 0f;
            Append(text);
            for (int j = 0; j < 4; j++)
            {
                // Textures may not be loaded without it
                Main.instance.LoadItem(74 - j);
                coinsTextures[j] = TextureAssets.Item[74 - j].Value;
            }
        }
        public UIMoneyDisplay(long money) //not a savings display, just displays whatever you pass in for money
        {
            this.money = money;
            for (int j = 0; j < 4; j++)
            {
                // Textures may not be loaded without it
                Main.instance.LoadItem(74 - j);
                coinsTextures[j] = TextureAssets.Item[74 - j].Value;
            }
        }
        public long CalcSavings()
        {
            long savings = 0;
            Player player = Main.LocalPlayer;
            bool overFlowing;
            savings += Utils.CoinsCount(out overFlowing, player.inventory, 58, 57, 56, 55, 54);
            savings += Utils.CoinsCount(out overFlowing, player.bank.item);
            savings += Utils.CoinsCount(out overFlowing, player.bank2.item);
            savings += Utils.CoinsCount(out overFlowing, player.bank3.item);
            savings += Utils.CoinsCount(out overFlowing, player.bank4.item);
            return savings;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (savingsDisplay)
                money = CalcSavings();
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle innerDimensions = GetInnerDimensions();
            // Getting top left position of this UIElement
            float shopx = innerDimensions.X;
            float shopy = innerDimensions.Y;

            // Drawing first line of coins (current collected coins)
            // CoinsSplit converts the number of copper coins into an array of all types of coins
            if (money > 999999999) //caps for visual purposes, like vanilla shops
                money = 999999999;
            DrawCoins(spriteBatch, shopx, savingsDisplay ? shopy + 30 : shopy, Utils.CoinsSplit(money));//savings display need space for the word savings

            // Drawing second line of coins (coins per minute) and text "CPM"
            //DrawCoins(spriteBatch, shopx, shopy, Utils.CoinsSplit(savings));
            //Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, "CPM", shopx + (float)(24 * 4), shopy + 25f, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
        }

        private void DrawCoins(SpriteBatch spriteBatch, float shopx, float shopy, int[] coinsArray, int xOffset = 0, int yOffset = 0)
        {
            for (int j = 0; j < 4; j++)
            {
                spriteBatch.Draw(coinsTextures[j], new Vector2(shopx + 11f + 24 * j + xOffset, shopy + yOffset), null, Color.White, 0f, coinsTextures[j].Size() / 2f, 1f, SpriteEffects.None, 0f);
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, coinsArray[3 - j].ToString(), shopx + 24 * j + xOffset, shopy + yOffset, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
            }
        }
    }
}
