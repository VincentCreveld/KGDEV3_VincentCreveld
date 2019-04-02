using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public void OnTriggerStay(Collider other) {
		IInteractable temp = other.GetComponent<IInteractable>();
		if(temp != null) {
			temp.CheckInput();
		}
	}
}
