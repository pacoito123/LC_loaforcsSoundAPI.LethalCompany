using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine.SceneManagement;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Moon;

[SoundAPICondition("LethalCompany:moon:current_time")]
public class CurrentTimeCondition : RangeCondition<float> {
    /// <inheritdoc/>
    protected override RangeOperator<float> DefaultRange => new(100.0f, 1080.0f);

    /// <inheritdoc/>
    public override bool Evaluate(IContext context) {
        if (SceneManager.loadedSceneCount <= 1) return false;
        if (!TimeOfDay.Instance) return false;
        return EvaluateRangeOperator(TimeOfDay.Instance.globalTime);
    }

    /// <inheritdoc/>
    protected override bool TryParseValue(string parameter, ref float value) {
        return string.IsNullOrEmpty(parameter) || float.TryParse(parameter, out value);
    }
}