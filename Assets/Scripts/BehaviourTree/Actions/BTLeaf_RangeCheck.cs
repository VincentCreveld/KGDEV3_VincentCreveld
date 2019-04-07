using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLeaf_RangeCheck : BaseNode
{
	private Blackboard blackboard;

	public BTLeaf_RangeCheck(Blackboard bb, float range)
	{
		blackboard = bb;

		blackboard.SetValueOnBlackboard(Blackboard.ENEMY_ATTACK_RANGE, range);
	}

	public override BTNodeState Evaluate()
	{
		if(blackboard.GetCurrentPlayer() == null)
			return BTNodeState.failure;

		if(Vector3.Distance(blackboard.GetCurrentPlayer().GetPos(),
			blackboard.GetCurrentAgent().GetPos()) < 
			blackboard.GetObjectFromBlackBoard<float>(Blackboard.ENEMY_ATTACK_RANGE))
		{
			return BTNodeState.success;
		}
		else
		{
			return BTNodeState.failure;
		}
	}
}
