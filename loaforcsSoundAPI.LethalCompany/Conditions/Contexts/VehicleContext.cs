using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Contexts;

public struct VehicleContext(AudioSource source, VehicleController vehicle) : IContext {
    public static VehicleController FallbackVehicle { get; internal set; }
    public readonly AudioSource Source => source;
    public readonly VehicleController Vehicle => vehicle;
}