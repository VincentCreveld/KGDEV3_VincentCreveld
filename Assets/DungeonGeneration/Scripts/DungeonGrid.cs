using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Tile
{
	empty = 0,
	room = 1,
	margin = 2,
	path = 3
}

public enum TileRotation
{
	right = 90,
	up = 0,
	left = 270,
	down = 180
}

public class DungeonGrid : MonoBehaviour
{
	[Range(3, 20)]
	public int rooms;

	[Range(1, 10)]
	public int roomMargin;

	public IntVector2 roomXRange, roomYRange;

	[SerializeField]
	private List<Room> roomList = new List<Room>();

	public GameObject room, path, wall, door, player, doubleDoor;

	public const int DUNGEON_SIZE = 64;
	private Tile[,] dungeonGrid;
	private GameObject[,] doors;

	private Vector3 bottomLeft;

	public GameObject dungeonParent;

	private void Awake()
	{
		GenerateDungeon();
	}

	// Exposed values for pathfinding purposes
	public Tile[,] GetDungeonGrid()
	{
		return dungeonGrid;
	}

	public IntVector2 GetDungeonSize()
	{
		return new IntVector2(DUNGEON_SIZE, DUNGEON_SIZE);
	}

	public Room startRoom { get { return roomList[0]; } }
	public Room endRoom { get { return roomList[roomList.Count-1]; } }

	//This function is the main function that generates the dungeon.
	public void GenerateDungeon()
	{
		dungeonGrid = new Tile[DUNGEON_SIZE, DUNGEON_SIZE];
		doors = new GameObject[DUNGEON_SIZE, DUNGEON_SIZE];
		PlaceRooms();
		PlacePaths();
		CheckRoomsConnected();
		CleanUpRoomBorders();
		GenerateGrid();
		GenerateWalls();
		//PlacePlayer();

		bottomLeft = (transform.position - Vector3.right * (DUNGEON_SIZE)/2 - Vector3.forward * (DUNGEON_SIZE)/2);

		// Parent all objects to new object for easy offset
		dungeonParent = new GameObject();
		foreach(Transform child in transform.GetComponentsInChildren<Transform>())
		{
			if(child == transform)
				continue;

			child.parent = dungeonParent.transform;
		}

		//dungeonParent.transform.localScale *= 2f;

		dungeonParent.transform.position = bottomLeft;
	}

	private void PlaceRooms()
	{
		roomList.Clear();
		int attempts = 0;
		while(attempts < 50 && roomList.Count < rooms)
		{
			Room r = GenerateRoom();
			if(CheckSpace(r))
			{
				roomList.Add(r);
				SetupGrid(r);
			}
			attempts++;
		}
	}

	private void SetupGrid(Room r)
	{
		for(int i = r.xPos - roomMargin; i < r.xPos + r.xSize + roomMargin; i++)
		{
			for(int j = r.yPos - roomMargin; j < r.yPos + r.ySize + roomMargin; j++)
			{
				if(i >= 0 && j >= 0 && i <= DUNGEON_SIZE - 1 && j <= DUNGEON_SIZE - 1)
					dungeonGrid[i, j] = Tile.margin;
			}
		}
		for(int i = r.xPos; i < r.xPos + r.xSize; i++)
		{
			for(int j = r.yPos; j < r.yPos + r.ySize; j++)
			{
				dungeonGrid[i, j] = Tile.room;
			}
		}
	}

	private Room GenerateRoom()
	{
		IntVector2 v2 = getXY();
		int x = v2.min;
		int y = v2.max;
		return new Room(UnityEngine.Random.Range(roomXRange.min, roomXRange.max), UnityEngine.Random.Range(roomYRange.min, roomYRange.max), x, y);
	}

	private IntVector2 getXY()
	{
		return new IntVector2(UnityEngine.Random.Range(0, DUNGEON_SIZE - 1 - (roomXRange.max * 2)), UnityEngine.Random.Range(0, DUNGEON_SIZE - 1 - (roomYRange.max * 2)));
	}

	private bool CheckSpace(Room room)
	{
		for(int i = room.xPos; i < room.xSize + room.xPos; i++)
		{
			for(int j = room.yPos; j < room.ySize + room.yPos; j++)
			{
				if(dungeonGrid[i, j] == Tile.room || dungeonGrid[i, j] == Tile.margin)
					return false;
			}
		}
		return true;
	}

	[ContextMenu("Clear dungeon")]
	private void ClearGrid()
	{
		Destroy(dungeonParent);
		doors = new GameObject[DUNGEON_SIZE, DUNGEON_SIZE];
	}

	private void GenerateGrid()
	{
		ClearGrid();
		for(int i = 0; i < DUNGEON_SIZE; i++)
		{
			for(int j = 0; j < DUNGEON_SIZE; j++)
			{
				if(dungeonGrid[i, j] == Tile.room)
				{
					Instantiate(room, new Vector3(i, 0, j), Quaternion.Euler(new Vector3(90f, 0.0f, 0.0f)), transform);
				}
				if(dungeonGrid[i, j] == Tile.path)
				{
					Instantiate(path, new Vector3(i, 0, j), Quaternion.Euler(new Vector3(90f, 0.0f, 0.0f)), transform);
				}
			}
		}
	}

