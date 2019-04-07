using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLeaf_SetTargetToFurthestRoom : BaseNode
{
	private Blackboard blackboard;

	// Initialise happens here
	public BTLeaf_SetTargetToFurthestRoom(Blackboard bb)
	{
		blackboard = bb;
		blackboard.SetValueOnBlackboard(Blackboard.CURRENT_TARGET, blackboard.GetObjectFromBlackBoard<Pathfinding>(Blackboard.PATHFINDING_COMP).
			GetFurthestRoom(blackboard.GetCurrentAgent().GetPos()));
	}

	// Update func
	public override BTNodeState Evaluate()
	{
		Pathfinding pf = blackboard.GetObjectFromBlackBoard<Pathfinding>(Blackboard.PATHFINDING_COMP);

		Vector3 roomClosestToPlayer = pf.GetRoomCenter(pf.GetClosestRoom(blackboard.GetCurrentPlayer().GetPos()));

		Vector3 targetPos = pf.GetRoomCenter(pf.GetFurthestRoom(roomClosestToPlayer));
		blackboard.SetValueOnBlackboard(Blackboard.CURRENT_TARGET, targetPos);

		Debug.Log(targetPos);

		return BTNodeState.success;
	}
}
