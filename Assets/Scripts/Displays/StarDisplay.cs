using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarDisplay : MonoBehaviour {
	public Image frontImage;
	public Image backImage;
	public Animator animator;

//	public IEnumerator RemoveStar(){
//		if (animator != null) {
//			animator.SetTrigger ("End");
//			yield return new WaitForSeconds (1);
//			frontImage.gameObject.SetActive (false);
//		} else {
//			frontImage.gameObject.SetActive (false);
//		}
//		yield return null;
//	}

	public void Init(bool value){
		Debug.Log (value + "Init" + Time.time);
		//frontImage.gameObject.SetActive (value);

		if (animator != null) {
			animator.SetBool ("IsOn", value);
			if (value) {
				animator.SetTrigger ("Start");
			} else {
				Debug.Log ("OFFING");
				animator.SetTrigger ("Off");
			}

		}
	}

}
