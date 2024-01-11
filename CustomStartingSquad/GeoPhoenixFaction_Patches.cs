using Base.Core;
using Base.Defs;
using Base.Levels;
using HarmonyLib;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Entities.Items;
using PhoenixPoint.Common.Game;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Geoscape.Levels.Factions;
using PhoenixPoint.Modding;
using PhoenixPoint.Tactical.Entities;
using System;
using System.Linq;
using UnityEngine;

namespace CustomStartingSquad
{
    [HarmonyPatch(typeof(GeoPhoenixFaction), "CreateInitialSquad", new Type[]{typeof(GeoSite)})]
    public static class CreateInitialSquad_Patch
    {
        internal static CustomStartingSquadConfig Config = CustomStartingSquadMain.Main.Config as CustomStartingSquadConfig;
        public static bool Prefix(GeoPhoenixFaction __instance, GeoSite site)
        {
            GeoVehicle geoVehicle = __instance.Vehicles.First<GeoVehicle>();
            GameDifficultyLevelDef currentDifficulty = __instance.GeoLevel.CurrentDifficultyLevel;
            foreach(TacCharacterDef template in currentDifficulty.StartingSquadTemplate)
            {
                GeoCharacter character;
                if(StartingTemplates.StaticTemplates.Values.Contains(template))
                {
                    if(Config.StartingStats == CustomStartingSquadConfig.StartingModifier.DifficultyScaled && template.Volume == 1)
                    {
                        character = __instance.GeoLevel.CreateCharacterFromTemplate(template, __instance, null, currentDifficulty.StartingSquadGenerationParams);
                    }
                    else
                    {
                        character = __instance.GeoLevel.CreateCharacterFromTemplate(template, __instance);
                    }
                }
                else
                {
                    GeoUnitDescriptor descriptor = __instance.GeoLevel.CharacterGenerator.GenerateUnit(__instance, template);
                    __instance.GeoLevel.CharacterGenerator.ApplyGenerationParameters(descriptor, currentDifficulty.StartingSquadGenerationParams);
                    __instance.GeoLevel.CharacterGenerator.RandomizeIdentity(descriptor);
                    character = descriptor.SpawnAsCharacter();
                }
                geoVehicle.AddCharacter(character);
            }

            foreach (ItemUnit itemUnit in currentDifficulty.StartingStorage)
            {
                if(__instance.FactionDef.UseGlobalStorage)
                {
                    __instance.ItemStorage.AddItem(new GeoItem(itemUnit));
                }
                else
                {
                    site.ItemStorage.AddItem(new GeoItem(itemUnit));
                }
            }
            return false;
        }
    }
    
}