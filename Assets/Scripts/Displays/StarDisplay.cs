using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarDisplay : MonoBehaviour {
	public Image frontImage;
	public Image backImage;
	public Animator animator;

	public void Init(bool value){
		if (animator != null) {
			animator.SetBool ("IsOn", value);
			if (value) {
				animator.SetTrigger ("Start");
			} else {
//				Debug.Log ("OFFING");
				animator.SetTrigger ("Off");
			}

		} else {
			frontImage.gameObject.SetActive (value);
		}
	}

}
