using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLeaf_SetNewPatrolTarget : BaseNode
{
	private Blackboard blackboard;

	// Initialise happens here
	public BTLeaf_SetNewPatrolTarget(Blackboard bb)
	{
		blackboard = bb;
		blackboard.SetValueOnBlackboard(Blackboard.CURRENT_PATHFINDING_TARGET, blackboard.GetObjectFromBlackBoard<List<Vector3>>(Blackboard.ENEMY_PATROL_NODES)[1]);
	}

	// Update func
	public override BTNodeState Evaluate()
	{
		Vector3 curTarget = blackboard.GetObjectFromBlackBoard<Vector3>(Blackboard.CURRENT_PATROL_TARGET);

		List<Vector3> patrolTargets = blackboard.GetObjectFromBlackBoard<List<Vector3>>(Blackboard.ENEMY_PATROL_NODES);

		Debug.Log(patrolTargets[0]);
		Debug.Log(patrolTargets[1]);

		Vector3 newTarget = (curTarget == patrolTargets[0]) ? patrolTargets[1] : patrolTargets[0];

		blackboard.SetValueOnBlackboard(Blackboard.CURRENT_PATHFINDING_TARGET, newTarget);
		blackboard.SetValueOnBlackboard(Blackboard.CURRENT_PATROL_TARGET, newTarget);

		return BTNodeState.success;
	}
}
