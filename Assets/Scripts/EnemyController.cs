using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pathfinding))]
public class EnemyController : MonoBehaviour
{
	private Pathfinding pathfindingComp;

	public PlayerController playerObject;

	private Blackboard blackboard;

	private SelectorNode behaviourTree;

	private void Awake()
	{
		if(playerObject == null)
		{
			Debug.Log("No referenced player on enemy. Stopping initialisation");
			return;
		}
		pathfindingComp = GetComponent<Pathfinding>();

		blackboard = new Blackboard(pathfindingComp, playerObject, this);

		SetupBT();
	}

	private void Update()
	{
		blackboard.SetValueOnBlackboard(Blackboard.PLAYER_CURHEALTH, playerObject.CurrentHealth);

		behaviourTree.Evaluate();
	}

	private void SetupBT()
	{
		behaviourTree = new SelectorNode(CreateHuntBehaviour());
	}

	private SequenceNode CreateHuntBehaviour()
	{
		return new SequenceNode(
			new BTLeaf_CheckForPlayerLowHP(blackboard),
			new BTLeaf_SetPathfindingTarget(blackboard),
			new BTLeaf_MoveEnemyObjectAlongPath(blackboard),
			new BTLeaf_DebugArrival()
			);
	}

	public Vector3 GetPos()
	{
		return transform.position;
	}
}
