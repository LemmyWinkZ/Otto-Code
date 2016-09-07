using UnityEngine;
using System.Collections;

public class SlopeBehaviour : MonoBehaviour {

	public Player2DController pc;
	[SerializeField]
	public bool inSlope = false;
	private float defaultMoveSpeed;

	void Start () {
		pc = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player2DController> ();
		defaultMoveSpeed = pc.movementSpeed;
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.CompareTag ("Slope Start")) {
			Debug.Log ("Entered Slope Trigger");
			inSlope = true;
			pc.characterControllable = false;
			pc.characterJumpable = false;
			pc.characterTurnable = false;
		} else if (col.gameObject.CompareTag ("Slope End")) {
			Debug.Log("Exitted Slope Trigger");
			inSlope = false;
			pc.characterControllable = true;
			pc.characterJumpable = true;
			pc.characterTurnable = true;
		}
	}
}
