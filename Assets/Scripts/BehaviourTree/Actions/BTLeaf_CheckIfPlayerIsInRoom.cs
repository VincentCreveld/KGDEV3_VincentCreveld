using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLeaf_CheckIfPlayerIsInRoom : BaseNode
{
	private Blackboard blackboard;

	// Initialise happens here
	public BTLeaf_CheckIfPlayerIsInRoom(Blackboard bb, Vector3 roomToSentry)
	{
		blackboard = bb;
		blackboard.SetValueOnBlackboard(Blackboard.ENEMY_ROOM_TO_SENTRY, roomToSentry);
	}

	// Update func
	public override BTNodeState Evaluate()
	{
		Pathfinding pf = blackboard.GetObjectFromBlackBoard<Pathfinding>(Blackboard.PATHFINDING_COMP);

		Vector3 roomClosestToPlayer = pf.GetRoomCenter(pf.GetClosestRoom(blackboard.GetCurrentPlayer().GetPos()));

		Vector3 roomClosestToEnemy = pf.GetRoomCenter(pf.GetClosestRoom(blackboard.GetCurrentAgent().GetPos()));

		if(roomClosestToEnemy == roomClosestToPlayer)
			return BTNodeState.success;
		else
			return BTNodeState.failure;
	}
}
