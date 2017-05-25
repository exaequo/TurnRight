using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarDisplay : MonoBehaviour {
	public Image colorable;
	public Image[] wheels = new Image[2];
	public float wheelRotationSpeed = 1;

	public void StartRotatingWheels(){
		StartCoroutine (RotateWheelsCoroutine ());
	}

	IEnumerator RotateWheelsCoroutine(){
		while (true) {
			foreach (Image wheel in wheels) {
				wheel.transform.Rotate (Vector3.forward * Time.deltaTime * wheelRotationSpeed);
			}
			yield return null;
		}
	}

	public void StopRotatingWheels(){
		StopAllCoroutines ();
	}
}
