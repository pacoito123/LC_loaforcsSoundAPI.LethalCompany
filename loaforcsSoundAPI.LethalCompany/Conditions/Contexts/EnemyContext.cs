using System;
using System.Collections.Generic;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Contexts;

public struct EnemyContext(AudioSource? source, EnemyAI? enemy) : IContext {
	public readonly AudioSource? Source => source;
	public readonly EnemyAI? Enemy => enemy;
	public readonly EnemyType? EnemyType => (enemy != null) ? enemy.enemyType : null;

	internal static EnemyType[]? _allEnemyTypes;
	internal static Dictionary<string, EnemyAI>? _cachedEnemies;

	internal static bool TryFindEnemy(string enemyName, out EnemyAI enemy) {
		_allEnemyTypes ??= Resources.FindObjectsOfTypeAll<EnemyType>();
		_cachedEnemies ??= new(_allEnemyTypes.Length);

		enemyName = enemyName.ToLowerInvariant();
		if(!_cachedEnemies.TryGetValue(enemyName, out enemy)) {
			foreach(EnemyType enemyType in _allEnemyTypes) {
				if(enemyType != null && string.Equals(enemyType.enemyName, enemyName, StringComparison.InvariantCultureIgnoreCase)
					&& enemyType.enemyPrefab != null && enemyType.enemyPrefab.TryGetComponent(out enemy)
					&& _cachedEnemies.TryAdd(enemyName, enemy)) break;
				enemy = null!;
			}
		}

		return enemy != null;
	}
}