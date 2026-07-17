using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Contexts;

public struct ItemContext(AudioSource source, GrabbableObject item) : IContext {
    public readonly AudioSource Source => source;
    public readonly GrabbableObject Item => item;
    public readonly Item ItemProperties => item ? item.itemProperties : null;
}