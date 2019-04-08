using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryAgent : EnemyController
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
			CreateFleeBehaviour(),
			CreateAttackBehaviour(),
			CreateSentryBehaviour(),
			CreateMoveBackToSentryBehaviour()
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
			// collision sphere check here
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
}
