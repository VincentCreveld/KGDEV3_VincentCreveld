using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLeaf_SetTargetPlayer : BaseNode
{
	private Blackboard blackboard;

	// Initialise happens here
	public BTLeaf_SetTargetPlayer(Blackboard bb)
	{
		blackboard = bb;
		blackboard.SetValueOnBlackboard(Blackboard.CURRENT_TARGET, bb.GetCurrentPlayer().GetPos());
	}

	// Update func
	public override BTNodeState Evaluate()
	{
		if(blackboard.GetCurrentPlayer() == null)
			return BTNodeState.failure;

		blackboard.SetValueOnBlackboard(Blackboard.CURRENT_TARGET, blackboard.GetCurrentPlayer().GetPos());

		return BTNodeState.success;
	}
}
