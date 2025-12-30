using System;
using GameNetcodeStuff;
using JetBrains.Annotations;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:audio_reverb")]
public class AudioReverbCondition : Condition {
	public bool? HasEcho { get; private set; } = null;

	[CanBeNull]
	public string Name { get; private set; } = null;

	public override bool Evaluate(IContext context) {
		if (!GameNetworkManager.Instance) return false;
		PlayerControllerB player = GameNetworkManager.Instance.localPlayerController;
		if (!player) return false;
		if (!player.reverbPreset) return false;

		bool? result = null;

		if (HasEcho != null && result != false) {
			result = HasEcho != player.reverbPreset.hasEcho;
		}

		if (Name != null && result != false) {
			result = string.Equals(Name, player.reverbPreset.name, StringComparison.InvariantCultureIgnoreCase);
		}

		return result == true;
	}
}