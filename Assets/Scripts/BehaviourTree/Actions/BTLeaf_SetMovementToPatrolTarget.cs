using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLeaf_SetMovementToPatrolTarget : BaseNode
{
	private Blackboard blackboard;

	public BTLeaf_SetMovementToPatrolTarget(Blackboard bb)
	{
		blackboard = bb;
	}

	public override BTNodeState Evaluate()
	{
		SetMovementTarget();
		return BTNodeState.success;
	}

	private void SetMovementTarget()
	{
		blackboard.GetObjectFromBlackBoard<Pathfinding>(Blackboard.PATHFINDING_COMP).
			FindPath(
			blackboard.GetCurrentAgent().GetPos(),
			blackboard.GetObjectFromBlackBoard<Vector3>(Blackboard.CURRENT_PATROL_TARGET)
			);

		blackboard.path = blackboard.GetObjectFromBlackBoard<Pathfinding>(Blackboard.PATHFINDING_COMP).GetPath();
	}
}
