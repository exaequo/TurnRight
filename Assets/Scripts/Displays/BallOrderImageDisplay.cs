using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallOrderImageDisplay : MonoBehaviour {
	public Image foreground;
	public Outline outline;


	void Start(){
		outline = GetComponentInChildren<Outline> ();
//		outline.enabled = false;
	}
	public void End(){
		GetComponent<Animator> ().SetTrigger ("End");
	}

	public Outline GetOutline(){
		if (outline == null) {
			outline = GetComponentInChildren<Outline> ();
		}
		return outline;
	}
}
