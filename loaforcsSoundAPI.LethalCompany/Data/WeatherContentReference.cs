using System;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Data;

namespace loaforcsSoundAPI.LethalCompany.Data;

public class WeatherContentReference : ContentReference<LevelWeatherType> {
    public WeatherContentReference(string input) : base(input) => StartOfRoundPatch.StartOfRoundAwake += Resolve;

    /// <inheritdoc/>
    protected override void OnResolved(bool success) => StartOfRoundPatch.StartOfRoundAwake -= Resolve;

    /// <inheritdoc/>
    protected override bool TryResolve(string input, out LevelWeatherType value) => Enum.TryParse(input, ignoreCase: true, out value);
}