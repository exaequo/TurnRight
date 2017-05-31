using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BackgroundAnimatorControl : MonoBehaviour {
	[HideInInspector]public Animator anim;
	public GameObject objectToTurnOff;

	[HideInInspector]public UnityEvent onShouldStartLoading;

	void Start(){
		anim = GetComponent<Animator> ();
	}

	public void ShouldTurnOff(){
		if (objectToTurnOff != null) {
			Debug.Log ("TURNING OFF: " + objectToTurnOff.name);
			objectToTurnOff.SetActive (false);
		}
	}

	public void StartAnimation(bool withMotion = true){
		if (objectToTurnOff != null) {
			objectToTurnOff.SetActive (true);
		}
		if (withMotion) {
			anim.SetTrigger ("TurnOn");
		} else {
			anim.SetTrigger ("TurnOnMotionless");
		}
	}

	public void EndAnimation(){
		anim.SetTrigger ("TurnOff");
	}

	public void SelectAnimation(){
		anim.SetTrigger ("Select");
	}

	public void LoadingScreenTrigger(){
		onShouldStartLoading.Invoke ();
	}
}
