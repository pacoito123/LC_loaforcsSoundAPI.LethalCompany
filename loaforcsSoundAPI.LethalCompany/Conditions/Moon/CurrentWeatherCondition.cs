using System.Collections.Generic;
using loaforcsSoundAPI.LethalCompany.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine.SceneManagement;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Moon;

[SoundAPICondition("LethalCompany:weather:name")]
public class WeatherNameCondition : Condition {
    public List<WeatherContentReference> Value { get; private set; }

    /// <inheritdoc/>
    public override bool Evaluate(IContext context) {
        if (SceneManager.loadedSceneCount <= 1) return false;
        if (!StartOfRound.Instance) return false;
        if (!StartOfRound.Instance.currentLevel) return false;

        return Value.FindIndex(reference => reference.Value == StartOfRound.Instance.currentLevel.currentWeather) != -1;
    }
}