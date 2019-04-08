using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLeaf_UseHealthpack : BaseNode
{
	private Blackboard blackboard;

	public BTLeaf_UseHealthpack(Blackboard bb, float range)
	{
		blackboard = bb;
	}

	public override BTNodeState Evaluate()
	{
		if(blackboard.GetCurrentPlayer() == null)
			return BTNodeState.failure;

		Collider[] colliders = Physics.OverlapSphere(
			blackboard.GetCurrentAgent().GetPos(),
			blackboard.GetObjectFromBlackBoard<float>(Blackboard.ENEMY_ATTACK_RANGE));

		List<Healthpack> validTargets = new List<Healthpack>();

		foreach(Collider col in colliders)
		{
			if(col.GetComponentInParent<Healthpack>() != null)
			{
				validTargets.Add(col.GetComponentInParent<Healthpack>());
			}
		}

		if(validTargets.Count <= 0)
			return BTNodeState.failure;

		validTargets[0].HealTarget(blackboard.GetCurrentAgent().GetComponent<IHealable>());

		return BTNodeState.success;
	}
}

