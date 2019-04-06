using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLeaf_MoveEnemyObjectAlongPath : BaseNode
{
	private Blackboard blackboard;

	public BTLeaf_MoveEnemyObjectAlongPath(Blackboard bb)
	{
		blackboard = bb;

		bb.SetValueOnBlackboard(Blackboard.ENEMY_ATTACK_RANGE, Blackboard.E_ATTACK_RANGE);
		bb.SetValueOnBlackboard(Blackboard.MOVESPEED, Blackboard.E_MOVESPEED);
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
		blackboard.GetNextMovementPos(), 0.2f);
	}
}
