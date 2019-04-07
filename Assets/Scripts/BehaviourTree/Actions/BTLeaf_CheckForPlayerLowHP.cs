using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLeaf_CheckForPlayerLowHP : BaseNode
{
	private Blackboard blackboard;

	// Initialise happens here
	public BTLeaf_CheckForPlayerLowHP(Blackboard bb)
	{
		blackboard = bb;
		blackboard.SetValueOnBlackboard(Blackboard.PLAYER_CURHEALTH, Blackboard.PLAYER_MAX_HEALTH);
	}

	// Update func
	public override BTNodeState Evaluate()
	{
		if(blackboard.GetObjectFromBlackBoard<float>(Blackboard.PLAYER_CURHEALTH) < Blackboard.PLAYER_LOW_HEALTH_THRESHOLD)
			return BTNodeState.success;
		else
			return BTNodeState.failure;
	}
}
