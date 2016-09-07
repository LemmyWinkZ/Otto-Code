// 2D Smooth Follow Camera Controller
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{
    [Header("Target Object")]
    public Transform target;

    [Header("Camera Settings")]
    [Range(-20, 0)]
    public float distance = -10f;
    [Range(0, 5)]
    public float distanceSmoothing = 2f;

    [Space(10)]
    [Range(-5, 10)]
    public float height = 5f;
    [Range(0, 5)]
    public float heightSmoothing = 2f;

    [Space(10)]
    [Range(0, 5)]
    public float offset = 1f;
    private float negOffset;
    [Range(0, 5)]
    public float offsetSmoothing = 5f;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        negOffset = -offset;
		target = player.transform;
    }

    void LateUpdate()
    {
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        // Early out if we don't have a target
        if (!target)
            return;

        // Calculate the current values
        float wantedHeight = target.position.y + height;
        float wantedOffset = target.position.x + offset;
        float wantedDistance = target.position.z + distance;
        if (target.GetComponent<Player2DController>().facingRight == false)
        {
            wantedOffset = target.position.x + negOffset;
        }

        float currentHeight = transform.position.y;
        float currentOffset = transform.position.x;
        float currentDistance = transform.position.z;

        // Damp the rotation around the y-axis
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightSmoothing * Time.deltaTime);

        // Damp the offset
        currentOffset = Mathf.Lerp(currentOffset, wantedOffset, offsetSmoothing * Time.deltaTime);

        // Damp the distance
        currentDistance = Mathf.Lerp(currentDistance, wantedDistance, distanceSmoothing * Time.deltaTime);

        // Set the offset, height & distance of the camera
        transform.position = new Vector3(currentOffset, currentHeight, currentDistance);
    }

	public void UpdateCameraDistance() {
		distance = GameObject.Find ("Camera Zoom").GetComponent<Slider> ().value;
	}
}
