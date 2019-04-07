using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pathfinding))]
public class EnemyController : MonoBehaviour, IDamagable
{
	public Transform graphics;

	private Pathfinding pathfindingComp;

	public PlayerController playerObject;

	private Blackboard blackboard;

	private SelectorNode behaviourTree;

	public float maxHealth = Blackboard.ENEMY_MAX_HEALTH;
	private float currentHealth;
	public float CurrentHealth
	{
		get
		{
			return currentHealth;
		}
		set
		{
			currentHealth = value;
			if(currentHealth <= 0)
				Destroy(gameObject);
		}
	}

	public void Initialise()
	{
		currentHealth = maxHealth;

		if(playerObject == null)
		{
			Debug.Log("No referenced player on enemy. Stopping initialisation");
			return;
		}
		pathfindingComp = GetComponent<Pathfinding>();
		
		blackboard = new Blackboard(pathfindingComp, playerObject, this);
		SetupBT();
		UpdateBlackboard();
	}

	private void Update()
	{
		// Keep the reference values updates here.
		UpdateBlackboard();

		// This runs the BT.
		behaviourTree.Evaluate();
	}

	private void UpdateBlackboard()
	{
		blackboard.SetValueOnBlackboard(Blackboard.PLAYER_CURHEALTH, playerObject.CurrentHealth);

		blackboard.SetValueOnBlackboard(Blackboard.ENEMY_CURHEALTH, CurrentHealth);
	}

	private void SetupBT()
	{
		behaviourTree = new SelectorNode(
			CreateFleeBehaviour(),
			CreateAttackBehaviour(),
			CreateHuntBehaviour(),
			CreateMoveBackToSentryBehaviour(),
			CreateSentryBehaviour()
			);
	}

	private SequenceNode CreateHuntBehaviour()
	{
		return new SequenceNode(
			new BTLeaf_CheckForPlayerLowHP(blackboard),
			new BTLeaf_SetTargetPlayer(blackboard),
			new BTLeaf_SetPathfindingTarget(blackboard),
			new BTLeaf_MoveEnemyObjectAlongPath(blackboard)
			);
	}

	private SequenceNode CreateAttackBehaviour()
	{
		return new SequenceNode(
			new BTLeaf_RangeCheck(blackboard, Blackboard.E_ATTACK_RANGE),
			new BTLeaf_AttackPlayer(blackboard)
			);
	}

	private SequenceNode CreateFleeBehaviour()
	{
		return new SequenceNode(
			new BTLeaf_CheckOwnHPLow(blackboard),
			new BTLeaf_SetTargetToFurthestRoom(blackboard),
			new BTLeaf_SetPathfindingTarget(blackboard),
			new BTLeaf_MoveEnemyObjectAlongPath(blackboard)
			);
	}
	
	private SequenceNode CreateSentryBehaviour()
	{
		Vector3 roomToSentry = pathfindingComp.GetRoomCenter(
			pathfindingComp.GetClosestRoom(
				blackboard.GetCurrentAgent().GetPos()));

		return new SequenceNode(
			new BTLeaf_CheckIfPlayerIsInRoom(blackboard, roomToSentry),
			new BTLeaf_SetTargetPlayer(blackboard),
			new BTLeaf_SetPathfindingTarget(blackboard),
			new BTLeaf_MoveEnemyObjectAlongPath(blackboard)
			);
	}

	private SequenceNode CreateMoveBackToSentryBehaviour()
	{
		Vector3 roomToSentry = pathfindingComp.GetRoomCenter(
			pathfindingComp.GetClosestRoom(
				blackboard.GetCurrentAgent().GetPos()));

		return new SequenceNode(
			new InverterNode(new BTLeaf_CheckIfPlayerIsInRoom(blackboard, roomToSentry)),
			new BTLeaf_SetMoveTargetToSentryRoom(blackboard),
			new BTLeaf_SetPathfindingTarget(blackboard),
			new BTLeaf_MoveEnemyObjectAlongPath(blackboard)
			);
	}

	public Vector3 GetPos()
	{
		return transform.position;
	}

	public void LookatGraphics(Vector3 pos)
	{
		graphics.LookAt(pos);
	}

	public void DealDamage(float val)
	{
		CurrentHealth -= val;
	}
}