	private void PlacePaths()
	{
		for(int i = 0; i < roomList.Count - 1; i++)
		{
			Room r1 = roomList[i];
			Room r2 = roomList[i + 1];
			GeneratePath(r1, r2);
		}
	}

	private bool IsPointInRoom(Room r, int x, int y)
	{
		foreach(Room room in roomList)
		{
			if((x >= room.xPos && x <= room.xPos + room.xSize) && (y >= room.yPos && y <= room.yPos + room.ySize) && room != r)
				return true;
		}
		return false;
	}

	private void CleanUpRoomBorders()
	{
		foreach(Room r in roomList)
		{
			for(int i = r.xPos; i < r.xPos + r.xSize; i++)
			{
				for(int j = r.yPos; j < r.yPos + r.ySize; j++)
				{
					CheckNeighbors(i, j);
				}
			}
		}
	}

	private void CheckRoomsConnected()
	{
		for(int g = 0; g < roomList.Count; g++)
		{
			Room r = roomList[g];
			if(!CheckForConnectedPaths(r))
			{
				int num = g + 1;
				if(num < roomList.Count)
				{
					GeneratePath(r, roomList[num]);//, r);
				}
				else
				{
					GeneratePath(r, roomList[0]);//, r);
				}
			}
		}
	}

	private bool CheckForConnectedPaths(Room r)
	{
		for(int x = r.xPos; x < r.xPos + r.xSize; x++)
		{
			for(int y = r.yPos; y < r.yPos + r.ySize; y++)
			{
				if(x - 1 > 0)
				{
					if(dungeonGrid[x - 1, y] == Tile.path)
					{
						return true;
					}
				}
				if(x + 1 < DUNGEON_SIZE)
				{
					if(dungeonGrid[x + 1, y] == Tile.path)
					{
						return true;
					}
				}
				if(y - 1 > 0)
				{
					if(dungeonGrid[x, y - 1] == Tile.path)
					{
						return true;
					}
				}
				if(y + 1 < DUNGEON_SIZE)
				{
					if(dungeonGrid[x, y + 1] == Tile.path)
					{
						return true;
					}
				}
			}
		}

		return false;
	}

	private bool CheckNeighbors(int x, int y)
	{

		if(x - 1 >= 0)
		{
			if(dungeonGrid[x - 1, y] == Tile.path)
			{
				dungeonGrid[x - 1, y] = Tile.room;
			}
		}

		if(x + 1 <= DUNGEON_SIZE)
		{
			if(dungeonGrid[x + 1, y] == Tile.path)
			{
				dungeonGrid[x + 1, y] = Tile.room;
			}
		}

		if(y - 1 >= 0)
		{
			if(dungeonGrid[x, y - 1] == Tile.path)
			{
				dungeonGrid[x, y - 1] = Tile.room;
			}
		}

		if(y + 1 <= DUNGEON_SIZE)
		{
			if(dungeonGrid[x, y + 1] == Tile.path)
			{
				dungeonGrid[x, y + 1] = Tile.room;
			}
		}

		return false;
	}

	private bool GeneratePath(Room r1, Room r2)
	{
		Room temp;
		int xR1 = (r1.xPos + Mathf.RoundToInt(r1.xSize / 2));
		int xR2 = (r2.xPos + Mathf.RoundToInt(r2.xSize / 2));
		int yR1 = (r1.yPos + Mathf.RoundToInt(r1.xSize / 2));
		int yR2 = (r2.yPos + Mathf.RoundToInt(r2.ySize / 2));

		//welke X/Y ligt lager?
		Room leftMostRoom = xR1 > xR2 ? r2 : r1;
		Room rightMostRoom = xR1 > xR2 ? r1 : r2;

		Room topMostRoom = yR1 > yR2 ? r2 : r1;
		Room bottomMostRoom = yR1 > yR2 ? r1 : r2;

		int lx, rx, ly, ry, tx, bx, ty, by;
		lx = leftMostRoom.xPos + Mathf.RoundToInt(leftMostRoom.xSize / 2);
		rx = rightMostRoom.xPos + Mathf.RoundToInt(rightMostRoom.xSize / 2);
		ly = leftMostRoom.yPos + Mathf.RoundToInt(leftMostRoom.ySize / 2);
		ry = rightMostRoom.yPos + Mathf.RoundToInt(rightMostRoom.ySize / 2);

		tx = topMostRoom.xPos + Mathf.RoundToInt(topMostRoom.xSize / 2);
		bx = bottomMostRoom.xPos + Mathf.RoundToInt(bottomMostRoom.xSize / 2);
		ty = topMostRoom.yPos + Mathf.RoundToInt(topMostRoom.ySize / 2);
		by = bottomMostRoom.yPos + Mathf.RoundToInt(bottomMostRoom.ySize / 2);


		for(int i = lx; i < rx + 1; i++)
		{
			if(dungeonGrid[i, ly] != Tile.room)
			{
				dungeonGrid[i, ly] = Tile.path;
			}
			else
			{
				dungeonGrid[i, ly] = Tile.path;
				if(IsPointInRoom(leftMostRoom, i, ly))
					temp = GetRoomByCoords(i, ly);
			}
		}

		for(int j = ty; j < by + 1; j++)
		{
			if(dungeonGrid[rx, j] != Tile.room)
			{
				dungeonGrid[rx, j] = Tile.path;
			}
			else
			{
				dungeonGrid[rx, j] = Tile.path;
				if(IsPointInRoom(bottomMostRoom, rx, j))
					temp = GetRoomByCoords(rx, j);
			}
		}

		temp = r2;
		return temp == r2 ? true : GeneratePath(temp, r2);
	}

