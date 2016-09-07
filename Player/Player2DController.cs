using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody), typeof(CheckpointController), typeof(PickupController))]
public class Player2DController : MonoBehaviour
{

	[Header("Player Movement")]
	[Range(0, 5)]
	public float movementSpeed = 2f;
	[Range(1, 5)]
	public float movementMultiplier = 1f;
	public bool isMoving = false;
	public bool facingRight = true;
	[SerializeField]
	private float moveX = 0f;
	public bool onNot = false;

	[Header("Mobile Settings")]
	public bool rightButtonPressed = false, leftButtonPressed = false, jumpButtonPressed = false;

	[Header("Player Jump")]
	public bool grounded = false;
	public bool hasJumped = false;
	public float jumpHeight = 3.5f;
	[Range(1, 5)]
	public float jumpHeightMultiplier = 1f;
	[Tooltip("groundDetectionLength")]
	[Range(0, 1)]
	public float groundDetectionLength = 0.53f;             // Also the DrawRay length
	[Tooltip("groundDetectionRayScale")]
	[Range(0f, 0.5f)]
	public float groundDetectionRayScale = 0.01f;
	[Tooltip("groundDetectionRayHeight")]
	[Range(-0.5f, 0.5f)]
	public float groundDetectionRayHeight = 0.05f;
	[Range(-0.5f, 0.5f)]
	[Tooltip("groundDetectionRayOffset")]
	public float groundDetectionRayOffset = 0.05f;

	[Header("Wall Detection")]
	public bool wallDetected = false;
	[Tooltip("Stop Movement When Wall Detected")]
	public bool stopMovementWhenWallDetected = true;
	[Range(0, 1)]
	[Tooltip("wallDetectionLength")]
	public float wallDetectionLength = 0.5f;
	[Range(0f, 0.3f)]
	[Tooltip("wallDetectionRayScale")]
	public float wallDetectionRayScale = 0.01f;
	[Range(-0.5f, 0.5f)]
	[Tooltip("wallDetectionHeight")]
	public float wallDetectionHeight = 0.05f;
	[Space(10)]
	public bool onPlatform = false;
	public GameObject currentPlatform;
	public Vector3 platformVelocity;

	[Header("Misc")]
	public bool drawRays = true;

	[Header("Private Movement Variables")]
	[SerializeField]
	public Rigidbody rigidbody;
	public string hitObject;
	[SerializeField]
	private RaycastHit hit;
	[SerializeField]
	private float currentVelocity;                              // Debugging
	public bool characterControllable = true;
	public bool characterJumpable = true;
	public bool characterTurnable = true;
	[SerializeField]
	private float gravityForce = 10f;

	void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	void Update()
	{
		#region Not Movement Values
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			onNot = true;
			movementMultiplier = 2;
			jumpHeight = 6;
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			onNot = false;
			movementMultiplier = 1;
			jumpHeight = 3.5f;
		}

		#endregion

