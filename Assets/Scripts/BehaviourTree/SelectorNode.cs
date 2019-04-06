using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : BaseNode
{
	private List<BaseNode> connectedNodes = new List<BaseNode>();

	public SelectorNode(params BaseNode[] nodes)
	{
		foreach(BaseNode node in nodes)
		{
			connectedNodes.Add(node);
		}
	}

	public override BTNodeState Evaluate()
	{
		foreach(BaseNode node in connectedNodes)
		{
			switch(node.Evaluate())
			{
				case BTNodeState.success:
					nodeState = BTNodeState.success;
					return nodeState;
				case BTNodeState.running:
					nodeState = BTNodeState.running;
					return nodeState;
				case BTNodeState.failure:
					continue;
				default:
					continue;
			}
		}
		nodeState = BTNodeState.failure;
		return nodeState;
	}
}
