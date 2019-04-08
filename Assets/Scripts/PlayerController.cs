using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public interface IDamagable
{
	void DealDamage(float val);
}

public class PlayerController : GameEntity, IDamagable, IHealable
{
	public float damageCooldown = 1f;
	private bool isDamagable = true;

	public LayerMask shootingMask;

	public Text hp;

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

			if(currentHealth >= maxHealth)
				currentHealth = maxHealth;

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
		hp.text = "HP: " + (int)currentHealth;

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
			RaycastHit hit;
			Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 5, Color.red, 0f);
			Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
			if(Physics.Raycast(ray, out hit, 5f, shootingMask))
			{
				Debug.Log(hit.transform.name);
				if(hit.transform.GetComponent<IDamagable>() != null)
					hit.transform.GetComponent<IDamagable>().DealDamage(10f);
				
				if(hit.transform.GetComponentInParent<Healthpack>() != null)
					hit.transform.GetComponentInParent<Healthpack>().HealTarget(this);
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

	public void Heal(float val)
	{
		CurrentHealth += val;
	}

	// Hacky cooldown mechanic so i don't have to track damage cooldowns on individual enemies.
	private IEnumerator DmgCooldown()
	{
		isDamagable = false;
		yield return new WaitForSeconds(damageCooldown);
		isDamagable = true;
	}

	// Reload scene on death
	private void Die()
	{
		Debug.LogError("Game over!");
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public override Vector3 GetPos()
	{
		return transform.position;
	}
}
