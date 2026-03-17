using System;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine.SceneManagement;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Moon;

[SoundAPICondition("LethalCompany:weather:name")]
public class WeatherNameCondition : MultipleCondition<LevelWeatherType> {
    /// <inheritdoc/>
    protected override void OnRegistered() {
        if(string.IsNullOrEmpty(Value)) return;
        StartOfRoundPatch.StartOfRoundAwake += PopulateValues;
    }

    /// <inheritdoc/>
    protected override void OnValuesPopulated() {
        StartOfRoundPatch.StartOfRoundAwake -= PopulateValues;
    }

    /// <inheritdoc/>
    protected override bool TryGetValue(out LevelWeatherType weather, string match) {
        return Enum.TryParse(match, ignoreCase: true, out weather);
    }

    /// <inheritdoc/>
    protected override bool CheckValue(LevelWeatherType weather) {
        return SceneManager.loadedSceneCount > 1 && StartOfRound.Instance != null && StartOfRound.Instance.currentLevel != null
            && StartOfRound.Instance.currentLevel.currentWeather == weather;
    }
}