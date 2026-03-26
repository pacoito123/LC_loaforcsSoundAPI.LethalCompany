using HarmonyLib;
using loaforcsSoundAPI.LethalCompany.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Patches;

[HarmonyPatch(typeof(LungProp))]
static class ApparatusPatch {
	[HarmonyPatch(nameof(LungProp.EquipItem)), HarmonyPostfix]
	static void OnApparatusPull(LungProp __instance) {
		if(__instance != null && __instance.disconnectAnimation != null) {
			ApparatusStateCondition.CurrentApparatusPulled = true;
		}
	}
}