using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float maxHealth = 100f;
	private float currentHealth;

	public float CurrentHealth {
		get
		{
			return currentHealth;
		}
		set
		{
			currentHealth = value;
			if(currentHealth <= 0)
				Die();
		}
	}

	private void Awake()
	{
		CurrentHealth = maxHealth;
	}

	public void Update()
	{
		if(Input.GetKeyDown(KeyCode.R))
		{
			CurrentHealth -= 10f;
		}

		if(Input.GetKeyDown(KeyCode.T))
		{
			CurrentHealth = 50f;
		}
	}

	private void Die()
	{
		Debug.LogError("Game over!");
	}

	public Vector3 GetPos()
	{
		return transform.position;
	}
}
