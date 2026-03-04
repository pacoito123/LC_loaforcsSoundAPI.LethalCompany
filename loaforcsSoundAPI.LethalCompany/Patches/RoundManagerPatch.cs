using HarmonyLib;
using loaforcsSoundAPI.LethalCompany.Reporting;
using loaforcsSoundAPI.Reporting;
using loaforcsSoundAPI.LethalCompany.Conditions.Dungeon;
using System;

namespace loaforcsSoundAPI.LethalCompany.Patches;

[HarmonyPatch(typeof(RoundManager))]
static class RoundManagerPatch {
	internal static event Action? OnRoundManagerAwake;

	[HarmonyPatch(nameof(RoundManager.GenerateNewFloor)), HarmonyPostfix]
	static void Reporting(RoundManager __instance) {
		if(SoundReportHandler.CurrentReport == null) return;

		string dungeonName = __instance.dungeonGenerator.Generator.DungeonFlow.name;
		string moonName = StartOfRound.Instance.currentLevel.name;

		_ = LethalCompanySoundReport.foundDungeonTypes.Add(dungeonName);
		_ = LethalCompanySoundReport.foundMoonNames.Add(moonName);
	}

	[HarmonyPatch(nameof(RoundManager.Awake)), HarmonyPostfix]
	static void RoundManagerAwakePost(RoundManager __instance) {
		__instance.onPowerSwitch.AddListener(static power => DungeonPowerStateCondition.CurrentPowerState = power);
		OnRoundManagerAwake?.Invoke();
	}
}