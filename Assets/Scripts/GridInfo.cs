using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInfo : MonoBehaviour
{
	public Transform StartPos;
	public LayerMask Wallmask;
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

	private void ConstructGrid()
	{
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

		for(int y = 0; y < gridSizeY; y++)
		{
			for(int x = 0; x < gridSizeX; x++)
			{
				float xStep = (x * nodeDiameter + nodeRadius);
				float yStep = (y * nodeDiameter + nodeRadius);
				Vector3 worldPoint = bottomLeft + Vector3.right * xStep + Vector3.forward * yStep;

				bool isWall = true;


			}
		}
	}
}
