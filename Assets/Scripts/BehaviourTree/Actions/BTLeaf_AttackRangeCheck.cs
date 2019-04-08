using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLeaf_AttackRangeCheck : BaseNode
{
	private Blackboard blackboard;

	public BTLeaf_AttackRangeCheck(Blackboard bb, float range)
	{
		blackboard = bb;

		blackboard.SetValueOnBlackboard(Blackboard.ENEMY_ATTACK_RANGE, range);
		blackboard.SetValueOnBlackboard(Blackboard.ENEMY_ATTACK_TARGET, blackboard.GetCurrentPlayer().GetComponent<IDamagable>());
	}

	public override BTNodeState Evaluate()
	{
		if(blackboard.GetCurrentPlayer() == null)
			return BTNodeState.failure;

		Collider[] colliders = Physics.OverlapSphere(
			blackboard.GetCurrentAgent().GetPos(),
			blackboard.GetObjectFromBlackBoard<float>(Blackboard.ENEMY_ATTACK_RANGE));

		List<IDamagable> validTargets = new List<IDamagable>();
		List<Vector3> targets = new List<Vector3>();
		foreach(Collider col in colliders)
		{
			if(col.GetComponent<IDamagable>() != null)
			{
				if(col.GetComponent<IDamagable>() == blackboard.GetCurrentAgent().GetComponent<IDamagable>())
					continue;
				validTargets.Add(col.GetComponent<IDamagable>());
				targets.Add(col.transform.position);
			}
		}

		if(validTargets.Count <= 0)
			return BTNodeState.failure;

		blackboard.SetValueOnBlackboard(Blackboard.CURRENT_PATHFINDING_TARGET, targets[0]);
		blackboard.SetValueOnBlackboard(Blackboard.ENEMY_ATTACK_TARGET, validTargets[0]);

		return BTNodeState.success;
	}
}
