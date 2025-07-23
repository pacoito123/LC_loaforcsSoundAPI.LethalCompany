using System;
using System.Collections.Generic;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Moon;

[SoundAPICondition("LethalCompany:weather:name")] 
public class WeatherNameCondition : Condition
{
    public string Value { get; internal set; }
    public override bool Evaluate(IContext context)
    {
        if (!StartOfRound.Instance) return false;
        if (!StartOfRound.Instance.currentLevel) return false;
        string weatherName = StartOfRound.Instance.currentLevel.currentWeather.ToString();
        return string.Equals(Value, weatherName, StringComparison.InvariantCultureIgnoreCase);
    }
}