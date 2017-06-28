using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleHighscoreDisplay : MonoBehaviour {
	public Image background;
	public Text dateText;
	public Text timeText;
	public Text speedUpTimeText;
	public Text rotationsText;

	public Color specialHighlightColor = Color.red;

	public void Init(SingleHighscore highscore, bool specialHighlight = false){
		GetComponent<LayoutElement> ().preferredHeight = Screen.height / 15.0f;
		GetComponent<LayoutElement> ().minHeight = Screen.height / 15.0f;

		if (specialHighlight) {
			background.color = specialHighlightColor;
		}
		if (dateText != null) {
			dateText.text = highscore.date;
		}
		if (timeText != null) {
			timeText.text = highscore.time.ToString ("##.00");
		}
		if (speedUpTimeText != null) {
			speedUpTimeText.text = highscore.speedUpTime.ToString ("##.00");
		}
		if (rotationsText != null) {
			rotationsText.text = highscore.numberOfRotations.ToString ();
		}
	}
}

