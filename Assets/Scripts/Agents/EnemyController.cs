using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Pathfinding))]
public class EnemyController : GameEntity, IDamagable, IHealable
{
	public Transform graphics;

	protected Pathfinding pathfindingComp;

	public PlayerController playerObject;

	protected Blackboard blackboard;

	protected SelectorNode behaviourTree;

	public Text hp;

	public float maxHealth = Blackboard.ENEMY_MAX_HEALTH;
	protected float currentHealth;
	public float CurrentHealth
	{
		get
		{
			return currentHealth;
		}
		set
		{
			currentHealth = value;
			if(currentHealth >= maxHealth)
				currentHealth = maxHealth;

			if(currentHealth <= 0)
				Destroy(gameObject);
		}
	}

	public virtual void Initialise(List<Healthpack> hps)
	{

	}

	protected virtual void UpdateBlackboard()
	{

	}

	protected virtual void Update()
	{
		hp.text = "HP: " + (int)currentHealth;
	}

	protected virtual void SetupBT() { }

	protected SequenceNode CreateAttackBehaviour()
	{
		return new SequenceNode(
			new BTLeaf_AttackRangeCheck(blackboard, Blackboard.E_ATTACK_RANGE),
			new BTLeaf_SetPathfindingTarget(blackboard),
			new BTLeaf_MoveEnemyObjectAlongPath(blackboard),
			new BTLeaf_AttackTarget(blackboard)
			);
	}

	public override Vector3 GetPos()
	{
		return transform.position;
	}

	public void LookatGraphics(Vector3 pos)
	{
		if(graphics != null)
			graphics.LookAt(pos);
	}

	public void DealDamage(float val)
	{
		CurrentHealth -= val;
	}

	public void Heal(float val)
	{
		CurrentHealth += val;
	}
}
