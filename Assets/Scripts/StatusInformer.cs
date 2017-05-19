using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatusInformer : MonoBehaviour {
	[HideInInspector]public UnityEvent onEnabled;
	[HideInInspector]public UnityEvent onDisabled;

	void OnEnable(){
		onEnabled.Invoke ();
	}

	void OnDisable(){
		onDisabled.Invoke ();
	}
}
