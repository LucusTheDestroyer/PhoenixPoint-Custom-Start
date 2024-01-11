using Base.Defs;
using HarmonyLib;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Tactical.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CustomStartingSquad
{
    public static class StartingTemplates
    {
        private static readonly DefRepository Repo = CustomStartingSquadMain.Repo;
        private static readonly CustomStartingSquadConfig Config = CustomStartingSquadMain.Main.Config as CustomStartingSquadConfig;

        internal enum Characters { Sophia, Jacob, Omar, Takeshi, Irina, Scarab}
        internal static readonly Dictionary<Characters, TacCharacterDef> StaticTemplates = new Dictionary<Characters, TacCharacterDef>
        {
            {Characters.Sophia, (TacCharacterDef)Repo.GetDef("400f644c-41f2-c534-1b99-34d48400b7f7")}, //"PX_Sophia_Tutorial2_TacCharacterDef"
            {Characters.Jacob, null}, 
            {Characters.Omar, (TacCharacterDef)Repo.GetDef("8c9986d9-d875-e0e4-8978-578af6eba952")}, //"PX_Omar_Tutorial3_TacCharacterDef"
            {Characters.Takeshi, (TacCharacterDef)Repo.GetDef("d008b763-7eac-e7f4-e9c4-57eec8bb0c1e")}, //"PX_Takeshi_Tutorial3_TacCharacterDef"
            {Characters.Irina, (TacCharacterDef)Repo.GetDef("e3c06e40-0543-fa04-5a9d-7ff43410b1e0")}, //"PX_Irina_Tutorial3_TacCharacterDef"
            {Characters.Scarab, (TacCharacterDef)Repo.GetDef("05dc56a1-fc93-4f84-4ac2-42104505d8b7")} //"PX_Scarab_CharacterTemplateDef"
        };
        
        internal static readonly Dictionary<CustomStartingSquadConfig.AdditionalTemplates, TacCharacterDef> StandardTemplates = new Dictionary<CustomStartingSquadConfig.AdditionalTemplates, TacCharacterDef>
        {
            {CustomStartingSquadConfig.AdditionalTemplates.Assault, (TacCharacterDef)Repo.GetDef("c777d353-484f-dc24-d98f-e5c35887e200")}, //"PX_AssaultStarting_TacCharacterDef"
            {CustomStartingSquadConfig.AdditionalTemplates.Sniper, (TacCharacterDef)Repo.GetDef("dd03b24f-bc6d-7564-3bd9-94cf52b2665a")}, //"PX_SniperStarting_TacCharacterDef"
            {CustomStartingSquadConfig.AdditionalTemplates.Heavy, (TacCharacterDef)Repo.GetDef("da34dfc5-c90f-c684-7aaa-823ccfec549a")}, //"PX_HeavyStarting_TacCharacterDef"
        };

        internal static List<CustomStartingSquadConfig.AdditionalTemplates> SoldierList;

        public static void Update()
        {
            StaticTemplates[Characters.Jacob] = SniperJacob();
            UpdateStats();
            UpdateList();
            foreach(GameDifficultyLevelDef difficulty in Repo.GetAllDefs<GameDifficultyLevelDef>())
            {
                difficulty.StartingSquadGenerationParams.HasInventoryItems = true;
                difficulty.StartingSquadGenerationParams.HasConsumableItems = true;
                difficulty.StartingSquadTemplate = new TacCharacterDef[]
                {
                    StaticTemplates[Characters.Sophia],
                    StaticTemplates[Characters.Jacob],
                    StaticTemplates[Characters.Omar],
                };
                if(Config.StartWithScarab)
                {
                    difficulty.StartingSquadTemplate = difficulty.StartingSquadTemplate.AddToArray(StaticTemplates[Characters.Scarab]);
                    continue;
                }
                if(Config.StartWithTakeshi)
                {
                    difficulty.StartingSquadTemplate = difficulty.StartingSquadTemplate.AddToArray(StaticTemplates[Characters.Takeshi]);
                }
                if(Config.StartWithIrina)
                {
                    difficulty.StartingSquadTemplate = difficulty.StartingSquadTemplate.AddToArray(StaticTemplates[Characters.Irina]);
                }
                foreach(CustomStartingSquadConfig.AdditionalTemplates additionalSoldier in SoldierList)
                {
                    if(difficulty.StartingSquadTemplate.Count() < 6 && additionalSoldier != CustomStartingSquadConfig.AdditionalTemplates.None)
                    {
                        difficulty.StartingSquadTemplate = difficulty.StartingSquadTemplate.AddToArray(StandardTemplates[additionalSoldier]);
                    }
                }
            }
        }

        public static TacCharacterDef SniperJacob()
        {
            TacCharacterDef JacobV2 = (TacCharacterDef)Repo.GetDef("d1aac8e5-f136-4968-8b58-b489758a5a03");
            if(JacobV2 == null)
            {
                TacCharacterDef Jacob = (TacCharacterDef)Repo.GetDef("2f7a41a8-d68a-3374-1a13-16f18425d7bb"); //"PX_Jacob_Tutorial2_TacCharacterDef"

                JacobV2 = Repo.CreateDef<TacCharacterDef>("d1aac8e5-f136-4968-8b58-b489758a5a03", Jacob);
                JacobV2.Data.GameTags[0] = StaticTemplates[Characters.Irina].Data.GameTags[0];

                JacobV2.Data.BodypartItems = StaticTemplates[Characters.Irina].Data.BodypartItems;
                JacobV2.Data.EquipmentItems = StaticTemplates[Characters.Irina].Data.EquipmentItems;
                JacobV2.Data.Abilites[0] = StaticTemplates[Characters.Irina].Data.Abilites[0];
            }
            return JacobV2;
        }

        public static void UpdateStats()
        {
            if(Config.StartingStats == CustomStartingSquadConfig.StartingModifier.DifficultyScaled)
            {
                foreach(TacCharacterDef template in StaticTemplates.Values)
                {
                    template.Data.Strength = 0;
                    template.Data.Will = 0;
                    template.Data.Speed = 0;
                    template.Data.CurrentHealth = -1;
                    template.Data.CurrentWP = -1;
                }
            }
            else
            {
                StaticTemplates[Characters.Sophia].Data.Strength = 8;
                StaticTemplates[Characters.Sophia].Data.Will = 5;
                StaticTemplates[Characters.Sophia].Data.Speed = 1;
                StaticTemplates[Characters.Sophia].Data.CurrentHealth = 220;

                StaticTemplates[Characters.Jacob].Data.Strength = 9;
                StaticTemplates[Characters.Jacob].Data.Will = 4;
                StaticTemplates[Characters.Jacob].Data.Speed = 1;
                StaticTemplates[Characters.Jacob].Data.CurrentHealth = 230;

                StaticTemplates[Characters.Omar].Data.Strength = 10;
                StaticTemplates[Characters.Omar].Data.Will = 3;
                StaticTemplates[Characters.Omar].Data.CurrentHealth = 240;

                StaticTemplates[Characters.Takeshi].Data.Strength = 9;
                StaticTemplates[Characters.Takeshi].Data.Will = 3;
                StaticTemplates[Characters.Takeshi].Data.CurrentHealth = 230;

                StaticTemplates[Characters.Irina].Data.Strength = 8;
                StaticTemplates[Characters.Irina].Data.Will = 3;
                StaticTemplates[Characters.Irina].Data.Speed = 1;
                StaticTemplates[Characters.Irina].Data.CurrentHealth = 220;
            }
        }

        public static void UpdateList()
        {
            SoldierList = new List<CustomStartingSquadConfig.AdditionalTemplates>
            {
                Config.FirstAdditionalSoldier,
                Config.SecondAdditionalSoldier,
                Config.ThirdAdditionalSoldier
            };
        }
    }
}