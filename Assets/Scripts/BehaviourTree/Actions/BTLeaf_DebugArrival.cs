using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Node made for debugging, always returns success.
public class BTLeaf_DebugArrival : BaseNode
{
	public override BTNodeState Evaluate()
	{
		Debug.Log("Arrived at this node!");
		return BTNodeState.success;
	}
}
