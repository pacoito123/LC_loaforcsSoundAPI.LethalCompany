using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Contexts;

public struct DropshipContext(AudioSource? source, ItemDropship? dropship) : IContext {
    public static ItemDropship? FallbackDropship { get; internal set; }
    public readonly AudioSource? Source => source;
    public readonly ItemDropship? Dropship => dropship;
}