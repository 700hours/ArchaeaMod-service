using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.TakerylProject.Items
{
	public class Alpha : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Alpha's Grace");
		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 32;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 2;
			Item.maxStack = 1;
			Item.value = 0;
			Item.rare = 3;
			Item.consumable = true;
			Item.autoReuse = false;
			Item.useTurn = false;
			Item.noMelee = true;
		}
		public override void AddRecipes()
		{
			//Recipe.Create(Type)
				  //.AddIngredient(9)
				  //.Register();
		}
		public override bool? UseItem(Player player)
		{
			var modPlayer = player.GetModPlayer<ProjectPlayer>();
			modPlayer.angel = true;
			
			SoundEngine.PlaySound(SoundID.Item2, player.Center);
			
			for(int i = 0; i < player.inventory.Length-1; i++)
			{
				if(player.inventory[i].type == Type)
					player.inventory[i].type = 0;
			}
			return true;
		}
	}
}