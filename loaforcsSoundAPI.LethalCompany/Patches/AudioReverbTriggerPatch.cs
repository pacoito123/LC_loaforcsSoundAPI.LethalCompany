using HarmonyLib;
using loaforcsSoundAPI.LethalCompany.Reporting;
using loaforcsSoundAPI.Reporting;

namespace loaforcsSoundAPI.LethalCompany.Patches;

[HarmonyPatch(typeof(AudioReverbTrigger))]
static class AudioReverbTriggerPatch {
	[HarmonyPatch(nameof(AudioReverbTrigger.ChangeAudioReverbForPlayer)), HarmonyPostfix]
	static void LogFoundReverbPreset(ReverbPreset ___reverbPreset) {
		if (SoundReportHandler.CurrentReport == null) return;
		if (!___reverbPreset) return;

		LethalCompanySoundReport.foundReverbPresets.Add(___reverbPreset);
	}
}