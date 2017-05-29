using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePath : MonoBehaviour {
	//public List<Transform> nodes = new List<Transform>();
	List<Transform> nodes = new List<Transform>();
	public Color pathColor = Color.red;
	public bool pathVisible = true;

	[SerializeField]
	private bool isEnd = false;
	public bool IsEnd{ get { return isEnd; } }
	// Use this for initialization
	void Awake () {
		foreach (Transform child in transform) {
			nodes.Add (child);
		}
	}

	void OnDrawGizmosSelected(){
		if(pathVisible){
			
			if(transform.childCount > 0){
				//if (nodes.Count == 0) {
				nodes.Clear();
					foreach (Transform child in transform) {
						nodes.Add (child);
					}
				//}
//
				iTween.DrawPath(GetPath(), pathColor);
			}	
		}
	}

	public Transform[] GetPath(){
		return nodes.ToArray ();
	}

	public Transform[] GetPathReversed(){
		List<Transform>  revNodes = nodes.GetRange(0, nodes.Count);
		revNodes.Reverse();
		return revNodes.ToArray();
	}
}