	private Room GetRoomByCoords(int x, int y)
	{
		foreach(Room r in roomList)
		{
			if(IsPointInRoom(r, x, y))
			{
				return r;
			}
		}
		return null;
	}

	private void GenerateWalls()
	{
		for(int i = 0; i < DUNGEON_SIZE; i++)
		{
			for(int j = 0; j < DUNGEON_SIZE; j++)
			{
				if(dungeonGrid[i, j] == Tile.room || dungeonGrid[i, j] == Tile.path)
					GenerateWall(i, j);
				//if(dungeonGrid[i, j] == Tile.path)
				//	GenerateDoor(i, j);
			}
		}
	}

	private void GenerateWall(int x, int y)
	{
		if(x - 1 >= 0)
		{
			if(dungeonGrid[x - 1, y] == Tile.margin || dungeonGrid[x - 1, y] == Tile.empty)
			{
				PlaceWall(TileRotation.left, x, y);
			}
		}
		else
		{
			PlaceWall(TileRotation.left, x, y);
		}
		if(x + 1 < DUNGEON_SIZE)
		{
			if(dungeonGrid[x + 1, y] == Tile.margin || dungeonGrid[x + 1, y] == Tile.empty)
			{
				PlaceWall(TileRotation.right, x, y);
			}
		}
		else
		{
			PlaceWall(TileRotation.right, x, y);
		}
		if(y - 1 >= 0)
		{
			if(dungeonGrid[x, y - 1] == Tile.margin || dungeonGrid[x, y - 1] == Tile.empty)
			{
				PlaceWall(TileRotation.down, x, y);
			}
		}
		else
		{
			PlaceWall(TileRotation.down, x, y);
		}
		if(y + 1 < DUNGEON_SIZE)
		{
			if(dungeonGrid[x, y + 1] == Tile.margin || dungeonGrid[x, y + 1] == Tile.empty)
			{
				PlaceWall(TileRotation.up, x, y);
			}
		}
		else
		{
			PlaceWall(TileRotation.up, x, y);
		}
	}

	private void GenerateDoor(int x, int y)
	{

		for(int i = x - 1; i < x + 1; i++)
		{
			for(int j = y - 1; j < y + 1; j++)
			{
				if(x == i || y == j)
					continue;

			}
		}

		if(x - 1 >= 0)
		{
			if(dungeonGrid[x - 1, y] == Tile.room)
			{
				PlaceDoor(TileRotation.left, x, y);
			}
		}

		if(x + 1 < DUNGEON_SIZE)
		{
			if(dungeonGrid[x + 1, y] == Tile.room)
			{
				PlaceDoor(TileRotation.right, x, y);
			}
		}

		if(y - 1 >= 0)
		{
			if(dungeonGrid[x, y - 1] == Tile.room)
			{
				PlaceDoor(TileRotation.down, x, y);
			}
		}

		if(y + 1 < DUNGEON_SIZE)
		{
			if(dungeonGrid[x, y + 1] == Tile.room)
			{
				PlaceDoor(TileRotation.up, x, y);
			}
		}

	}

	private void PlaceDoor(TileRotation t, int x, int y)
	{
		doors[x, y] = Instantiate(door, new Vector3(x, 0, y), Quaternion.Euler(new Vector3(0, (int)t, 0)), transform);
	}

	private void PlaceDoubleDoor(TileRotation t, int x, int y)
	{
		doors[x, y] = Instantiate(doubleDoor, new Vector3(x, 0, y), Quaternion.Euler(new Vector3(0, (int)t, 0)), transform);
	}

	private void PlaceWall(TileRotation t, int x, int y)
	{
		Instantiate(wall, new Vector3(x, 0, y), Quaternion.Euler(new Vector3(0, (int)t, 0)), transform);
	}

	// Places the player in the starting room.
	private void PlacePlayer()
	{
		Room r1 = roomList[0];
		int x = r1.xPos + Mathf.RoundToInt(r1.xSize / 2);
		int y = r1.yPos + Mathf.RoundToInt(r1.ySize / 2);

		player.transform.position = (new Vector3(x, 5, y));
	}

}

[Serializable]
public struct IntVector2
{
	public int min, max;
	public IntVector2(int x, int y)
	{
		min = x;
		max = y;
	}
}
