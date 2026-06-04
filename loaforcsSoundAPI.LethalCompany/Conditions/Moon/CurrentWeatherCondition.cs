using System;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine.SceneManagement;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Moon;

[SoundAPICondition("LethalCompany:weather:name")]
public class WeatherNameCondition : MultipleCondition<LevelWeatherType> {
    protected override string ValidateWarnMessage => $"Value field for a WeatherNameCondition in SoundPack '{Pack.Name}' is empty or missing!";

    /// <inheritdoc/>
    public override void OnRegistered() {
        StartOfRoundPatch.StartOfRoundAwake -= PopulateValues;
        StartOfRoundPatch.StartOfRoundAwake += PopulateValues;
    }

    /// <inheritdoc/>
    protected override void OnValuesPopulated() => StartOfRoundPatch.StartOfRoundAwake -= PopulateValues;

    /// <inheritdoc/>
    protected override bool TryCacheValue(out LevelWeatherType value, string match) {
        return Enum.TryParse(match, ignoreCase: true, out value);
    }

    /// <inheritdoc/>
    protected override bool TryObtainValue(out LevelWeatherType value, IContext context) {
        value = default;
        if (SceneManager.loadedSceneCount <= 1) return false;
        if (!StartOfRound.Instance) return false;
        if (!StartOfRound.Instance.currentLevel) return false;
        value = StartOfRound.Instance.currentLevel.currentWeather;
        return true;
    }
}