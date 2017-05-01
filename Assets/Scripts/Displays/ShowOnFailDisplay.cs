using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowOnFailDisplay : MonoBehaviour {
	public Image shouldveImage;
	public Image wasImage;

	public void Init(int was, int shouldve){
		if (wasImage != null) {
			wasImage.color = MasterController.instance.ballColorCodes [was];
		}
		if (shouldveImage != null) {
			shouldveImage.color = MasterController.instance.ballColorCodes [shouldve];
		}
		GetComponent<Animator> ().SetTrigger ("Start");
	}

	public void ResetStars(){}
	public void StartStar(int which){}
		
}
