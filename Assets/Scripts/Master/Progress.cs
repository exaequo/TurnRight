using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Progress{

	public List<LevelScript.LevelInfo> levelInfos = new List<LevelScript.LevelInfo> ();

	public string PrintInfo(){
		string mess = "";
		foreach (LevelScript.LevelInfo info in levelInfos) {
			mess += info.levelNumber + ": " + ParseBool(info.oldScore.stars [0]) + ", " + ParseBool(info.oldScore.stars [1]) + ", " + ParseBool(info.oldScore.stars [2]) + "||";
		}
		return mess;
	}

	string ParseBool(bool val){
		if (val) {
			return "T";
		} else {
			return "F";
		}
	}

	public void Replace(Progress progress){
		levelInfos.Clear ();
		foreach (LevelScript.LevelInfo info in progress.levelInfos) {
			levelInfos.Add (info);
		}
	}
}
