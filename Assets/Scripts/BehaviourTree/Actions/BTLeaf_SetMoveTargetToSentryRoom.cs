using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLeaf_SetMoveTargetToSentryRoom : BaseNode
{
	private Blackboard blackboard;

	// Initialise happens here
	public BTLeaf_SetMoveTargetToSentryRoom(Blackboard bb)
	{
		blackboard = bb;
		blackboard.SetValueOnBlackboard(Blackboard.CURRENT_TARGET, blackboard.GetObjectFromBlackBoard<Vector3>(Blackboard.ENEMY_ROOM_TO_SENTRY));
	}

	// Update func
	public override BTNodeState Evaluate()
	{
		blackboard.SetValueOnBlackboard(Blackboard.CURRENT_TARGET, blackboard.GetObjectFromBlackBoard<Vector3>(Blackboard.ENEMY_ROOM_TO_SENTRY));

		return BTNodeState.success;
	}
}
