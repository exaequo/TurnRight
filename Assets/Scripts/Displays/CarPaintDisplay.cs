using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarPaintDisplay : MonoBehaviour {
	public Image carImage;
	public Image colorBallImage;
	Animator anim;
	Color currentColor = Color.red;
	//TODO random sprity dla różnych samochodów
	List<Color> colorsToPaint = new List<Color>();

	void Start(){
		anim = GetComponent<Animator> ();
	}

	public IEnumerator PaintCarCoroutine(){
		if (carImage != null) {
			float paintTime = Time.time + 20 * Time.deltaTime;
			while (Time.time <= paintTime) {
				carImage.color = Color.Lerp (carImage.color, currentColor, 1 / 20.0f);
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
		}
	}

	public void ChangeCarSprite(){
		if (carImage != null) {
			carImage.color = Color.white;
		}
	}
}
