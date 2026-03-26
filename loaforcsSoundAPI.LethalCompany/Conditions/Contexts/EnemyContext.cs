using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Contexts;

public struct EnemyContext(AudioSource? source, EnemyAI? enemy) : IContext {
	public readonly AudioSource? Source => source;
	public readonly EnemyAI? Enemy => enemy;
	public readonly EnemyType? EnemyType => (enemy != null) ? enemy.enemyType : null;
}