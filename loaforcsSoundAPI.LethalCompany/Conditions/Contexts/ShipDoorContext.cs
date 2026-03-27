using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Contexts;

public struct ShipDoorContext(AudioSource? source, HangarShipDoor? shipDoor) : IContext {
	public static HangarShipDoor? FallbackShipDoor { get; internal set; }
    public readonly AudioSource? Source => source;
    public readonly HangarShipDoor? ShipDoor => shipDoor;
}