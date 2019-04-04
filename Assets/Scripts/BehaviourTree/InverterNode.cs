using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Decorator node
public class InverterNode : BaseNode
{
	private BaseNode nodeToInvert;

	public InverterNode(BaseNode node)
	{
		nodeToInvert = node;
	}

	public override BTNodeState Evaluate()
	{
		switch(nodeToInvert.Evaluate())
		{
			case BTNodeState.success:
				nodeState = BTNodeState.failure;
				return nodeState;
			case BTNodeState.running:
				nodeState = BTNodeState.running;
				return nodeState;
			case BTNodeState.failure:
				nodeState = BTNodeState.success;
				return nodeState;
		}
		nodeState = BTNodeState.success;
		return nodeState;
	}
}
