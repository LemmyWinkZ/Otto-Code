using UnityEngine;
using System.Collections;

public class NotCameraMovement : MonoBehaviour {

    private CameraController cc;

	void Start () {
        cc = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
	}

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.CompareTag("Player")) {
            cc.distance = -4;
            cc.offset = 1.5f;
            cc.heightSmoothing = 5f;
        }
    }
}
