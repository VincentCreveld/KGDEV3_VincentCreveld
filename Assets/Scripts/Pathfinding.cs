﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
	public LevelManager levelManager;

	public Transform startPos, targetPos;

	public List<Node> path;

	//private void Update()
	//{
	//	FindPath(startPos.position, targetPos.position);
	//}

	// Access function for agent.
	public void FindPath(Vector3 startPos, Vector3 targetPos)
	{
		Node startNode = levelManager.GetNodeFromWorldPos(startPos);
		Node targetNode = levelManager.GetNodeFromWorldPos(targetPos + new Vector3(0.5f,0,0.5f));

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

			foreach(Node neighbor in levelManager.GetNeighbors(curNode))
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

	public List<Node> GetPath()
	{
		return path;
	}

	public int GetClosestRoom(Vector3 pos)
	{
		int closestNo = 0;
		float closestDist = 10000000f;

		for(int i = 0; i < levelManager.roomCenters.Count; i++)
		{
			if(Vector3.Distance(pos, levelManager.roomCenters[i]) < closestDist)
			{
				closestDist = Vector3.Distance(pos, levelManager.roomCenters[i]);
				closestNo = i;
			}
		}
		return closestNo;
	}

	public Vector3 GetClosestHealthpack(Vector3 pos)
	{
		float closestDist = 10000000f;

		Vector3 closestPack = pos;

		for(int i = 0; i < levelManager.healthpacks.Count; i++)
		{
			if(Vector3.Distance(pos, levelManager.healthpacks[i].GetPos()) < closestDist)
			{
				closestDist = Vector3.Distance(pos, levelManager.healthpacks[i].GetPos());
				closestPack = levelManager.healthpacks[i].GetPos();
			}
		}
		return closestPack;
	}

	public int GetFurthestRoom(Vector3 pos)
	{
		int furthestNo = 0;
		float furthestDist = 0f;

		for(int i = 0; i < levelManager.roomCenters.Count; i++)
		{
			if(Vector3.Distance(pos, levelManager.roomCenters[i]) > furthestDist)
			{
				furthestDist = Vector3.Distance(pos, levelManager.roomCenters[i]);
				furthestNo = i;
			}
		}
		return furthestNo;
	}

	public Vector3 GetRoomCenter(int i)
	{
		if(i < levelManager.roomCenters.Count)
			return levelManager.roomCenters[i];
		else
			return levelManager.roomCenters[0];
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
		levelManager.finalPath = finalPath;

		path = finalPath;
	}

	private int GetManhattenDist(Node startNode, Node targetNode)
	{
		int deltaX = Mathf.Abs(startNode.gridX - targetNode.gridX);
		int deltaY = Mathf.Abs(startNode.gridY - targetNode.gridY);

		return deltaX + deltaY;
	}
}
