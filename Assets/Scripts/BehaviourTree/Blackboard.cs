using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard
{
	// static strings in blackboard class to prevent typo's in strings down the line
	public static string PLAYER_OBJ = "p_obj";
	public static string VISION_RANGE = "v_range";
	
	public static string DAMAGE = "e_dmg";
	public static string PLAYER_CURHEALTH = "p_health";
	public static string HIGH_ALERT = "p_alert";
	public static float PLAYER_MAX_HEALTH = 100;
	public static float PLAYER_LOW_HEALTH_THRESHOLD = 35;
	public static string PATHFINDING_COMP = "e_pathfindingComp";

	public static string ENEMY_ATTACK_RANGE = "e_attackRange";
	public static float E_ATTACK_RANGE = 1.5f;
	public static string MOVESPEED = "e_speed";
	public static float E_MOVESPEED = .002f;

	public static string OWN_ENEMY_OBJECT = "e_object";

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
		if(path.Count <= 0)
			return GetCurrentAgent().GetPos();
		else
			return path[1].pos;
	}
}
