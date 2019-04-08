using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLeaf_SetTargetToHealthpack : BaseNode
{
	private Blackboard blackboard;

	// Initialise happens here
	public BTLeaf_SetTargetToHealthpack(Blackboard bb)
	{
		blackboard = bb;
		blackboard.SetValueOnBlackboard(Blackboard.CURRENT_PATHFINDING_TARGET, blackboard.GetObjectFromBlackBoard<Pathfinding>(Blackboard.PATHFINDING_COMP).
			GetFurthestRoom(blackboard.GetCurrentAgent().GetPos()));
	}

	// Update func
	public override BTNodeState Evaluate()
	{
		Pathfinding pf = blackboard.GetObjectFromBlackBoard<Pathfinding>(Blackboard.PATHFINDING_COMP);

		Vector3 roomClosestToAgent = pf.GetRoomCenter(pf.GetClosestRoom(blackboard.GetCurrentAgent().GetPos()));

		Vector3 targetPos = pf.GetClosestHealthpack(roomClosestToAgent);
		blackboard.SetValueOnBlackboard(Blackboard.CURRENT_PATHFINDING_TARGET, targetPos);

		return BTNodeState.success;
	}
}