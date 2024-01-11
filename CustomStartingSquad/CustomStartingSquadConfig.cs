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
		// public int IntegerValue;
		// public float FloatValue;

		public bool FullManticore;

		public bool StartWithScarab;

		public StartingModifier StartingStats;
		
		public AdditionalTemplates FirstNewSoldier = AdditionalTemplates.Takeshi;
		public AdditionalTemplates SecondAdditionalSoldier = AdditionalTemplates.None;
		public AdditionalTemplates ThirdAdditionalSoldier = AdditionalTemplates.None;

		public enum StartingModifier
		{
			TutorialBuffed, DifficultyScaled
		}

		public enum AdditionalTemplates
		{
			None, Takeshi, Irina, Assault, Heavy, Sniper, 
		}
	}
}
