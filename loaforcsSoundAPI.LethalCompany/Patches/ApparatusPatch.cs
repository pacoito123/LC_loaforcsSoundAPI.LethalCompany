using HarmonyLib;
using loaforcsSoundAPI.LethalCompany.Conditions;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Patches;

[HarmonyPatch(typeof(LungProp))]
static class ApparatusPatch {
	[HarmonyPatch(nameof(LungProp.EquipItem)), HarmonyPostfix]
	static void OnApparatusPull(Coroutine ___disconnectAnimation) {
		if (___disconnectAnimation == null) return;

		ApparatusStateCondition.CurrentApparatusPulled = true;
	}
}