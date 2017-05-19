using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenDisplay : MonoBehaviour {
	public Button startButton;


	void Awake(){
		startButton.GetComponent<StatusInformer> ().onEnabled.AddListener (OnButtonEnabled);
	}

	void OnButtonEnabled(){
		GetComponentInParent<Animator> ().SetTrigger ("Start");
		startButton.interactable = false;
	}

	public void StartStartScreen(){
		startButton.interactable = true;
	}
}
