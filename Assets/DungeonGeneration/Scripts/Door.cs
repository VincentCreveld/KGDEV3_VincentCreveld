using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour, IInteractable {
	private Animator anim;
	private void Awake() {
		anim = GetComponent<Animator>();
	}
	public void Act() {
		anim.SetTrigger("Act");
	}

	public void CheckInput() {
		if(Input.GetKeyDown(KeyCode.E))
			Act();
	}
}
