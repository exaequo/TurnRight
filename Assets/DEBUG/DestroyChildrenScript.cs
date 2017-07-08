using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyChildrenScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		foreach (Transform child in transform) {
			Destroy (child.gameObject);
		}
	}

}
