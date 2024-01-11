using PhoenixPoint.Modding;

namespace CustomStartingSquad
{
	/// <summary>
	/// ModConfig is mod settings that players can change from within the game.
	/// Config is only editable from players in main menu.
	/// Only one config can exist per mod assembly.
	/// Config is serialized on disk as json.
	/// </summary>
	public class CustomStartingSquadConfig : ModConfig
	{
		/// Only public fields are serialized.
		/// Supported types for in-game UI are:

		[ConfigField(text: "Start with a Scarab", description: "Your starting squad will include a Scarab. Overrides other squad settings.")]
		public bool StartWithScarab;

		[ConfigField(text: "Start with Takeshi", description: "Start with the additional Assault that features in the Tutorial Base Mission.")]
		public bool StartWithTakeshi;

		[ConfigField(text: "Start with Irina", description: "Start with the additional Sniper that features in the Tutorial Base Mission.")]
		public bool StartWithIrina;

		[ConfigField(text: "Starting Base Stats", description: "Tutorial templates have additional base stats. You can choose to keep them or scale the units by difficulty. Non-tutorial templates will scale by difficulty.")]
		public StartingModifier StartingStats = StartingModifier.DifficultyScaled;
		
		[ConfigField(text: "First Additional Soldier", description: "Allows you to fill the remaining seat(s) of the Manticore with a class of your choosing. Once full, the game will ignore further templates. Select 'None' to leave any of the remaining seats empty.")]
		public AdditionalTemplates FirstAdditionalSoldier = AdditionalTemplates.Assault;
		
		[ConfigField(text: "Second Additional Soldier", description: "Allows you to fill the remaining seat(s) of the Manticore with a class of your choosing. Once full, the game will ignore further templates. Select 'None' to leave any of the remaining seats empty.")]
		public AdditionalTemplates SecondAdditionalSoldier = AdditionalTemplates.None;
		
		[ConfigField(text: "Third Additional Soldier", description: "Allows you to fill the remaining seat(s) of the Manticore with a class of your choosing. Once full, the game will ignore further templates. Select 'None' to leave any of the remaining seats empty.")]
		public AdditionalTemplates ThirdAdditionalSoldier = AdditionalTemplates.None;

		public enum StartingModifier
		{
			TutorialBuffed, DifficultyScaled
		}

		public enum AdditionalTemplates
		{
			None, Assault, Heavy, Sniper, 
		}
	}
}
