using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallOrderDisplay : MonoBehaviour {
	List<int> ballOrder;
	List<BallOrderImageDisplay> ballImages = new List<BallOrderImageDisplay>();
	public BallOrderImageDisplay ballImagePrefab;

	public void Init(List<int> ballOrder){
		this.ballOrder = ballOrder;
		GridLayoutGroup grid = GetComponent<GridLayoutGroup> ();
		float cellSize = GetComponent<RectTransform> ().rect.height * 0.8f;
		grid.cellSize = new Vector2 (cellSize, cellSize);
		grid.padding.left = (int)cellSize / 4;
		grid.spacing = new Vector2(cellSize / 4, 0);

		GetComponent<Animator> ().SetTrigger ("Start");
	}

	public IEnumerator SpawnBalls(){
		foreach (int which in ballOrder) {
			BallOrderImageDisplay ballImage = (BallOrderImageDisplay) Instantiate (ballImagePrefab, transform, false);

			ballImage.foreground.color = MasterController.instance.ballColorCodes [which];
			ballImages.Add (ballImage);
			float outlineSize = GetComponent<RectTransform> ().rect.height * 0.03f;
			if (ballImage.outline == null) {
				ballImage.outline = ballImage.foreground.GetComponent<Outline> ();
			}
			ballImage.outline.effectDistance = new Vector2 (outlineSize, -outlineSize);
			ballImage.outline.enabled = false;

			yield return new WaitForSeconds (0.1f);
		}
		ballImages [0].outline.enabled = true;
	}

	public IEnumerator DestroyFirst(){
		if (ballImages.Count > 0) {
			BallOrderImageDisplay ball = ballImages [0];
			ballImages.RemoveAt (0);

			ball.End ();
			yield return new WaitForSeconds (1);
			Destroy (ball.gameObject);
		}

		if (ballImages.Count > 0) {
			ballImages [0].outline.enabled = true;
		}
	}
}
