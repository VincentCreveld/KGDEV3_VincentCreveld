using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BTNodeState { success, running, failure }

public abstract class BaseNode
{
	// Returns the state of the node
	public delegate BTNodeState NodeReturn();

	protected BTNodeState nodeState;
	public BTNodeState NodeState { get { return nodeState; } }

	// ctor
	public BaseNode() { }

	// init -> geef blackboard mee

	// Overwritten function that tests the node conditions (update / tick)
	public abstract BTNodeState Evaluate();
}
