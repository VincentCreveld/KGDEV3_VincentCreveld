using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysSuccessNode : BaseNode
{
	private BaseNode connectedNode;

	public AlwaysSuccessNode(BaseNode node)
	{
		connectedNode = node;
	}

	public override BTNodeState Evaluate()
	{
		connectedNode.Evaluate();

		nodeState = BTNodeState.success;
		return nodeState;
	}
}
