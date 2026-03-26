using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions;

[SoundAPICondition("LethalCompany:occlude_audio")]
public class OccludeAudioCondition : Condition {
	public bool? Value { get; internal set; }

	/// <inheritdoc/>
	public override bool Evaluate(IContext context) {
		return context.Source != null && context.Source.TryGetComponent(out OccludeAudio occludeAudio) 
			&& occludeAudio.occluded == (Value ?? true);
	}
}