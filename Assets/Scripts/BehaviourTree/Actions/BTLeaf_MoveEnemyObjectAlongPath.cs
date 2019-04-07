using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLeaf_MoveEnemyObjectAlongPath : BaseNode
{
	private Blackboard blackboard;

	public BTLeaf_MoveEnemyObjectAlongPath(Blackboard bb)
	{
		blackboard = bb;

		blackboard.SetValueOnBlackboard(Blackboard.ENEMY_ATTACK_RANGE, Blackboard.E_ATTACK_RANGE);
		blackboard.SetValueOnBlackboard(Blackboard.MOVESPEED, Blackboard.E_MOVESPEED);
	}

	public override BTNodeState Evaluate()
	{
		Move();

		if(Vector3.Distance(blackboard.GetCurrentAgent().GetPos(),
			blackboard.GetCurrentPlayer().GetPos()) <
			blackboard.GetObjectFromBlackBoard<float>(Blackboard.ENEMY_ATTACK_RANGE))
		{
			return BTNodeState.success;
		}
		else
			return BTNodeState.running;
	}

	private void Move()
	{
		blackboard.GetCurrentAgent().transform.position =
		Vector3.MoveTowards(blackboard.GetCurrentAgent().transform.localPosition,
		blackboard.GetNextMovementPos(), blackboard.GetObjectFromBlackBoard<float>(Blackboard.MOVESPEED));

		Vector3 lookPos = blackboard.GetNextMovementPos() + new Vector3(0, 1, 0);

		blackboard.GetCurrentAgent().LookatGraphics(lookPos);
	}
}
