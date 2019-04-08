using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLeaf_AttackTarget : BaseNode
{
	private Blackboard blackboard;

	public BTLeaf_AttackTarget(Blackboard bb)
	{
		blackboard = bb;

		blackboard.SetValueOnBlackboard(Blackboard.DAMAGE, Blackboard.E_DAMAGE);
	}

	public override BTNodeState Evaluate()
	{
		blackboard.GetObjectFromBlackBoard<IDamagable>(Blackboard.ENEMY_ATTACK_TARGET).DealDamage(blackboard.GetObjectFromBlackBoard<float>(Blackboard.DAMAGE));

		return BTNodeState.success;
	}
}
