using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard
{
	// static strings in blackboard class to prevent typo's in strings down the line
	public static string PLAYER_OBJ = "p_obj";
	public static string VISION_RANGE = "v_range";

	public static string PLAYER_CURHEALTH = "p_health";
	public static float PLAYER_MAX_HEALTH = 100;
	public static float PLAYER_LOW_HEALTH_THRESHOLD = 35;

	public static string ENEMY_CURHEALTH = "e_health";
	public static float ENEMY_MAX_HEALTH = 50f;
	public static float ENEMY_LOW_HEALTH_THRESHOLD = 15f;

	public static string PATHFINDING_COMP = "e_pathfindingComp";

	public static string ENEMY_ROOM_TO_SENTRY = "e_sentryRoom";

	public static string ENEMY_ATTACK_RANGE = "e_attackRange";
	public static float E_ATTACK_RANGE = 1.5f;
	public static float E_FLEE_RANGE = 20f;
	public static string MOVESPEED = "e_speed";
	public static float E_MOVESPEED = .1f;
	public static string DAMAGE = "e_dmg";
	public static float E_DAMAGE = 25f;

	public static string HEALTHPACK_LOCATIONS = "hp_locations";

	public static string OWN_ENEMY_OBJECT = "e_object";

	public static string CURRENT_PATHFINDING_TARGET = "e_curTarget";

	public static string ENEMY_ATTACK_TARGET = "e_attackTarget";

	public static string ENEMY_PATROL_NODES = "e_patrolNodes";
	public static string CURRENT_PATROL_TARGET = "e_patrolTarget";

	private Dictionary<string, object> blackboard;

	public BaseNode currentEvaluatedNode;
	public List<Node> path = new List<Node>();

    public Blackboard(Pathfinding pathfindingComp, PlayerController player, EnemyController enemy)
	{
		blackboard = new Dictionary<string, object>();
		blackboard.Add(PLAYER_OBJ, player);
		blackboard.Add(PATHFINDING_COMP, pathfindingComp);
		blackboard.Add(OWN_ENEMY_OBJECT, enemy);
	}

	public void SetValueOnBlackboard(string key, object value)
	{
		if(!blackboard.ContainsKey(key))
		{
			blackboard.Add(key, value);
		}
		else
		{
			blackboard[key] = value;
		}
	}

	public bool DoesBlackboardContain(string key)
	{
		return blackboard.ContainsKey(key);
	}

	public T GetObjectFromBlackBoard<T>(string key)
	{
		if(blackboard.ContainsKey(key))
			return (T)blackboard[key];
		else
			return default(T);
	}

	public EnemyController GetCurrentAgent()
	{
		return GetObjectFromBlackBoard<EnemyController>(OWN_ENEMY_OBJECT);
	}

	public PlayerController GetCurrentPlayer()
	{
		return GetObjectFromBlackBoard<PlayerController>(PLAYER_OBJ);
	}

	public Vector3 GetCurrentAgentPosOnGrid()
	{
		return path[0].pos;
	}

	public Vector3 GetNextMovementPos()
	{
		if(path.Count <= 1)
			return GetCurrentAgent().GetPos();
		else
			return path[1].pos;
	}
}
