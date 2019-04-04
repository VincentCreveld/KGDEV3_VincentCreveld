using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafNode : BaseNode
{
	public delegate BTNodeState LeafNodeDelegate();

	private LeafNodeDelegate leafAction;

	public LeafNode(LeafNodeDelegate action)
	{
		leafAction = action;
	}

	public override BTNodeState Evaluate()
	{
		switch(leafAction())
		{
			case BTNodeState.success:
				nodeState = BTNodeState.success;
				return nodeState;
			case BTNodeState.running:
				nodeState = BTNodeState.running;
				return nodeState;
			case BTNodeState.failure:
				nodeState = BTNodeState.failure;
				return nodeState;
			default:
				nodeState = BTNodeState.failure;
				return nodeState;
		}
	}
}
