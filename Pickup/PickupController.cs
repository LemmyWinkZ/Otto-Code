using UnityEngine;
using System.Collections;

public class PickupController : MonoBehaviour {

	private GUIManager gm;
    private PlayerManager pm;

    void Start() {
        pm = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
		gm = GameObject.Find ("GUI Manager").GetComponent<GUIManager>();
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.CompareTag("Orb")) {
            Destroy(col.gameObject);
            pm.AddOrb();
            gm.UpdateOrbCount();
        }

        if (col.gameObject.CompareTag("Rock Pickup")) {
            Destroy(col.gameObject);
            pm.AddRock();
            gm.UpdateRockCount();
        }
    }
}
