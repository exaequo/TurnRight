using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MasterInput : MonoBehaviour {
	public static float DEAD_SWIPE = 5.0f;
	public static MasterInput instance;

	[HideInInspector]
	public UnityEvent onBackPressed;

	public Text debugText;

	public Text timerText;

	RotatingWheel current;

	Vector2 touchStart;
	Vector2 touchLast;

	void Awake () {
		if (instance == null) {
			instance = this;
		}
		DEAD_SWIPE *= Screen.width / 512;
	}

	List<string> info = new List<string>();


	void Update() {
		if (Input.touchCount > 0){
			if (Input.GetTouch(0).phase == TouchPhase.Began) {
				current = CastRayFromPosition(Input.GetTouch(0).position, true);
				touchStart = Input.GetTouch (0).position;
				touchLast = touchStart;
				//DebugText ("Casting ray " + Input.GetTouch (0).position);
			}

			if (Input.GetTouch(0).phase == TouchPhase.Moved) {
				Vector2 touchCurrent = Input.GetTouch (0).position;
				if (current != null) {
					current.TouchSwipeBehaviour (touchCurrent - touchLast);
				}
				touchLast = touchCurrent;
			}

			if (Input.GetTouch(0).phase == TouchPhase.Ended){// && Input.GetTouch(0).deltaPosition.magnitude > 0) {
//				CastRayFromPosition(Input.GetTouch(0).position, true);
				//DebugText ("Ending ray " + Input.GetTouch (0).deltaPosition);
				if (current != null) {
					current.TouchEndBehaviour (true, Input.GetTouch(0).position - touchStart);
				}
			}
		}

//		if (Input.GetMouseButtonDown (0)) {
////			CastRayFromPosition (Input.mousePosition, false);
//		}
	}


	RotatingWheel CastRayFromPosition(Vector2 pos, bool withTouch){
		PointerEventData cursor = new PointerEventData (EventSystem.current);
		cursor.position = pos;
		List<RaycastResult> objectsHit = new List<RaycastResult> ();
		EventSystem.current.RaycastAll(cursor, objectsHit);

//		float cameraWidth = Camera.main.ScreenToWorldPoint (new Vector2(Screen.width,0)).x - Camera.main.ScreenToWorldPoint (new Vector2(0,0)).x;


		for (int i = 0; i < objectsHit.Count; i++) {

			RectTransform rectTransform = objectsHit [i].gameObject.GetComponent<RectTransform> ();

			RotatingWheel grabber = objectsHit [i].gameObject.GetComponent<RotatingWheel> ();

			RayInterrupter interrupt = objectsHit [i].gameObject.GetComponent<RayInterrupter> ();

			if (interrupt != null) {
				return null;
			}

			if (grabber != null) {
				Vector2 localPoint;
				RectTransformUtility.ScreenPointToLocalPointInRectangle (rectTransform, pos, Camera.main, out localPoint);
				Image image = objectsHit [i].gameObject.GetComponent<Image> ();

				int pixelX = Mathf.RoundToInt ((((localPoint.x + (rectTransform.rect.width / 2.0f)) / (rectTransform.rect.width)) * image.sprite.texture.width));
				int pixelY = Mathf.RoundToInt ((((localPoint.y + (rectTransform.rect.height / 2.0f)) / (rectTransform.rect.height)) * image.sprite.texture.height));

				//Debug.Log ("PIXEL " + new Vector2 (pixelX, pixelY));

				if (image.sprite.texture.GetPixel (pixelX, pixelY).a != 0) {
					return grabber;
				}

				//DebugText ("LOCAL POINT " + localPoint + "width: " + rectTransform.rect.width);


			}
		}
		return null;
	}

	public void DebugText(string text){
		if (info.Count > 3) {
			info.RemoveAt (0);
		}
		info.Add (Time.time.ToString("#0.00") + ": " + text);
		string result = "";
		foreach (string txt in info) {
			result += txt + "\n";
		}
		Debug.Log (text);
		debugText.text = result;
	}

	public void SetTimer(float time){
		timerText.text = time.ToString ("#0.00");
	}


}
