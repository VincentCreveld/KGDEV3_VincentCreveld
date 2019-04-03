﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
	public GridInfo grid;

	public Transform startPos, targetPos;

	public List<Node> path;

	private void Update()
	{
		FindPath(startPos.position, targetPos.position);
	}

	// Access function for agent.
	public void FindPath(Vector3 startPos, Vector3 targetPos)
	{
		Node startNode = grid.GetNodeFromWorldPos(startPos);
		Node targetNode = grid.GetNodeFromWorldPos(targetPos);

		List<Node> openNodes = new List<Node>();
		HashSet<Node> closedList = new HashSet<Node>();

		openNodes.Add(startNode);

		while(openNodes.Count > 0)
		{
			Node curNode = openNodes[0];

			for(int i = 1; i < openNodes.Count; i++)
			{
				if(openNodes[i].TotalCost < curNode.TotalCost || openNodes[i].TotalCost == curNode.TotalCost && openNodes[i].distToGoal < curNode.distToGoal)
				{
					curNode = openNodes[i];
				}
			}
			openNodes.Remove(curNode);
			closedList.Add(curNode);

			// Check if done
			if(curNode == targetNode)
			{
				GetFinalPath(startNode, targetNode);
				break;
			}

			foreach(Node neighbor in grid.GetNeighbors(curNode))
			{
				// If node is wall or already checked, skip iteration
				if(!neighbor.isWall || closedList.Contains(neighbor))
					continue;

				int moveCost = curNode.distToStart + GetManhattenDist(curNode, neighbor);

				if(moveCost < neighbor.distToStart || !openNodes.Contains(neighbor))
				{
					neighbor.distToStart = moveCost;
					neighbor.distToGoal = GetManhattenDist(neighbor, targetNode);
					neighbor.parentNode = curNode;

					if(!openNodes.Contains(neighbor))
						openNodes.Add(neighbor);
				}
			}
		}
	}

	private void GetFinalPath(Node startNode, Node targetNode)
	{
		List<Node> finalPath = new List<Node>();
		Node curNode = targetNode;

		while(curNode != startNode)
		{
			finalPath.Add(curNode);
			curNode = curNode.parentNode;
		}

		finalPath.Reverse();

		// For gizmo purposes
		grid.finalPath = finalPath;

		path = finalPath;
	}

	private int GetManhattenDist(Node startNode, Node targetNode)
	{
		int deltaX = Mathf.Abs(startNode.gridX - targetNode.gridX);
		int deltaY = Mathf.Abs(startNode.gridY - targetNode.gridY);

		return deltaX + deltaY;
	}
}
