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
	protected override string ValidateWarnMessage => $"Value field for an AudioReverbCondition in SoundPack '{Pack.Name}' is empty or missing!";

	static ReverbPreset[] _allReverbPresets;
	static Dictionary<string, ReverbPreset> _cachedReverbPresets;

	/// <inheritdoc/>
	public override void OnRegistered() {
		StartOfRoundPatch.StartOfRoundAwake -= PopulateValues;
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
	protected override bool TryCacheValue(out ReverbPreset value, string match) {
		value = null!;

		if (_cachedReverbPresets == null || _allReverbPresets == null) return false;

		match = match.ToLowerInvariant();
		if (!_cachedReverbPresets.TryGetValue(match, out value)) {
			for (int i = _allReverbPresets.Length - 1; i >= 0; i--) {
				value = _allReverbPresets[i];
				if (value != null && string.Equals(value.name, match, StringComparison.InvariantCultureIgnoreCase)
					&& _cachedReverbPresets.TryAdd(match, value)) break;
				value = null!;
			}
		}

		return value != null;
	}

	/// <inheritdoc/>
	protected override bool TryObtainValueWithContext(out ReverbPreset value, PlayerContext context) {
		value = null;
		if (!context.Player) return false;
		value = context.Player.reverbPreset;
		return value != null;
	}

	/// <inheritdoc/>
	public override bool EvaluateFallback(IContext context) {
		if (!GameNetworkManager.Instance) return false;
		return EvaluateWithContext(new PlayerContext(context?.Source, GameNetworkManager.Instance.localPlayerController));
	}
}