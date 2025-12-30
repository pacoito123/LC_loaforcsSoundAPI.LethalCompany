using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Contexts;

public class EnemyContext(EnemyAI enemy) : IContext {
	public EnemyAI Enemy => enemy;
	public EnemyType EnemyType => enemy.enemyType;
}