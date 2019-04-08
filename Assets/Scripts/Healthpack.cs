using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealable
{
	void Heal(float val);
}

public class Healthpack : GameEntity
{
	public float healAmt = 25f;

	public Transform graphics;

	public float cooldown = 5f;

	private bool isActive = true;

	public override Vector3 GetPos()
	{
		return transform.position;
	}

	public void HealTarget(IHealable h)
	{
		if(isActive)
		{
			h.Heal(healAmt);
			StartCoroutine(Cooldown());
		}
	}

	private IEnumerator Cooldown()
	{
		isActive = false;
		graphics.gameObject.SetActive(false);
		yield return new WaitForSeconds(cooldown);
		isActive = true;
		graphics.gameObject.SetActive(true);
	}
}
