using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pathfinding))]
public class MoveAgent : MonoBehaviour
{
	public Transform targetPos;

	private Pathfinding pathfindingComponent;

	public float moveSpeed = 3f;

	private void Awake()
	{
		pathfindingComponent = GetComponent<Pathfinding>();
	}

	public void MoveToPosition(Vector3 pos)
	{
		StopAllCoroutines();
		StartCoroutine(EnemyMovement(pos));
	}

	[ContextMenu("Move to target")]
	public void TestFunc()
	{
		MoveToPosition(targetPos.position);
	}

	protected IEnumerator EnemyMovement(Vector3 pos)
	{
		pathfindingComponent.FindPath(transform.position, pos);

		List<Node> path = pathfindingComponent.path;

		Vector3 previousNode;
		Vector3 nextNode;

		for(int i = 1; i < path.Count; i++)
		{
			pathfindingComponent.FindPath(transform.position, targetPos.position);
			path = pathfindingComponent.path;

			previousNode = (path[i - 1].pos - new Vector3(0.5f,0,0.5f)) * 0.5f;
			nextNode = (path[i].pos - new Vector3(0.5f, 0, 0.5f)) * 0.5f;
			yield return StartCoroutine(MoveToNextNode(previousNode, nextNode));
		}

		Debug.LogError("Desination reached");
	}

	private IEnumerator MoveToNextNode(Vector3 startPos, Vector3 endPos)
	{
		Debug.Log("Starting");

		// This block moves the agent one node, then rechecks the path
		float curTime = 0f;
		float distToNextNode = Vector3.Distance(startPos, endPos);
		while(true)
		{
			yield return null;
			curTime += Time.deltaTime * moveSpeed;

			//// Makes agent look at movement direction
			//Vector3 targetDir = startPos - endPos;
			//float step = 10 * Time.deltaTime;
			//Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
			//transform.rotation = Quaternion.LookRotation(newDir);

			transform.position = Vector3.MoveTowards(startPos, endPos, curTime / distToNextNode);

			if(curTime >= distToNextNode)
				break;
		}

		Debug.Log("Section done");
	}
}
