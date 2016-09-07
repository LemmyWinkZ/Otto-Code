using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour {

	[Header("Public Variables")]
	public int healthPoints = 6;
	public int maxHealthPoints = 6;
	public bool godMode = false;
	public bool isAlive = true;
	public bool healthChange = false;
	public bool initialisedPlayerMesh = false;

	private Player2DController playerController;
	private GUIManager gm;
	private LevelManager lm;
	public GameObject[] meshObj = new GameObject[7];

	void Start() {
		lm = GameObject.Find ("Level Manager").GetComponent<LevelManager>();
		gm = GameObject.Find ("GUI Manager").GetComponent<GUIManager> ();
	}

	void Update() {
		if (healthPoints <= 0 || !isAlive) {
			Die ();
		}

		if (healthChange) {
			gm.SetHeartCount (healthPoints);
			healthChange = false;
		}

		InitMeshArray ();
	}

	public int GetHealth() {
		return healthPoints;
	}

	public void AddHealth() {
		if (healthPoints < maxHealthPoints) {
			healthPoints++;
			healthChange = true;
		}
	}

	public void AddHealth(int amount) {
		if (healthPoints + amount <= maxHealthPoints) {
			healthPoints += amount;
			healthChange = true;
		}
	}

	public void RemoveHealth() {
		if (!godMode) {
			healthPoints--;
			healthChange = true;
			StartCoroutine (DamageResistance ());
			DamageFlash ();
		}
	}

	public void RemoveHealth(int amount) {
		if (!godMode) {
			if (healthPoints - amount < 0) {
				healthPoints = 0;
				healthChange = true;
			} else {
				healthPoints -= amount;
				healthChange = true;
			}
			StartCoroutine (DamageResistance ());
			DamageFlash ();
		}
	}

	public void RemoveAllHealth() {
		if (!godMode) {
			healthPoints = 0;
			healthChange = true;
			DamageFlash ();
		}
	}

	public void Die() {
		healthPoints = 0;
		isAlive = false;

		playerController = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player2DController> ();
		playerController.characterControllable = false;
		Time.timeScale = 0;
	}

	public void DamageFlash() {
		StartCoroutine (DamageFlashCR ());
	}

	private IEnumerator DamageFlashCR() {
		for (int i = 0; i < 3; i++) {
			for (int x = 0; x < meshObj.Length; x++) {
				meshObj [x].GetComponent<SkinnedMeshRenderer> ().enabled = false;
			}
			yield return new WaitForSeconds(.1f);
			for (int y = 0; y < meshObj.Length; y++) {
				meshObj [y].GetComponent<SkinnedMeshRenderer> ().enabled = true;
			}
			yield return new WaitForSeconds(.1f);
		}
	}

	private void InitMeshArray() {
		if (lm.currentLevelIndex >= 1 && !initialisedPlayerMesh) {
			for (int i = 0; i < meshObj.Length; i++) {
				meshObj [i] = GameObject.FindGameObjectWithTag ("Player").transform.GetChild (2).transform.GetChild (i).gameObject;
			}
			initialisedPlayerMesh = true;
		}
	}

	private IEnumerator DamageResistance() {
		godMode = true;
		yield return new WaitForSeconds (1f);
		godMode = false;
	}
}
