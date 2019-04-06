using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLeaf_SetPathfindingTarget : BaseNode
{
	private Blackboard blackboard;

	public BTLeaf_SetPathfindingTarget(Blackboard bb)
	{
		blackboard = bb;
	}

	public override BTNodeState Evaluate()
	{
		SetMovementTargetToPlayer();
		return BTNodeState.success;
	}

	private void SetMovementTargetToPlayer()
	{
		Debug.Log("Setting new pathfinding target to " + blackboard.GetCurrentPlayer().GetPos());

		blackboard.GetObjectFromBlackBoard<Pathfinding>(Blackboard.PATHFINDING_COMP).
			FindPath(
			blackboard.GetCurrentAgent().GetPos(),
			blackboard.GetCurrentPlayer().GetPos()
			);

		blackboard.path = blackboard.GetObjectFromBlackBoard<Pathfinding>(Blackboard.PATHFINDING_COMP).GetPath();
	}
}
