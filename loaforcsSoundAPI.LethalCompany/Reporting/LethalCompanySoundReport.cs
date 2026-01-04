using System.Collections.Generic;
using System.Linq;
using loaforcsSoundAPI.LethalCompany.Compatibility;
using loaforcsSoundAPI.LethalCompany.Conditions;
using loaforcsSoundAPI.LethalCompany.Conditions.Player;
using loaforcsSoundAPI.LethalCompany.Conditions.Ship;
using loaforcsSoundAPI.LethalCompany.Conditions.Vehicle;
using loaforcsSoundAPI.Reporting;

namespace loaforcsSoundAPI.LethalCompany.Reporting;

static class LethalCompanySoundReport {
	internal static readonly HashSet<string> foundDungeonTypes = [];
	internal static readonly HashSet<string> foundMoonNames = [];
	internal static readonly HashSet<ReverbPreset> foundReverbPresets = [];
	internal static readonly HashSet<FootstepSurface> foundFootstepSurfaces = [];
	internal static readonly Dictionary<EnemyType, EnemyBehaviourState[]> foundEnemyBehaviourStates = [];

	internal static void Init() {
		SoundReportHandler.AddReportSection("Lethal Company", (stream, _) => {
			stream.WriteLine($"Version: `{MyPluginInfo.PLUGIN_VERSION}` <br/>");

			SoundReportHandler.WriteList("Found Dungeon Types", stream, foundDungeonTypes);
			SoundReportHandler.WriteList("Found Moon Names", stream, foundMoonNames);
			SoundReportHandler.WriteList("Found Reverb Presets", stream, [.. foundReverbPresets.Select(ReverbPresetToHumanReadable)]);
			SoundReportHandler.WriteList("Found Footstep Surfaces", stream, [.. foundFootstepSurfaces.Select(it => it.surfaceTag)]);

			foreach (EnemyType enemyType in foundEnemyBehaviourStates.Keys) {
				SoundReportHandler.WriteList($"Found '{enemyType.enemyName}' Behaviour States", stream, [.. foundEnemyBehaviourStates[enemyType].Select(enemyState => enemyState.name)]);
			}

			SoundReportHandler.WriteEnum<PlayerLocationCondition.LocationType>("Player Location Types", stream);
			SoundReportHandler.WriteEnum<ApparatusStateCondition.StateType>("Apparatus State Types", stream);
			SoundReportHandler.WriteEnum<DayMode>("Time Of Day Types", stream); // :skull:
			SoundReportHandler.WriteEnum<ShipStateCondition.ShipStateType>("Ship State Types", stream);
			SoundReportHandler.WriteEnum<VehicleRidingCondition.RiderType>("Vehicle Riding Types", stream);

			if (LethalLevelLoaderCompatibility.Enabled) {
				LethalLevelLoaderCompatibility.WriteLLLDataToReport(stream);
			}
		});
	}

	static string ReverbPresetToHumanReadable(ReverbPreset preset) {
		string result = preset.name + "<br/>\n";
		result += $"hasEcho: {preset.hasEcho} <br/>\n";
		result += $"changeRoom: {preset.changeRoom}, room: {preset.room} <br/>\n";
		result += $"changeDecayTime: {preset.changeDecayTime}, decayTime: {preset.decayTime} <br/>\n";
		result += $"changeDryLevel: {preset.changeDryLevel}, dryLevel: {preset.dryLevel} <br/>\n";
		result += $"changeHighFreq: {preset.changeHighFreq}, highFreq: {preset.highFreq} <br/>\n";
		result += $"changeLowFreq: {preset.changeLowFreq}, lowFreq: {preset.lowFreq} <br/>\n";

		return result;
	}
}