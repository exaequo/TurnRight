using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreDisplay : MonoBehaviour {
	public SingleHighscoreDisplay SingleHighscorePrefab;
	public GameObject content;

	public void Init(Score score){
		List<SingleHighscore> listOfHighscores = new List<SingleHighscore> ();

		foreach (Transform child in content.transform) {
			Destroy (child.gameObject);
		}

		foreach (SingleHighscore highscore in score.highscores) {
			listOfHighscores.Add (highscore);
		}

		listOfHighscores.Sort(delegate(SingleHighscore a, SingleHighscore b) {
			return a.time.CompareTo(b.time);
		});

		foreach (SingleHighscore highscore in listOfHighscores) {
			SingleHighscoreDisplay disp = (SingleHighscoreDisplay)Instantiate (SingleHighscorePrefab, content.transform, false);
//			disp.transform.parent = content.transform;

			disp.Init (highscore, highscore == score.highscores [score.highscores.Count - 1]);
		}
	}

}
