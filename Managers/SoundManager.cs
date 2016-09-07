using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	private LevelManager lm;

	void Start() {
		lm = GameObject.Find ("Level Manager").GetComponent<LevelManager> ();
	}

	void Update() {
		if (lm.levelChangeDetected) {
			Debug.Log ("Level Change Detected in Sound Manager");
		}
	}
}
