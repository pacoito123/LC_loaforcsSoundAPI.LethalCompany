using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions;

[SoundAPICondition("LethalCompany:occlude_audio")]
public class OccludeAudioCondition : Condition {
    public bool? Value { get; private set; }

    /// <inheritdoc/>
    public override bool Evaluate(IContext context) {
        if (!context.Source) return false;
        if (!context.Source.TryGetComponent(out OccludeAudio occludeAudio)) return false;
        return occludeAudio.occluded == (Value ?? true);
    }
}