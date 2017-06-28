using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable()]public class IntBoolEvent : UnityEvent<int,bool>{}
[System.Serializable]
public class SingleHighscore{
	public string date = System.DateTime.Now.ToString();
	public float time = 0;
	public int numberOfRotations = 0;
	public float speedUpTime = 0;
}

[System.Serializable]
public class Score {
	[HideInInspector]public IntBoolEvent onStarValueChange;
	public List<SingleHighscore> highscores = new List<SingleHighscore> ();

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

	public bool AddHighscore(SingleHighscore highscore){
		bool value = true;
		highscores.Add (highscore);
		if (highscores.Count > 6) {
			List<SingleHighscore> sorted = new List<SingleHighscore> ();
			foreach (SingleHighscore high in highscores) {
				sorted.Add (high);
			}

			sorted.Sort(delegate(SingleHighscore a, SingleHighscore b) {
				return a.time.CompareTo(b.time);
			});

			for (int i = 0; i < highscores.Count; i++) {
				if (highscores[i] == sorted [sorted.Count - 1]) {
					if (highscores [i] == highscore) {
						value = false;
					}
					highscores.RemoveAt (i);
					break;
				}
			}
		}
		return value;
	}
}


