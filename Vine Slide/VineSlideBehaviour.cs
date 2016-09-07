using UnityEngine;
using System.Collections;

public class VineSlideBehaviour : MonoBehaviour {

	[Header("Vine Slide Settings")]
	public bool onVine = false;
	public bool hasSlid = false;
	public float time = 1f;
	public float timeOnSlide = 0f;
	public float startTime = 0f;
	public float distance = 0f;
	public float distanceTravelled = 0f;

	public Transform startPoint, endPoint, currentPlayerPosition;
	public Vector3 playerPos;
	public GameObject player;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void Update () {
		if (onVine && !hasSlid) {
			timeOnSlide += Time.deltaTime;
			currentPlayerPosition = player.gameObject.transform;
			playerPos = player.gameObject.transform.position;
			VineSlidePlayer ();
		}

		if (timeOnSlide >= 0.1f && !hasSlid) {
			player.GetComponent<Player2DController> ().rigidbody.useGravity = false;
		}
			
		if (onVine && currentPlayerPosition.position.x >= (endPoint.position.x - .10f)) {
			player.GetComponent<Player2DController> ().characterControllable = true;
			hasSlid = true;
			player.GetComponent<Player2DController> ().rigidbody.useGravity = true;
			onVine = false;
		}
	}

	void OnTriggerEnter(Collider col) {
		currentPlayerPosition = player.gameObject.transform;
	
		if (col.gameObject.CompareTag("Player")) {
			onVine = true;
			startPoint = currentPlayerPosition;
			distance = Vector3.Distance (currentPlayerPosition.position, endPoint.position);
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;

            if (!player.GetComponent<Player2DController>().facingRight) player.GetComponent<Player2DController>().FlipPlayer();

        }
	}

	void VineSlidePlayer() {
		startTime += Time.deltaTime;
		player.GetComponent<Player2DController> ().characterControllable = false;
		player.transform.position = Vector3.Lerp (startPoint.position, new Vector3(endPoint.position.x + 0.5f, endPoint.position.y, endPoint.position.z), startTime / time);
		distanceTravelled = Vector3.Distance (startPoint.position, currentPlayerPosition.position);
	}
}