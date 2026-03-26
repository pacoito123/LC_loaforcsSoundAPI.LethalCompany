using GameNetcodeStuff;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Contexts;

public struct PlayerContext(AudioSource? source, PlayerControllerB? player) : IContext {
	public readonly AudioSource? Source => source;
	public readonly PlayerControllerB? Player => player;
}