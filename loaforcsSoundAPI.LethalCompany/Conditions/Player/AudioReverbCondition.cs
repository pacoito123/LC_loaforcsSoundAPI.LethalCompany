using System;
using System.Collections.Generic;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:audio_reverb")]
public class AudioReverbCondition : MultipleCondition<ReverbPreset, PlayerContext> {
	private static ReverbPreset[]? _allReverbPresets;
	private static Dictionary<string, ReverbPreset>? _cachedReverbPresets;

	/// <inheritdoc/>
	protected override void OnRegistered() {
		if(string.IsNullOrEmpty(Value)) return;
		StartOfRoundPatch.StartOfRoundAwake += PopulateValues;
	}

	/// <inheritdoc/>
	protected override void PopulateValues() {
		_allReverbPresets ??= Resources.FindObjectsOfTypeAll<ReverbPreset>();
		_cachedReverbPresets ??= new(_allReverbPresets.Length);
		base.PopulateValues();
	}

	/// <inheritdoc/>
	protected override void OnValuesPopulated() {
		StartOfRoundPatch.StartOfRoundAwake -= PopulateValues;
	}

	/// <inheritdoc/>
	protected override bool TryGetValue(out ReverbPreset reverbPreset, string match) {
		reverbPreset = null!;

		if(_cachedReverbPresets == null || _allReverbPresets == null) return false;

		match = match.ToLowerInvariant();
		if(!_cachedReverbPresets.TryGetValue(match, out reverbPreset)) {
			for(int i = _allReverbPresets.Length - 1; i >= 0; i--) {
				reverbPreset = _allReverbPresets[i];
				if(reverbPreset != null && string.Equals(reverbPreset.name, match, StringComparison.InvariantCultureIgnoreCase)
					&& _cachedReverbPresets.TryAdd(match, reverbPreset)) break;
				reverbPreset = null!;
			}
		}

		return reverbPreset != null;
	}

	/// <inheritdoc/>
	protected override bool CheckValueWithContext(ReverbPreset value, PlayerContext context) {
		return context.Source != null && context.Player != null && context.Player.reverbPreset == value;
	}

	/// <inheritdoc/>
	public override bool EvaluateFallback(IContext context) {
		return _currentContext.HasValue ? EvaluateWithContext(_currentContext.Value)
			: GameNetworkManager.Instance != null && GameNetworkManager.Instance.localPlayerController != null
				&& EvaluateWithContext(new(context.Source, GameNetworkManager.Instance.localPlayerController));
	}
}