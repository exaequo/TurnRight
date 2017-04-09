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



//	[SerializeField]bool firstStar = false;
//	[SerializeField]bool secondStar = true;
//	[SerializeField]bool thirdStar = true;
//
//	public bool FirstStar{
//		get{ return firstStar; }
//		set{ 
//			if (firstStar != value) {
//				onStarValueChange.Invoke (0, value);
//			}
//			firstStar = value;
//		}			
//	}
//
//	public bool SecondStar{
//		get{ return secondStar; }
//		set{
//			if (secondStar != value) {
//				onStarValueChange.Invoke (1, value);
//			}
//			secondStar = value;
//		}			
//	}
//
//	public bool ThirdStar{
//		get{ return thirdStar; }
//		set{ 
//			if (thirdStar != value) {
//				onStarValueChange.Invoke (2, value);
//			}
//			thirdStar = value;
//		}			
//	}
}