		#region Movement Keys
		// Detect Key Pressed
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) && characterControllable  && characterTurnable)
		{
			moveX = 1;
			if (!facingRight) FlipPlayer();
		}
		else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) && characterControllable && characterTurnable)
		{
			moveX = -1;
			if (facingRight) FlipPlayer();
		}

		// Detect Key Released
		if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
		{
			moveX = 0;
			isMoving = false;
		}
		else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
		{
			moveX = 0;
			isMoving = false;
		}

		if (moveX != 0 && characterControllable)
		{
			isMoving = true;
			MovePlayer();
		}
		else
		{
			isMoving = false;
		}
		#endregion

		#region Jump Key
		// Ground Detection Method
		grounded = isGrounded();
		if (grounded)
		{
			hasJumped = false;
		}

		// Wall Detection
		wallDetected = wallDetection();
		if (wallDetected)
		{
			characterControllable = false;
			characterTurnable = true;
		} else if (!wallDetected && !GetComponent<SlopeBehaviour>().inSlope) {
			characterControllable = true;
		}

		if (Input.GetKeyDown(KeyCode.Space) && grounded)
		{
			Jump();
		}
		#endregion

		#region PlatformPhysics
		if (platformDetection())
		{
			onPlatform = true;
			/*
            platformVelocity = currentPlatform.GetComponent<Rigidbody>().velocity;
            rigidbody.velocity += platformVelocity;
            */


		}
		else
		{
			onPlatform = false;
			transform.parent = null;
		}

		#endregion
	}

	void FixedUpdate() {

	}

	#region Triggers
	void OnTriggerEnter(Collider col) {
		if (col.gameObject.CompareTag("Kill Zone")) {
			gameObject.transform.position = GetComponent<CheckpointController>().currentCheckpoint.position;
		} else if (col.gameObject.CompareTag("Checkpoint")) {
			GetComponent<CheckpointController>().currentCheckpoint = col.gameObject.transform;
		}
	}
	#endregion

	#region Movement
	void MovePlayer()
	{
		// Translate movement
		transform.Translate(0, 0, ((movementSpeed * Time.deltaTime) * movementMultiplier) * moveX);

		// Look at rigidbody.MovePosition();
	}

	public void MovePlayer(int moveXInput)
	{
		if (characterControllable && characterTurnable)
		{
			moveX = moveXInput;

			if (moveXInput == 1 && !facingRight)
			{
				FlipPlayer();
			}
			else if (moveXInput == -1 && facingRight)
			{
				FlipPlayer();
			}
		}
	}

	public void FlipPlayer()
	{
		if (characterTurnable) {
			facingRight = !facingRight;
			transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, (transform.localScale.z * -1));
		}
	}

	#endregion

	#region Jump
	public void Jump()
	{
		if (isGrounded() && characterJumpable) {
			rigidbody.AddForce(new Vector2(0, jumpHeight * jumpHeightMultiplier), ForceMode.Impulse);
			hasJumped = true;
		}
	}
	#endregion

	#region groundDetection
	public bool isGrounded()
	{
		Vector3 behindOtto;
		Vector3 middleOtto;
		Vector3 infrontOtto;

		if (facingRight)
		{
			behindOtto = new Vector3((transform.position.x - groundDetectionRayScale) + groundDetectionRayOffset, transform.position.y + groundDetectionRayHeight, transform.position.z);
			middleOtto = new Vector3(transform.position.x + groundDetectionRayOffset, transform.position.y + groundDetectionRayHeight, transform.position.z);
			infrontOtto = new Vector3((transform.position.x + groundDetectionRayScale) + groundDetectionRayOffset, transform.position.y + groundDetectionRayHeight, transform.position.z);
		}
		else {
			behindOtto = new Vector3((transform.position.x - groundDetectionRayScale) - groundDetectionRayOffset, transform.position.y + groundDetectionRayHeight, transform.position.z);
			middleOtto = new Vector3(transform.position.x - groundDetectionRayOffset, transform.position.y + groundDetectionRayHeight, transform.position.z);
			infrontOtto = new Vector3((transform.position.x + groundDetectionRayScale) - groundDetectionRayOffset, transform.position.y + groundDetectionRayHeight, transform.position.z);
		}

		if (drawRays) {
			Debug.DrawRay(infrontOtto, Vector3.down * groundDetectionLength, Color.green);
			Debug.DrawRay(middleOtto, Vector3.down * groundDetectionLength, Color.yellow);
			Debug.DrawRay(behindOtto, Vector3.down * groundDetectionLength, Color.red);
		}

		if (Physics.Raycast(middleOtto, -Vector3.up, out hit, groundDetectionLength) || Physics.Raycast(infrontOtto, -Vector3.up, out hit, groundDetectionLength) || Physics.Raycast(behindOtto, -Vector3.up, out hit, groundDetectionLength))
		{
			if (hit.collider.gameObject.CompareTag("Platform"))
			{
				return true;
			}
			else if (hit.collider.gameObject.CompareTag("Plant")) {
				return true;
			}
			else if (hit.collider.gameObject.CompareTag("Untagged"))
			{
				return true;
			}
			else if (hit.collider.gameObject.CompareTag("Ground"))
			{
				return true;
			}
			else {
				return false;
			}
		}
		return false;
	}
	#endregion

	#region wallDetection
	public bool wallDetection()
	{
		Vector3 upperFrontOtto = new Vector3(transform.position.x, (transform.position.y + wallDetectionRayScale) + wallDetectionHeight, transform.position.z);
		Vector3 lowerFrontOtto = new Vector3(transform.position.x, (transform.position.y - wallDetectionRayScale) + wallDetectionHeight, transform.position.z);

		// Change direction rays cast depending on the direction the character is facing
		if (facingRight)
		{
			if (drawRays)
			{
				Debug.DrawRay(upperFrontOtto, Vector3.right * wallDetectionLength, Color.cyan);
				Debug.DrawRay(lowerFrontOtto, Vector3.right * wallDetectionLength, Color.magenta);
			}

			if (Physics.Raycast(upperFrontOtto, Vector3.right, out hit, wallDetectionLength) || Physics.Raycast(lowerFrontOtto, Vector3.right, out hit, wallDetectionLength))
			{
				if (hit.collider.gameObject.CompareTag("Wall") || hit.collider.gameObject.CompareTag("Ground") || hit.collider.gameObject.CompareTag("Platform") || hit.collider.gameObject.CompareTag("Plant"))
				{
					return true;
				}
				return false;
			}
			return false;
		}
		else
		{
			if (drawRays)
			{
				Debug.DrawRay(upperFrontOtto, Vector3.left * wallDetectionLength, Color.cyan);
				Debug.DrawRay(lowerFrontOtto, Vector3.left * wallDetectionLength, Color.magenta);
			}

			if (Physics.Raycast(upperFrontOtto, Vector3.left, out hit, wallDetectionLength) || Physics.Raycast(lowerFrontOtto, Vector3.left, out hit, wallDetectionLength))
			{
				if (hit.collider.gameObject.CompareTag("Wall") || hit.collider.gameObject.CompareTag("Ground") || hit.collider.gameObject.CompareTag("Platform") || hit.collider.gameObject.CompareTag("Plant"))
				{
					return true;
				}
				return false;
			}
			return false;
		}
	}
	#endregion

	#region platformDetection
	public bool platformDetection()
	{
		Vector3 behindOtto;
		Vector3 middleOtto;
		Vector3 infrontOtto;

		if (facingRight)
		{
			behindOtto = new Vector3((transform.position.x - groundDetectionRayScale) + groundDetectionRayOffset, transform.position.y + groundDetectionRayHeight, transform.position.z);
			middleOtto = new Vector3(transform.position.x + groundDetectionRayOffset, transform.position.y + groundDetectionRayHeight, transform.position.z);
			infrontOtto = new Vector3((transform.position.x + groundDetectionRayScale) + groundDetectionRayOffset, transform.position.y + groundDetectionRayHeight, transform.position.z);
		}
		else {
			behindOtto = new Vector3((transform.position.x - groundDetectionRayScale) - groundDetectionRayOffset, transform.position.y + groundDetectionRayHeight, transform.position.z);
			middleOtto = new Vector3(transform.position.x - groundDetectionRayOffset, transform.position.y + groundDetectionRayHeight, transform.position.z);
			infrontOtto = new Vector3((transform.position.x + groundDetectionRayScale) - groundDetectionRayOffset, transform.position.y + groundDetectionRayHeight, transform.position.z);
		}

		if (drawRays)
		{
			Debug.DrawRay(infrontOtto, Vector3.down * groundDetectionLength, Color.magenta);
			Debug.DrawRay(middleOtto, Vector3.down * groundDetectionLength, Color.gray);
			Debug.DrawRay(behindOtto, Vector3.down * groundDetectionLength, Color.cyan);
		}

		if (Physics.Raycast(middleOtto, -Vector3.up, out hit, groundDetectionLength) || Physics.Raycast(infrontOtto, -Vector3.up, out hit, groundDetectionLength) || Physics.Raycast(behindOtto, -Vector3.up, out hit, groundDetectionLength))
		{
			hitObject = hit.collider.gameObject.tag;
			if (hit.collider.gameObject.CompareTag("Platform"))
			{
				currentPlatform = hit.collider.gameObject;
				return true;
			}
			else if (hit.collider.gameObject.CompareTag("Untagged"))
			{
				currentPlatform = null;
				return false;
			}
			else if (hit.collider.gameObject.CompareTag("Ground"))
			{
				currentPlatform = null;
				return false;
			}
			return false;
		}
		return false;
	}

	#endregion

	#region Kill Zone Detection

	#endregion
}
