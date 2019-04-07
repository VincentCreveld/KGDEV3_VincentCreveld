using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLeaf_CheckOwnHPLow : BaseNode
{
	private Blackboard blackboard;

	// Initialise happens here
	public BTLeaf_CheckOwnHPLow(Blackboard bb)
	{
		blackboard = bb;
		blackboard.SetValueOnBlackboard(Blackboard.ENEMY_CURHEALTH, Blackboard.ENEMY_MAX_HEALTH);
	}

	// Update func
	public override BTNodeState Evaluate()
	{
		if(blackboard.GetObjectFromBlackBoard<float>(Blackboard.ENEMY_CURHEALTH) < Blackboard.ENEMY_LOW_HEALTH_THRESHOLD)
			return BTNodeState.success;
		else
			return BTNodeState.failure;
	}
}
