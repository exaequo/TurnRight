using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarPaintDisplay : MonoBehaviour {
	public CarDisplay car;
	public Image colorBallImage;
	Animator anim;
	Color currentColor = Color.red;
	//TODO random sprity dla różnych samochodów
	List<Color> colorsToPaint = new List<Color>();

	void Start(){
		anim = GetComponent<Animator> ();
	}

	void OnEnable(){
		if (anim != null) {
			anim.SetBool ("Ended", false);
		}
	}
	public IEnumerator PaintCarCoroutine(){
		if (car != null) {
			float paintTime = Time.time + 20 * Time.deltaTime;
			while (Time.time <= paintTime) {
				car.colorable.color = Color.Lerp (car.colorable.color, currentColor, 0.1f);
				yield return null;
			}
		}
	}

	public void Init(List<Color> colors){
		colorsToPaint = colors;

		PaintNextCar ();
	}

	public void PaintNextCar(){
		if (colorsToPaint.Count > 0) {
			currentColor = colorsToPaint [0];
			colorsToPaint.RemoveAt (0);
			if (colorBallImage != null) {
				colorBallImage.color = currentColor;
			}
			if (anim == null) {
				anim = GetComponent<Animator> ();
			}
			anim.SetTrigger ("PaintCar");
		} else {
			anim.SetBool ("Ended", true);
		}
	}

	public void ChangeCarSprite(){
		if (car != null) {
			car.colorable.color = Color.white;
		}
	}

	public void StartRotatingCarWheels(){
		if (car != null) {
			car.StartRotatingWheels ();
		}
	}

	public void StopRotatingCarWheels(){
		if (car != null) {
			car.StopRotatingWheels ();
		}
	}

}
