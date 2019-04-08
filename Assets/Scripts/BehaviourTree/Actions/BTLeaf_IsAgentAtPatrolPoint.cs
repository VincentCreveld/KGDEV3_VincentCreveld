using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLeaf_IsAgentAtPatrolPoint : BaseNode
{
	private Blackboard blackboard;

	public BTLeaf_IsAgentAtPatrolPoint(Blackboard bb)
	{
		blackboard = bb;
	}

	public override BTNodeState Evaluate()
	{
		Vector3 p1 = blackboard.GetCurrentAgent().GetPos();
		Vector3 p2 = blackboard.GetObjectFromBlackBoard<Vector3>(Blackboard.CURRENT_PATROL_TARGET);

		float dist = Vector3.Distance(p1, p2);

		if(dist < 2.5f)
			return BTNodeState.success;
		else
			return BTNodeState.failure;
	}
}
