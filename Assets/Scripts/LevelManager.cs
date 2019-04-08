using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This class has all the info the A* needs to function.
// Contains the dungeon grid info
public class LevelManager : MonoBehaviour
{
	public Transform startPos;
	public LayerMask wallMask;
	public Vector2 gridWorldSize;

	public float nodeRadius;
	public float distance;

	private Node[,] grid;
	public List<Node> finalPath;
	public List<Vector3> roomCenters;

	private float nodeDiameter;
	private int gridSizeX, gridSizeY;

	public DungeonGrid dungeonGrid;

	public List<EnemyController> enemies;

	public List<Healthpack> healthpacks;

	public GameObject endGameText;
	private bool gameEnded = false;

	private void Start()
	{
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(dungeonGrid.GetDungeonSize().min);
		gridSizeY = Mathf.RoundToInt(dungeonGrid.GetDungeonSize().max);
		ConstructGrid();
		PlaceStart();
		PlaceHealthpacksInRooms();
		PlaceEnemiesInRooms();
	}

	private void Update()
	{
		if(!gameEnded)
			CheckEnd();	
	}

	private void CheckEnd()
	{
		foreach(EnemyController e in enemies)
		{
			if(e != null)
				return;
		}
		StartCoroutine(EndSequence());
	}

	private IEnumerator EndSequence()
	{
		gameEnded = true;
		endGameText.SetActive(true);
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void GetRooms()
	{
		List<Room> rooms = dungeonGrid.GetRooms();
		roomCenters = new List<Vector3>();

		foreach(Room r in rooms)
		{
			int x = r.xPos + Mathf.RoundToInt(r.xSize / 2);
			int y = r.yPos + Mathf.RoundToInt(r.ySize / 2);
			Vector3 center = new Vector3(x - 31.5f, 0, y - 31.5f);

			roomCenters.Add(center);
		}
	}


	public void ConstructGrid()
	{
		if(dungeonGrid == null)
			return;

		dungeonGrid.GenerateDungeon();

		Tile[,] dGrid = dungeonGrid.GetDungeonGrid();
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

		for(int x = 0; x < gridSizeX; x++)
		{
			for(int y = 0; y < gridSizeY; y++)
			{
				float xStep = (x * nodeDiameter);
				float yStep = (y * nodeDiameter);
				Vector3 worldPoint = (bottomLeft + Vector3.right * xStep + Vector3.forward * yStep);

				bool isWalkable = false;

				if(dGrid[x, y] == Tile.path || dGrid[x, y] == Tile.room)
					isWalkable = true;

				grid[x, y] = new Node(isWalkable, worldPoint, x, y);

			}
		}

		GetRooms();
	}

	public List<Node> GetNeighbors(Node node)
	{
		List<Node> neighbors = new List<Node>();

		int xCheck;
		int yCheck;

		// Checking all 8 neighbors.
		// This allows diagonal movement.
		for(int x = -1; x <= 1; x++)
		{
			for(int y = -1; y <= 1; y++)
			{
				// The nested dloop normally allows diagonal movement
				// This only allows checking of direct neighbors
				if(x != 0 && y != 0)
					continue;

				// Skip iteration if checking self.
				if(x == 0 && y == 0)
					continue;

				xCheck = node.gridX + x;
				yCheck = node.gridY + y;

				// Check if neighbor is not over grid boundary
				if(xCheck >= 0 && xCheck < gridSizeX && yCheck >= 0 && yCheck < gridSizeY)
					neighbors.Add(grid[xCheck, yCheck]);
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

	// In editor visualisation of the dungoen and path. Not visible at runtime
	private void OnDrawGizmos()
	{
		if(dungeonGrid.dungeonParent == null)
			return;

		Gizmos.DrawWireCube(new Vector3(((gridWorldSize.x - 1) / 2), 0, ((gridWorldSize.x - 1) / 2)) + dungeonGrid.dungeonParent.transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

		if(grid != null)
		{
			foreach(Node node in grid)
			{
				float size = 0.4f;
				if(node.isWall)
					Gizmos.color = Color.white;
				else
					Gizmos.color = Color.red;

				if(finalPath != null)
					if(finalPath.Contains(node))
					{
						Gizmos.color = Color.blue;
						size = 0.8f;
					}

				//Vector3 pos = new Vector3(-(gridWorldSize.x - 2), 0, -(gridWorldSize.y - 2)) + 
				Vector3 pos = (node.pos);
				Gizmos.DrawCube(((node.pos * 2) * 0.5f), Vector3.one * (nodeDiameter - distance) * size);
			}

		}
	}

	public void PlaceStart()
	{
		Room r1 = dungeonGrid.startRoom;
		int x = r1.xPos + Mathf.RoundToInt(r1.xSize / 2);
		int y = r1.yPos + Mathf.RoundToInt(r1.ySize / 2);

		startPos.transform.position = (new Vector3((-(gridWorldSize.x-1)/2) + x, 4, (-(gridWorldSize.x - 1) / 2) + y));
	}

	public void PlaceEnemiesInRooms()
	{
		foreach(EnemyController enemy in enemies)
		{
			// Starting at 1 to prevent spawning in same room as player
			int num = UnityEngine.Random.Range(1, roomCenters.Count);
			enemy.transform.position = roomCenters[num];
			enemy.Initialise(healthpacks);
		}
	}

	public void PlaceHealthpacksInRooms()
	{
		foreach(Healthpack h in healthpacks)
		{
			h.transform.position = roomCenters[UnityEngine.Random.Range(0, roomCenters.Count)];
		}
	}
}
