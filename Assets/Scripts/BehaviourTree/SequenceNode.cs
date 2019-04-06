using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : BaseNode
{
	private List<BaseNode> connectedNodes = new List<BaseNode>();

	public SequenceNode(params BaseNode[] nodes)
	{
		foreach(BaseNode node in nodes)
		{
			connectedNodes.Add(node);
		}
	}

	public override BTNodeState Evaluate()
	{
		bool isAnyChildRunning = false;

		foreach(BaseNode node in connectedNodes)
		{
			switch(node.Evaluate())
			{
				case BTNodeState.success:
					continue;
				case BTNodeState.running:
					isAnyChildRunning = true;
					continue;
				case BTNodeState.failure:
					nodeState = BTNodeState.failure;
					return nodeState;
				default:
					nodeState = BTNodeState.success;
					return nodeState;
			}
		}

		nodeState = isAnyChildRunning ? BTNodeState.running : BTNodeState.success;

		return nodeState;
	}
}
