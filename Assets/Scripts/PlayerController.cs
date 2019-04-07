using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
	void DealDamage(float val);
}

public class PlayerController : MonoBehaviour, IDamagable
{
	public float damageCooldown = 1f;
	private bool isDamagable = true;

	public LayerMask shootingMask;

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
		if(Input.GetMouseButtonDown(0))
		{
			Debug.Log("Here");
			RaycastHit hit;
			Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 5, Color.red, 0f);
			Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
			if(Physics.Raycast(ray, out hit, 5f, shootingMask))
			{
				Debug.Log(hit.transform.name);
				if(hit.transform.GetComponent<IDamagable>() != null)
					hit.transform.GetComponent<IDamagable>().DealDamage(10f);
			}
		}
	}

	public void DealDamage(float val)
	{
		if(isDamagable)
		{
			CurrentHealth -= val;
			StartCoroutine(DmgCooldown());
		}
	}

	// Hacky cooldown mechanic so i don't have to track damage cooldowns on individual enemies.
	private IEnumerator DmgCooldown()
	{
		isDamagable = false;
		yield return new WaitForSeconds(damageCooldown);
		isDamagable = true;
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
