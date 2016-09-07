using UnityEngine;
using System.Collections;

public class PlatformBehaviour : MonoBehaviour {

	public enum PlatformDirection {
		Up,
		Right,
		Left,
		Down
	};

	[Header("Platform Settings")]
	public PlatformDirection direction;
	public float distance;
	public float time;

    [Space(10)]
    public bool easing = true;

    [Space(10)]
	public bool addDelay = false;
	public float delayTime = 1f;

    [SerializeField]
	private Vector3 startPosition;
    [SerializeField]
    private bool firstStage = true;
    [SerializeField]
    private Vector3 endPosition;
    [SerializeField]
    private float startTime;
    [SerializeField]
    private float journeyLength;

	void Start () {
		startPosition = transform.position;


		CalculateEndPosition (direction);
		journeyLength = Vector3.Distance (startPosition, endPosition);
	}

	void Update () {
		// Add Delay logic 
		if (addDelay) {
			StartCoroutine (movePlatformCo ());
		} else {
			movePlatform ();
		}

	}

	void CalculateEndPosition(PlatformDirection pDir) {
		switch(pDir) {
		case PlatformDirection.Up:
			endPosition = new Vector3(startPosition.x, startPosition.y + distance, startPosition.z);
			break;

		case PlatformDirection.Right:
			endPosition = new Vector3(startPosition.x + distance, startPosition.y, startPosition.z);
			break;

		case PlatformDirection.Left:
			endPosition = new Vector3(startPosition.x - distance, startPosition.y, startPosition.z);
			break;

		case PlatformDirection.Down:
			endPosition = new Vector3(startPosition.x, startPosition.y - distance, startPosition.z);
			break;
		}
	}

	void movePlatform() {
		if (firstStage) {
			startTime += Time.deltaTime;
			CalculateEndPosition (direction);
		} else {
			startTime -= Time.deltaTime;
		}
		
		if (startTime <= time) {
            float t = startTime / time;
            if (easing) {
                t = Mathf.Sin(t * Mathf.PI * 0.5f);
            }
			transform.position = Vector3.Lerp (startPosition, endPosition, t);
		} else {
			firstStage = false;
        }

		if (startTime <= 0 && !firstStage) {
			firstStage = true;
		}
	}

	IEnumerator movePlatformCo() {
        if (firstStage)
        {
            startTime += Time.deltaTime;
            CalculateEndPosition(direction);
        }
        else {
            startTime -= Time.deltaTime;
        }

        if (startTime <= time)
        {
            float t = startTime / time;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
        }
        else
        {
            firstStage = false;
        }

        if (startTime <= 0 && !firstStage)
        {
            firstStage = true;
        }
        yield return new WaitForSeconds (delayTime);
	}
}
