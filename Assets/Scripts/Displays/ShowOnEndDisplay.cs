using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnEndDisplay : MonoBehaviour {
	public StarDisplay[] stars = new StarDisplay[3];
	public bool[] starAchievability = { false, false, false };
	[HideInInspector]public bool showHighscore = false;

	public void Init(bool[] starsGotten){
		for (int i = 0; i < 3; i++) {
			starAchievability [i] = starsGotten [i];
		}
	}

	public void ResetStars(){
		foreach (StarDisplay star in stars) {
			star.Init (false);
		}
	}

	public void StartStar(int value){
		stars [value].Init (starAchievability[value]);
	}

	public void ShowHighscore(){
		if (showHighscore) {
			GetComponent<Animator> ().SetTrigger ("Highscore");
		}
	}
}
