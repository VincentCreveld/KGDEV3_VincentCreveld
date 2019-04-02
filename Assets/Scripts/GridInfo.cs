using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInfo : MonoBehaviour
{
	public Transform StartPos;
	public LayerMask WallMask;
	public Vector2 gridWorldSize;

	public float nodeRadius;
	public float distance;

	private Node[,] grid;
	public List<Node> FinalPath;

	private float nodeDiameter;
	private int gridSizeX, gridSizeY;



	private void Start()
	{
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
		ConstructGrid();
	}

	// Grid generation fro tutorial, not needed as i have my own maze generation
	private void ConstructGrid()
	{
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

		for(int x = 0; x < gridSizeX; x++)
		{
			for(int y = 0; y < gridSizeY; y++)
			{
				float xStep = (x * nodeDiameter + nodeRadius);
				float yStep = (y * nodeDiameter + nodeRadius);
				Vector3 worldPoint = bottomLeft + Vector3.right * xStep + Vector3.forward * yStep;

				bool isWall = true;

				if(Physics.CheckSphere(worldPoint, nodeRadius, WallMask))
					isWall = false;

				grid[x, y] = new Node(isWall, worldPoint, x, y);

			}
		}
	}

	public List<Node> GetNeighbors(Node node)
	{
		List<Node> neighbors = new List<Node>();

		int xCheck;
		int yCheck;

		// Checking all 8 neighbors.
		for(int x = -1; x <= 1; x++)
		{
			for(int y = -1; y <= 1; y++)
			{
				// Skip iteration if checking self.
				if(x == 0 && y == 0)
					continue;

				xCheck = node.gridX + x;
				yCheck = node.gridY + y;

				// Check if neighbor is not over grid boundary
				if(xCheck >= 0 && xCheck < gridSizeX && yCheck >= 0 && yCheck < gridSizeY)
					neighbors.Add(grid[xCheck,yCheck]);
			}
		}

		return neighbors;
	}

	public Node GetNodeFromWorldPos(Vector3 pos)
	{
		float xPoint = Mathf.Clamp01((pos.x + gridWorldSize.x / 2) / gridWorldSize.x);
		float yPoint = Mathf.Clamp01((pos.z + gridWorldSize.y / 2) / gridWorldSize.y);

		int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
		int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint);

		return grid[x, y];

	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

		if(grid != null)
		{
			foreach(Node node in grid)
			{
				if(node.isWall)
					Gizmos.color = Color.white;
				else
					Gizmos.color = Color.red;

				if(FinalPath != null)
					if(FinalPath.Contains(node))
						Gizmos.color = Color.blue;

				Gizmos.DrawCube(node.pos, Vector3.one * (nodeDiameter - distance));
			}

		}
	}
}
