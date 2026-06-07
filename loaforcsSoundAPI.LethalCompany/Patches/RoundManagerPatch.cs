using DunGen;
using HarmonyLib;
using loaforcsSoundAPI.LethalCompany.Reporting;
using loaforcsSoundAPI.Reporting;
using loaforcsSoundAPI.LethalCompany.Conditions.Dungeon;
using System;

namespace loaforcsSoundAPI.LethalCompany.Patches;

[HarmonyPatch(typeof(RoundManager))]
static class RoundManagerPatch {
	internal static event Action OnRoundManagerAwake = delegate { };

	[HarmonyPatch(nameof(RoundManager.GenerateNewFloor)), HarmonyPostfix]
	static void Reporting(RuntimeDungeon ___dungeonGenerator, StartOfRound ___playersManager) {
		if (SoundReportHandler.CurrentReport == null) return;

		if (___dungeonGenerator && ___dungeonGenerator.Generator?.DungeonFlow) {
			string dungeonName = ___dungeonGenerator.Generator.DungeonFlow.name;
			LethalCompanySoundReport.foundDungeonTypes.Add(dungeonName);
		}
		if (___playersManager && ___playersManager.currentLevel) {
			string moonName = ___playersManager.currentLevel.name;
			LethalCompanySoundReport.foundMoonNames.Add(moonName);
		}
	}

	[HarmonyPatch(nameof(RoundManager.Awake)), HarmonyPostfix]
	static void ListenForPowerChanges(PowerSwitchEvent ___onPowerSwitch) {
		___onPowerSwitch.AddListener(static power => DungeonPowerStateCondition.CurrentPowerState = power);
		OnRoundManagerAwake();
	}
}