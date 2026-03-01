using System;
using JetBrains.Annotations;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Moon;

[SoundAPICondition("LethalCompany:weather:name")]
public class WeatherNameCondition : Condition {
    [CanBeNull]
    public string Value { get; internal set; } = null;

    public override bool Evaluate(IContext context) {
        return StartOfRound.Instance != null && StartOfRound.Instance.currentLevel != null
            && string.Equals(Value, $"{StartOfRound.Instance.currentLevel.currentWeather}", StringComparison.InvariantCultureIgnoreCase);
    }
}