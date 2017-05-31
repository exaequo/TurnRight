using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarDisplay : MonoBehaviour {
	public Image colorable;
	public Image uncolorable;
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

	public void SwapInfo(CarDisplay car){
		colorable.overrideSprite = car.colorable.sprite;
		uncolorable.overrideSprite = car.uncolorable.sprite;

		for (int i = 0; i < 2; i++) {
			wheels [i].rectTransform.anchorMin = car.wheels [i].rectTransform.anchorMin;
			wheels [i].rectTransform.anchorMax = car.wheels [i].rectTransform.anchorMax;
			wheels [i].rectTransform.anchoredPosition = car.wheels [i].rectTransform.anchoredPosition;
			wheels [i].overrideSprite = car.wheels [i].sprite;
		}
	}
}
