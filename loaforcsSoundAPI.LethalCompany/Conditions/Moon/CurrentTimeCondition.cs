using loaforcsSoundAPI.SoundPacks.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine.SceneManagement;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Moon;

[SoundAPICondition("LethalCompany:moon:current_time")]
public class CurrentTimeCondition : Condition {
    public RangeOperator<float> Value { get; private set; } = new(100.0f, 1080.0f);

    /// <inheritdoc/>
    public override bool Evaluate(IContext context) {
        if (SceneManager.loadedSceneCount <= 1) return false;
        if (!TimeOfDay.Instance) return false;

        return Value.EvaluateRange(TimeOfDay.Instance.globalTime);
    }
}