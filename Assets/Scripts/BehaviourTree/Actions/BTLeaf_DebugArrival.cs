using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Node made for debugging, always returns success.
public class BTLeaf_DebugArrival : BaseNode
{
	string debugLine = "";

	public BTLeaf_DebugArrival(string str)
	{
		debugLine = str;
	}

	public override BTNodeState Evaluate()
	{
		Debug.Log(debugLine);
		return BTNodeState.success;
	}
}
