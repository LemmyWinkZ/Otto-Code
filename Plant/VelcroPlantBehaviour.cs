using UnityEngine;
using System.Collections;

public class VelcroPlantBehaviour : MonoBehaviour {

	private Rigidbody rb;
	private HealthManager hm;

	void Update() {
		if (Input.GetKeyDown (KeyCode.G)) {
			KnockBackPlayer ();
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.CompareTag ("Player")) {
			rb = col.gameObject.GetComponent<Rigidbody> ();
			if (rb != null) {
				hm = GameObject.Find ("Health Manager").GetComponent<HealthManager> ();

				if (!hm.godMode) {
					hm.RemoveHealth ();
					KnockBackPlayer ();
				}

			}
		}
	}

	void KnockBackPlayer() {
		StartCoroutine (KnockBackPlayerCR ());
	}

	private IEnumerator KnockBackPlayerCR() {
		float defX, defY;
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player2DController> ().characterControllable = false;

		defX = rb.velocity.x;
		defY = rb.velocity.y;

		rb.velocity = new Vector3 ((defX * -1), (defY * -1), 0);
		yield return new WaitForSeconds (0.3f);
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player2DController> ().characterControllable = true;
	}
}
