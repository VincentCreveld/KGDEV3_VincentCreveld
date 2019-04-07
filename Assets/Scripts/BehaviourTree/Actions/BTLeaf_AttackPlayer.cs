using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLeaf_AttackPlayer : BaseNode
{
	private Blackboard blackboard;

	public BTLeaf_AttackPlayer(Blackboard bb)
	{
		blackboard = bb;

		blackboard.SetValueOnBlackboard(Blackboard.DAMAGE, Blackboard.E_DAMAGE);
	}

	public override BTNodeState Evaluate()
	{
		blackboard.GetCurrentPlayer().DealDamage(blackboard.GetObjectFromBlackBoard<float>(Blackboard.DAMAGE));

		return BTNodeState.success;
	}
}
