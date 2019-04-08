using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterAgent : EnemyController
{
	protected override void Update()
	{
		base.Update();
		// Keep the reference values updates here.
		UpdateBlackboard();

		// This runs the BT.
		behaviourTree.Evaluate();
	}

	public override void Initialise(List<Healthpack> hps)
	{
		currentHealth = maxHealth;

		if(playerObject == null)
		{
			Debug.Log("No referenced player on enemy. Stopping initialisation");
			return;
		}
		pathfindingComp = GetComponent<Pathfinding>();

		blackboard = new Blackboard(pathfindingComp, playerObject, this);

		List<Vector3> healthpackLocations = new List<Vector3>();

		foreach(Healthpack hp in hps)
		{
			healthpackLocations.Add(hp.GetPos());
		}

		blackboard.SetValueOnBlackboard(Blackboard.HEALTHPACK_LOCATIONS, healthpackLocations);

		SetupBT();
		UpdateBlackboard();
	}

	protected override void UpdateBlackboard()
	{
		blackboard.SetValueOnBlackboard(Blackboard.PLAYER_CURHEALTH, playerObject.CurrentHealth);

		blackboard.SetValueOnBlackboard(Blackboard.ENEMY_CURHEALTH, CurrentHealth);
	}

	protected override void SetupBT()
	{
		behaviourTree = new SelectorNode(
			CreateHealthpackBehaviour(),
			CreateAttackBehaviour(),
			CreateHuntBehaviour(),
			CreatePatrolBehaviour()
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

	private SequenceNode CreateHealthpackBehaviour()
	{
		return new SequenceNode(
			new BTLeaf_CheckOwnHPLow(blackboard),
			new BTLeaf_SetTargetToHealthpack(blackboard),
			new BTLeaf_SetPathfindingTarget(blackboard),
			new BTLeaf_MoveEnemyObjectAlongPath(blackboard),
			new BTLeaf_UseHealthpack(blackboard, 3f)
			);
	}

	private SequenceNode CreatePatrolBehaviour()
	{
		int r1 = UnityEngine.Random.Range(1, pathfindingComp.levelManager.roomCenters.Count);
		int r2 = UnityEngine.Random.Range(1, pathfindingComp.levelManager.roomCenters.Count);

		// Duplication prevention
		while(r2 == r1)
		{
			r2 = UnityEngine.Random.Range(1, pathfindingComp.levelManager.roomCenters.Count);
		}

		List<Vector3> patrolPoints = new List<Vector3>();
		patrolPoints.Add(pathfindingComp.levelManager.roomCenters[r1]);
		patrolPoints.Add(pathfindingComp.levelManager.roomCenters[r2]);

		transform.position = patrolPoints[0];

		blackboard.SetValueOnBlackboard(Blackboard.ENEMY_PATROL_NODES, patrolPoints);

		blackboard.SetValueOnBlackboard(Blackboard.CURRENT_PATROL_TARGET, patrolPoints[1]);

		return new SequenceNode(
			new AlwaysSuccessNode(
			new SequenceNode(
				new BTLeaf_IsAgentAtPatrolPoint(blackboard),
				new BTLeaf_SetNewPatrolTarget(blackboard)
				)),
				new BTLeaf_SetMovementToPatrolTarget(blackboard),
				new BTLeaf_MoveEnemyObjectAlongPath(blackboard)
			);
	}
}
