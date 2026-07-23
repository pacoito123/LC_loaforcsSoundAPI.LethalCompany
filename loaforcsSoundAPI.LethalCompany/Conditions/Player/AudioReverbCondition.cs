using System.Collections.Generic;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:audio_reverb")]
[SoundAPICondition("LethalCompany:player:reverb_preset")]
public class AudioReverbCondition : Condition<PlayerContext> {
	public List<ReverbPresetContentReference> Value { get; private set; }

	/// <inheritdoc/>
	public override bool EvaluateWithContext(PlayerContext context) {
		if (!context.Player) return false;

		return Value.Find(reference => reference.Value == context.Player.reverbPreset) != null;
	}

	/// <inheritdoc/>
	public override bool EvaluateFallback(IContext context) {
		if (!GameNetworkManager.Instance) return false;
		if (!GameNetworkManager.Instance.localPlayerController) return false;

		return EvaluateWithContext(new PlayerContext(context?.Source, GameNetworkManager.Instance.localPlayerController));
	}
}