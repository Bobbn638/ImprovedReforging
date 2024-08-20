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
	public class ReforgeNPC : GlobalNPC
	{
        public override bool PreChatButtonClicked(NPC npc, bool firstButton)
        {
            if(ModContent.GetInstance<ImprovedReforgingConfig>().GoblinTinkererRework && npc.type == NPCID.GoblinTinkerer && firstButton == false) //this is the reforge button, only while tinkerer rework is enabled in config
            {
                Main.npcChatText = ""; //hide the text
                ModContent.GetInstance<ReforgeModSystem>().ShowUI();
                return false;
            }
            return true;
        }
    }
}
