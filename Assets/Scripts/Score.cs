using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable()]public class IntBoolEvent : UnityEvent<int,bool>{}
[System.Serializable]
public class Score {
	[HideInInspector]public IntBoolEvent onStarValueChange;

	public bool[] stars = { false, false, false };
	public void ChangeStar(int which, bool value){
		if(which >= 0 && which < 3){
			stars[which] = value;
			onStarValueChange.Invoke(which, value);
		}
	}

	public void ChangeStarScore(bool[] newStars){
		for (int i = 0; i < 3; i++) {
			stars [i] = stars [i] || newStars [i];
		}
	}
}
