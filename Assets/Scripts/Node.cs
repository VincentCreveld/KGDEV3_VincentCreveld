﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
// Contains info for a* pathfinding
public class Node
{
	public int gridX, gridY;
	public int distToStart; // G cost
	public int distToGoal;   // H cost
	public int TotalCost { get { return distToStart + distToGoal; } } // F cost

	public bool isWall;
	public Vector3 pos;
	public Node parentNode;

	// Default constructor
	public Node(bool _isWall, Vector3 _pos, int _gridX, int _gridY)
	{
		isWall = _isWall;
		pos = _pos;
		gridX = _gridX;
		gridY = _gridY;
	}
}
